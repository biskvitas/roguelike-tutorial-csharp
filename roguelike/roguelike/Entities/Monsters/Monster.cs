using roguelike.Interfaces;
using SadConsole;
using SadConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelike.Entities.Monsters
{
    public class Monster : GameObject, IActor
    {
        public int Attack { get; set; }
        public int AttackChance { get; set; }
        public int Awareness { get; set; }
        public int Defense { get; set; }
        public int DefenseChance { get; set; }
        public int Gold { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public string Name { get; set; }
        public int Speed { get; set; }     

        public Monster(Font font) : base(font) {}
        
        // TODO: add code to draw monster only when it is in FoV       
    }
}
