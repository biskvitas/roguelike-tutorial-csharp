using System.Text;
using RogueSharp.DiceNotation;
using roguelike.Entities;
using roguelike.Entities.Monsters;

namespace roguelike.Core.Systems
{
    public class CombatSystem
    {
        
		public void Attack(Entity attacker, Entity defender)
		{
			StringBuilder attackMessage = new StringBuilder();
			StringBuilder defenseMessage = new StringBuilder();

			int hits = ResolveAttack(attacker, defender, attackMessage);
			int blocks = ResolveDefense(defender, hits, attackMessage, defenseMessage);

            GameWorld.DungeonScreen.MessageConsole.PrintMessage(attackMessage.ToString());
			if (!string.IsNullOrWhiteSpace(defenseMessage.ToString()))
			{
                GameWorld.DungeonScreen.MessageConsole.PrintMessage(defenseMessage.ToString());
			}

			int damage = hits - blocks;
			ResolveDamage(defender, damage);
		}

		// The attacker rolls based on his stats to see if he gets any hits
		private static int ResolveAttack(Entity attacker, Entity defender, StringBuilder attackMessage)
		{
			int hits = 0;

			attackMessage.AppendFormat("{0} attacks {1} and rolls: ", attacker.Name, defender.Name);

			// Roll a number of 100-sided dice equal to the Attack value of the attacking actor
			DiceExpression attackDice = new DiceExpression().Dice(attacker.Attack, 100);
			DiceResult attackResult = attackDice.Roll();

			// Look at the face value of each single die that was rolled
			foreach (TermResult termResult in attackResult.Results)
			{
				attackMessage.Append(termResult.Value + ", ");
				// Compare the value to 100 minus the attack chance and add a hit if it's greater
				if (termResult.Value >= 100 - attacker.AttackChance)
				{
					hits++;
				}
			}

			return hits;
		}

		// The defender rolls based on his stats to see if he blocks any of the hits from the attacker
		private static int ResolveDefense(Entity defender, int hits, StringBuilder attackMessage, StringBuilder defenseMessage)
		{
			int blocks = 0;

			if (hits > 0)
			{
				attackMessage.AppendFormat("scoring {0} hits.", hits);
				defenseMessage.AppendFormat("  {0} defends and rolls: ", defender.Name);

				// Roll a number of 100-sided dice equal to the Defense value of the defendering actor
				DiceExpression defenseDice = new DiceExpression().Dice(defender.Defense, 100);
				DiceResult defenseRoll = defenseDice.Roll();

				// Look at the face value of each single die that was rolled
				foreach (TermResult termResult in defenseRoll.Results)
				{
					defenseMessage.Append(termResult.Value + ", ");
					// Compare the value to 100 minus the defense chance and add a block if it's greater
					if (termResult.Value >= 100 - defender.DefenseChance)
					{
						blocks++;
					}
				}
				defenseMessage.AppendFormat("resulting in {0} blocks.", blocks);
			}
			else
			{
				attackMessage.Append("and misses completely.");
			}

			return blocks;
		}

		// Apply any damage that wasn't blocked to the defender
		private static void ResolveDamage(Entity defender, int damage)
		{
			if (damage > 0)
			{
				defender.Health = defender.Health - damage;
                GameWorld.DungeonScreen.MessageConsole.PrintMessage($"  {defender.Name} was hit for {damage} damage");

				if (defender.Health <= 0)
				{
					ResolveDeath(defender);
				}
			}
			else
			{
                GameWorld.DungeonScreen.MessageConsole.PrintMessage($"  {defender.Name} blocked all damage");
			}
		}

		// Remove the defender from the map and add some messages upon death.
		private static void ResolveDeath(Entity defender)
		{
			if (defender is Player)
			{
                GameWorld.DungeonScreen.MessageConsole.PrintMessage($"  {defender.Name} was killed, GAME OVER MAN!");
			}
			else if (defender is Monster)
			{
				GameWorld.DungeonScreen.MapConsole.RemoveMonster((Monster)defender);
                GameWorld.DungeonScreen.MessageConsole.PrintMessage($"  {defender.Name} died and dropped {defender.Gold} gold");
			}
		}
        /*
		public bool IsPlayerTurn { get; set; }

		public void EndPlayerTurn()
		{
			IsPlayerTurn = false;
		}

		public void ActivateMonsters()
		{
			IScheduleable scheduleable = Game.SchedulingSystem.Get();
			if (scheduleable is Player)
			{
				IsPlayerTurn = true;
				Game.SchedulingSystem.Add(Game.Player);
			}
			else
			{
				Monster monster = scheduleable as Monster;

				if (monster != null)
				{
					monster.PerformAction(this);
					Game.SchedulingSystem.Add(monster);
				}

				ActivateMonsters();
			}
		}

		public void MoveMonster(Monster monster, Cell cell)
		{
			//if (Game.DungeonMap.SetActorPosition(monster, cell.X, cell.Y)) return;
			if (Game.Player.X == cell.X && Game.Player.Y == cell.Y)
			{
				Attack(monster, Game.Player);
			}
		}
        */
	}
}
