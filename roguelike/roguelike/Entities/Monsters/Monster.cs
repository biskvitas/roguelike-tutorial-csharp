using Microsoft.Xna.Framework;
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

        // TODO: add code to draw monster only when it is in FoV       


        // some old code from original class
        //public int? TurnsAlerted { get; set; }

        //public virtual void PerformAction(CommandSystem commandSystem)
        //{
        //    var behavior = new StandardMoveAndAttack();
        //    behavior.Act(this, commandSystem);
        //}
    }
}
