using System.Collections.Generic;
using roguelike.Entities;
using roguelike.Core;
using roguelike.Entities.Monsters;
using System.Linq;
using roguelike.Core.Systems;
using RogueSharp;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using roguelike.MapObjects;

namespace roguelike.Consoles
{
    public class MapConsole : SadConsole.Consoles.Console
    {
        FieldOfView rogueFOV;
        MapObjectBase[,] mapData;
        public Player Player { get; private set; }
        public Dictionary<Point, Monster> Monsters { get; private set; }

        Map rogueMap;
        public DungeonMap detailedMap { get; private set; }

        IReadOnlyCollection<Cell> previousFOV = new List<Cell>();

        public MapConsole(int viewWidth, int viewHeight, int mapWidth, int mapHeight): base(mapWidth, mapHeight)
        {
            TextSurface.RenderArea = new Rectangle(0, 0, viewWidth, viewHeight);
            Player = new Player();           
            GenerateMap();
        }

        bool runOnce = true; // TODO: carry on thinking for a proper fix
        public override void Render()
        {
            if(runOnce) {
                MovePlayerBy(new Point(0, 0));
                GameWorld.DungeonScreen.StatsConsole.DrawPlayerStats(Player);
                runOnce = false;
            }

            base.Render();
            Player.Render();
            
            Monsters.ToList().ForEach(m =>
            {
                if (!m.Value.InFoV) return;
                if (textSurface.RenderArea.Contains(m.Value.Position)) { m.Value.Render(); }
            });
        }

        public override void Update()
        {
            base.Update();
            Player.Update();
            Monsters.ToList().ForEach(m =>
            {
                m.Value.RenderOffset = Position - textSurface.RenderArea.Location;
                m.Value.Update();
            });
        }

        public void MovePlayerBy(Point amount)
        {
            Point newPosition = Player.Position + amount;

            if (!new Rectangle(0, 0, Width, Height).Contains(newPosition) || !rogueMap.IsWalkable(newPosition.X, newPosition.Y)) return;

            if(Monsters.ContainsKey(newPosition))
            {
                Game.CombatSystem.Attack(Player, Monsters[newPosition]);
            }
            else
            {
                Player.Position += amount;
                detailedMap.OpenDoor(Player, Player.Position.X, Player.Position.Y, ref mapData);
                
                // TODO: fix this possitioning horror
                TextSurface.RenderArea = new Rectangle(Player.Position.X - (TextSurface.RenderArea.Width / 2),
                                                        Player.Position.Y - (TextSurface.RenderArea.Height / 2),
                                                        TextSurface.RenderArea.Width, TextSurface.RenderArea.Height);

                // If he view area moved, we'll keep our entity in sync with it.
                Player.RenderOffset = Position - TextSurface.RenderArea.Location;

                // Update FoV
                foreach (var cell in previousFOV)
                {
                    mapData[cell.X, cell.Y].RemoveCellFromView(this[cell.X, cell.Y]);
                }
                previousFOV = rogueFOV.ComputeFov(Player.Position.X, Player.Position.Y, 10, true);
                foreach (var cell in previousFOV)
                {
                    rogueMap.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                    mapData[cell.X, cell.Y].RenderToCell(this[cell.X, cell.Y], true, rogueMap.GetCell(cell.X, cell.Y).IsExplored);
                }
            }

            GameWorld.DungeonScreen.StatsConsole.Clear();
            GameWorld.DungeonScreen.StatsConsole.DrawPlayerStats(Player);
            int idx = 0;
            foreach (KeyValuePair<Point, Monster> monster in Monsters)
            {
                if (rogueFOV.IsInFov(monster.Value.Position.X, monster.Value.Position.Y))
                {
                    monster.Value.InFoV = true;
                    GameWorld.DungeonScreen.StatsConsole.DrawMonsterStats(monster.Value, idx);
                    idx++;
                } else {
                    monster.Value.InFoV = false;
                }                  
            }
        }

        public void MoveMonster(Monster monster, Cell cell)
        {
            //if (detailedMap.SetActorPosition(monster, cell.X, cell.Y)) return;
            Point newLocation = new Point(cell.X, cell.Y);
            if (Player.Position.X == cell.X && Player.Position.Y == cell.Y)
            {
                Game.CombatSystem.Attack(monster, GameWorld.DungeonScreen.MapConsole.Player);
            }
            else if(rogueMap.IsWalkable(cell.X, cell.Y) && !Monsters.ContainsKey(newLocation))
            {            
                Monsters.Remove(new Point(monster.Position.X, monster.Position.Y));
                monster.Position = newLocation;
                Monsters.Add(newLocation, monster);
            }
        }

        private void GenerateMap()
        {
            // Create the map
            //RogueSharp.MapCreation.IMapCreationStrategy<RogueSharp.Map> mapCreationStrategy
            //    = new RogueSharp.MapCreation.RandomRoomsMapCreationStrategy<RogueSharp.Map>(Width, Height, 100, 20, 7);
            //rogueMap = RogueSharp.Map.Create(mapCreationStrategy);
            MapGenerator mapGenerator = new MapGenerator(Width, Height, 50, 12, 6);

            // TODO: I don't think I should have two maps
            detailedMap = mapGenerator.CreateMap();
            rogueMap = detailedMap;

            rogueFOV = new RogueSharp.FieldOfView(rogueMap);
            mapData = new MapObjects.MapObjectBase[Width, Height];

            // Loop through the map information generated by RogueSharp and create our cached visuals of that data
            foreach (var cell in rogueMap.GetAllCells())
            {
                if (cell.IsWalkable)
                {
                    if (detailedMap.GetDoor(cell.X,cell.Y) == null)
                    {
                        mapData[cell.X, cell.Y] = new Floor();
                    }
                    else
                    {
                        mapData[cell.X, cell.Y] = new Door('+');
                    }                
                    mapData[cell.X, cell.Y].RenderToCell(this[cell.X, cell.Y], false, false);
                }
                else
                { 
                    //rogueMap.SetCellProperties(cell.X, cell.Y, false, false);
                    mapData[cell.X, cell.Y] = new MapObjects.Wall();
                    mapData[cell.X, cell.Y].RenderToCell(this[cell.X, cell.Y], false, false);

                    // We're a wall, so we block LOS
                    rogueMap.SetCellProperties(cell.X, cell.Y, false, cell.IsWalkable);
                }
            }

            
            Monsters = detailedMap.getMonsters(Position - textSurface.RenderArea.Location);
            PositionPlayer();
        }


        private void PositionPlayer()
        {
            Player.Position = detailedMap.getPlayerStartingPosition();
            
            TextSurface.RenderArea = new Rectangle(Player.Position.X - (TextSurface.RenderArea.Width / 2),
                                                    Player.Position.Y - (TextSurface.RenderArea.Height / 2),
                                                    TextSurface.RenderArea.Width, TextSurface.RenderArea.Height);

            Player.RenderOffset = Position - TextSurface.RenderArea.Location;
            Game.SchedulingSystem.Add(Player);
        }

        public void RemoveMonster(Monster killed)
        {
            foreach(KeyValuePair<Point, Monster> monster in Monsters)
            {
                if(monster.Value.Equals(killed)) {
                    Monsters.Remove(monster.Key);
                    Game.SchedulingSystem.Remove(monster.Value);
                    return;
                }
            }
        }
    }
}
