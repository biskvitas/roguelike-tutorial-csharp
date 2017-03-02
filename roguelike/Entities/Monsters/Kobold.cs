using roguelike.Utils;
using RogueSharp.DiceNotation;
using SadConsole;
using SadConsole.Consoles;

namespace roguelike.Entities.Monsters
{
    public class Kobold : Monster
    {
        public Kobold(int level) : base(Engine.DefaultFont)
        {
            int health = Dice.Roll("2D5");
            Attack = Dice.Roll("1D3") + level / 3;
            AttackChance = Dice.Roll("25D3");
            Awareness = 10;
            Defense = Dice.Roll("1D3") + level / 3;
            DefenseChance = Dice.Roll("10D4");
            Gold = Dice.Roll("5D5");
            Health = health;
            MaxHealth = health;
            Name = "Kobold";
            Speed = 14;
            Symbol = "k";
            color = Colors.KoboldColor;

            // set animation
            AnimatedTextSurface monsterAnimation = new AnimatedTextSurface("default", 1, 1, Engine.DefaultFont);
            monsterAnimation.CreateFrame();
            monsterAnimation.CurrentFrame[0].Foreground = Colors.KoboldColor;
            monsterAnimation.CurrentFrame[0].GlyphIndex = 'k';
            Animation = monsterAnimation;
        }
    }
}
