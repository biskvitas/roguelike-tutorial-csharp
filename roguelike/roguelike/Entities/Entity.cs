using roguelike.Interfaces;
using SadConsole;
using SadConsole.Game;

namespace roguelike.Entities
{
    public class Entity : GameObject, IActor
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

        public Entity(Font font):base(font) { }
    }
}
