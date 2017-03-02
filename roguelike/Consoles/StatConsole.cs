using Microsoft.Xna.Framework;
using roguelike.Entities;
using roguelike.Entities.Monsters;
using roguelike.Utils;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelike.Consoles
{
    class StatConsole : SadConsole.Consoles.Console
    {
        public StatConsole(int width, int height): base(width, height)
        {
            // Draw the side bar
            SadConsole.Shapes.Line line = new SadConsole.Shapes.Line();
            line.EndingLocation = new Point(0, height - 1);
            line.CellAppearance.GlyphIndex = 179;
            line.UseEndingCell = false;
            line.UseStartingCell = false;
            line.Draw(this);
        }

        public void DrawPlayerStats(Player player)
        {
            Print(1, 1, $"Name:    {player.Name}", Colors.Text);
            Print(1, 3, $"Health:  {player.Health}/{player.MaxHealth}", Colors.Text);
            Print(1, 5, $"Attack:  {player.Attack} ({player.AttackChance}%)", Colors.Text);
            Print(1, 7, $"Defense: {player.Defense} ({player.DefenseChance}%)", Colors.Text);
            Print(1, 9, $"Gold:    {player.Gold}", Colors.Gold);
        }

        public void DrawMonsterStats(Monster monster, int monsterIdx)
        {
            // Start at Y=13 which is below the player stats.
            // Multiply the position by 2 to leave a space between each stat
            int yPosition = 13 + (monsterIdx * 2);

            // Begin the line by printing the symbol of the monster in the appropriate color
            Print(1, yPosition, monster.Symbol.ToString(), monster.color);

            // Figure out the width of the health bar by dividing current health by max health
            int width = Convert.ToInt32(((double)monster.Health / monster.MaxHealth) * 16.0);
            int remainingWidth = 16 - width;

            // Set the background colors of the health bar to show how damaged the monster is
            ColoredString curHp = new ColoredString(width);
            curHp.SetBackground(Swatch.Primary);
            Print(3, yPosition, curHp);

            ColoredString missHp = new ColoredString(remainingWidth);
            missHp.SetBackground(Swatch.PrimaryDarkest);
            Print(3 + width, yPosition, missHp);

            // Print the monsters name over top of the health bar
            Print(2, yPosition, $": {monster.Name}", Swatch.DbLight);
        }
    }
}
