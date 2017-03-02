using Microsoft.Xna.Framework;
using roguelike.Core.Behaviors;
using SadConsole;

namespace roguelike.Entities.Monsters
{
    public class Monster : Entity
    {
        // monster specific properties
        public string Symbol { get; set; }
        public Color color { get; set; }
        public bool InFoV { get; set; }

        public Monster(Font font) : base(font) {}

        public int? TurnsAlerted { get; set; }

        public virtual void PerformAction()
        {
            var behavior = new StandardMoveAndAttack();
            behavior.Act(this, Game.CombatSystem);
        }
    }
}
