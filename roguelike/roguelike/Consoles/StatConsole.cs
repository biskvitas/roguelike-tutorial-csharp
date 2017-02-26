using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelike.Consoles
{
    class StatConsole : SadConsole.Consoles.Console
    {
        private string characterName;
        private int health;
        private int maxHealth;

        public string CharacterName
        {
            set { characterName = value; RedrawPanel(); }
        }

        public int Health
        {
            set { health = value; RedrawPanel(); }
        }

        public int MaxHealth
        {
            set { maxHealth = value; RedrawPanel(); }
        }

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

        private void RedrawPanel()
        {
            Print(2, 2, characterName);

            // Create a colored string that looks like 52/500
            SadConsole.ColoredString healthStatus = health.ToString().CreateColored(Color.LightGreen, Color.Black, null) +
                                                    "/".CreateColored(Color.White, Color.Black, null) +
                                                    maxHealth.ToString().CreateColored(Color.DarkGreen, Color.Black, null);

            // Align the string to the right side of the console
            Print(Width - 2 - healthStatus.ToString().Length, 2, healthStatus);
        }
    }
}
