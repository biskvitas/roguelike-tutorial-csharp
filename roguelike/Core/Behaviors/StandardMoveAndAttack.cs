using Microsoft.Xna.Framework;
using roguelike.Core.Systems;
using roguelike.Entities;
using roguelike.Entities.Monsters;
using roguelike.Interfaces;
using RogueSharp;

namespace roguelike.Core.Behaviors
{
	public class StandardMoveAndAttack : IBehavior
	{
        private const int AlertStatusLength = 15;

		public bool Act(Monster monster, CombatSystem combatSystem)
		{
            Microsoft.Xna.Framework.Point pLoc = GameWorld.DungeonScreen.MapConsole.Player.Position;
            FieldOfView monsterFov = new FieldOfView(GameWorld.DungeonScreen.MapConsole.detailedMap);

			// If the monster has not been alerted, compute a field-of-view 
			// Use the monster's Awareness value for the distance in the FoV check
			// If the player is in the monster's FoV then alert it
			// Add a message to the MessageLog regarding this alerted status
			if (!monster.TurnsAlerted.HasValue)
			{
				monsterFov.ComputeFov(monster.Position.X, monster.Position.Y, monster.Awareness, true);
				if (monsterFov.IsInFov(pLoc.X, pLoc.Y))
				{
                    GameWorld.DungeonScreen.MessageConsole.PrintMessage($"{monster.Name} is eager to fight {GameWorld.DungeonScreen.MapConsole.Player.Name}");
					monster.TurnsAlerted = 1;
				}
			}

			if (monster.TurnsAlerted.HasValue)
			{
                PathFinder pathFinder = new PathFinder(GameWorld.DungeonScreen.MapConsole.detailedMap);
				Path path = null;

				try
				{
					path = pathFinder.ShortestPath(
                    GameWorld.DungeonScreen.MapConsole.detailedMap.GetCell(monster.Position.X, monster.Position.Y),
                    GameWorld.DungeonScreen.MapConsole.detailedMap.GetCell(pLoc.X, pLoc.Y));
				}
				catch (PathNotFoundException)
				{
                    // The monster can see the player, but cannot find a path to him
                    // This could be due to other monsters blocking the way
                    // Add a message to the message log that the monster is waiting
                    GameWorld.DungeonScreen.MessageConsole.PrintMessage($"{monster.Name} waits for a turn");
				}

				// In the case that there was a path, tell the CommandSystem to move the monster
				if (path != null)
				{
					try
					{
						GameWorld.DungeonScreen.MapConsole.MoveMonster(monster, path.StepForward());
					}
					catch (NoMoreStepsException)
					{
                        GameWorld.DungeonScreen.MessageConsole.PrintMessage($"{monster.Name} growls in frustration");
					}
				}

				monster.TurnsAlerted++;

				if (monster.TurnsAlerted > AlertStatusLength)
				{
					monster.TurnsAlerted = null;
				}
			}
			return true;
		}      
    }
}
