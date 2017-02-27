using roguelike.Entities.Monsters;
using RogueSharp;
using RogueSharp.DiceNotation;
using SadConsole.Game;
using System.Collections.Generic;


namespace roguelike.Core
{
    // Our custom DungeonMap class extends the base RogueSharp Map class
    public class DungeonMap : Map
    {
        public List<Rectangle> Rooms;
        private readonly List<Monster> _monsters;
		public List<Door> Doors { get; set; }

		public DungeonMap()
        {
            // Initialize the list of rooms when we create a new DungeonMap
            Rooms = new List<Rectangle>();
            _monsters = new List<Monster>();
			//Doors = new List<Door>();
		}

        public Microsoft.Xna.Framework.Point getPlayerStartingPosition()
        {
            return new Microsoft.Xna.Framework.Point(Rooms[0].Center.X, Rooms[0].Center.Y);
        }

        public List<Monster> getMonsters(Microsoft.Xna.Framework.Point renderOffset)
        {
            List<Monster> monsters = new List<Monster>();
            foreach (var room in Rooms)
            {
                // Each room has a 60% chance of having monsters
                if (Dice.Roll("1D10") < 7)
                {
                    // Generate between 1 and 4 monsters
                    var numberOfMonsters = Dice.Roll("1D4");
                    for (int i = 0; i < numberOfMonsters; i++)
                    {
                        // Find a random walkable location in the room to place the monster
                        Point randomRoomLocation = GetRandomWalkableLocationInRoom(room);
                        // It's possible that the room doesn't have space to place a monster
                        // In that case skip creating the monster
                        if (randomRoomLocation != null)
                        {
                            // Temporarily hard code this monster to be created at level 1
                            Monster monster = new Kobold(1);
                            
                            monster.Position = new Microsoft.Xna.Framework.Point(randomRoomLocation.X, randomRoomLocation.Y);
                            //monster.AbsoluteArea = new Microsoft.Xna.Framework.Rectangle(monster.Position, new Microsoft.Xna.Framework.Point(1, 1));                    
                            monster.RenderOffset = renderOffset;
                            monsters.Add(monster);
                        }
                    }
                }
            }
            return monsters;
        }
        /*
        // The Draw method will be called each time the map is updated
        // It will render all of the symbols/colors for each cell to the map sub console
        public void Draw(RLConsole mapConsole, RLConsole statConsole)
        {
            mapConsole.Clear();
            foreach (Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }

            int i = 0;
            foreach (Monster monster in _monsters)
            {
	            if (!IsInFov(monster.X, monster.Y)) continue;

				monster.Draw(mapConsole, this);
	            monster.DrawStats(statConsole, i);
	            i++;
            }
			_monsters.ForEach(m => m.Draw(mapConsole,this));
			Doors.ForEach(d => d.Draw(mapConsole, this));
        }     
       

        // This method will be called any time we move the player to update field-of-view
        public void UpdatePlayerFieldOfView()
        {
            Player player = Game.Player;
            // Compute the field-of-view based on the player's location and awareness
            ComputeFov(player.X, player.Y, player.Awareness, true);
            // Mark all cells in field-of-view as having been explored
            foreach (Cell cell in GetAllCells())
            {
                if (IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }
        

        // Returns true when able to place the Actor on the cell or false otherwise
        public bool SetActorPosition(Actor actor, int x, int y)
        {
	        if (!GetCell(x, y).IsWalkable) return false;

	        // The cell the actor was previously on is now walkable
	        SetIsWalkable(actor.X, actor.Y, true);
	        // Update the actor's position
	        actor.X = x;
	        actor.Y = y;
	        // The new cell the actor is on is now not walkable
	        SetIsWalkable(actor.X, actor.Y, false);
	        // Try to open a door if one exists here
	        OpenDoor(actor, x, y);

	        if (actor is Player)
	        {
		        UpdatePlayerFieldOfView();
	        }

	        return true;
        }
         */

        // A helper method for setting the IsWalkable property on a Cell
        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            Cell cell = GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }

        /*
        // Called by MapGenerator after we generate a new map to add the player to the map
        public void AddPlayer(Player player)
        {
            Game.Player = player;
            SetIsWalkable(player.X, player.Y, false);
            UpdatePlayerFieldOfView();
			Game.SchedulingSystem.Add(player);
		}
        
        public void AddMonster(Monster monster)
        {
            _monsters.Add(monster);
            // After adding the monster to the map make sure to make the cell not walkable
            SetIsWalkable(monster.X, monster.Y, false);
			Game.SchedulingSystem.Add(monster);
		}
        */

        // Look for a random location in the room that is walkable.
        public Point GetRandomWalkableLocationInRoom(Rectangle room)
        {
	        if (!DoesRoomHaveWalkableSpace(room)) return null;
	        for (int i = 0; i < 100; i++)
	        {
		        int x = Game.Random.Next(1, room.Width - 2) + room.X;
		        int y = Game.Random.Next(1, room.Height - 2) + room.Y;
		        if (IsWalkable(x, y))
		        {
			        return new RogueSharp.Point(x, y);
		        }
	        }

	        // If we didn't find a walkable location in the room return null
            return null;
        }

        // Iterate through each Cell in the room and return true if any are walkable
        public bool DoesRoomHaveWalkableSpace(Rectangle room)
        {
            for (int x = 1; x <= room.Width - 2; x++)
            {
                for (int y = 1; y <= room.Height - 2; y++)
                {
                    if (IsWalkable(x + room.X, y + room.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /*
		public void RemoveMonster(Monster monster)
		{
			_monsters.Remove(monster);
			// After removing the monster from the map, make sure the cell is walkable again
			SetIsWalkable(monster.X, monster.Y, true);
			Game.SchedulingSystem.Remove(monster);
		}

		public Monster GetMonsterAt(int x, int y)
		{
			return _monsters.FirstOrDefault(m => m.X == x && m.Y == y);
		}

		public Door GetDoor(int x, int y)
		{
			return Doors.SingleOrDefault(d => d.X == x && d.Y == y);
		}
		
		private void OpenDoor(Actor actor, int x, int y)
		{
			Door door = GetDoor(x, y);
			if (door != null && !door.IsOpen)
			{
				door.IsOpen = true;
				var cell = GetCell(x, y);
				// Once the door is opened it should be marked as transparent and no longer block field-of-view
				SetCellProperties(x, y, true, cell.IsWalkable, cell.IsExplored);

				Game.MessageLog.Add($"{actor.Name} opened a door");
			}
		}
        */
	}
}