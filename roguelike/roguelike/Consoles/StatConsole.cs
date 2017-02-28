using Microsoft.Xna.Framework;
using roguelike.Entities;
using roguelike.Utils;
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
    }
}
