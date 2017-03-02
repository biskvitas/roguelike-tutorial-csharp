using roguelike.Entities;
using roguelike.Entities.Monsters;
using roguelike.Interfaces;
using RogueSharp;

namespace roguelike.Core.Systems
{
    public class CommandSystem
    {
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
                Game.SchedulingSystem.Add(GameWorld.DungeonScreen.MapConsole.Player);
            }
            else
            {
                Monster monster = scheduleable as Monster;

                if (monster != null)
                {
                    monster.PerformAction();
                    Game.SchedulingSystem.Add(monster);
                }

                ActivateMonsters();
            }
        }
    }
}
