using roguelike.Interfaces;
using RogueSharp;
using RLNET;

namespace roguelike.Core
{
    public class Actor : IActor, IDrawable, IScheduleable
    {
        // IActor

	    public int Time => Speed;

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

	    // IDrawables
        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }



        public void Draw(RLConsole console, IMap map)
        {
            // Don't draw actors in cells that haven't been explored
	        if (!map.GetCell(X, Y).IsExplored) return;

            // Only draw the actor with the color and symbol when they are in field-of-view
            if (map.IsInFov(X, Y))
            {
                console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            }
            else
            {
                // When not in field-of-view just draw a normal floor
                console.Set(X, Y, Colors.Floor, Colors.FloorBackground, '.');
            }
        }
    }
}