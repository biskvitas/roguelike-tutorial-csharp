using Microsoft.Xna.Framework;
using roguelike.Interfaces;
using roguelike.Utils;
using SadConsole;
using SadConsole.Consoles;
using SadConsole.Game;

namespace roguelike.Entities
{
    public class Player : Entity
    {
        public Player() : base(Engine.DefaultFont)
        {
            AnimatedTextSurface playerAnimation = new AnimatedTextSurface("default", 1, 1, Engine.DefaultFont);
            playerAnimation.CreateFrame();
            playerAnimation.CurrentFrame[0].Foreground = Colors.Player;
            playerAnimation.CurrentFrame[0].GlyphIndex = '@';

            Animation = playerAnimation;
            Position = new Point(0, 0);

            // set stats
            Attack = 2;
            AttackChance = 50;
            Awareness = 15;
            Defense = 2;
            DefenseChance = 40;
            Gold = 0;
            Health = 100;
            MaxHealth = 100;
            Name = "Rogue";
            Speed = 10;

            // TODO: call statConsole to draw stats
        }
    }
}
