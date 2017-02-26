using Microsoft.Xna.Framework;
using roguelike.Consoles;
using SadConsole.Consoles;
using System;
using Console = SadConsole.Consoles.Console;

namespace roguelike.Consoles
{
    class DungeonScreen : ConsoleList
    {
        public Console ViewConsole;
        public StatConsole StatsConsole;
        public MessagesConsole MessageConsole;
        public InventoryConsole InventoryConsole;

        private Console messageHeaderConsole; // ???

        // console window sizes
        private static readonly int _screenWidth = 150;
        private static readonly int _screenHeight = 71;
        private static readonly int _mapWidth = 130;
        private static readonly int _mapHeight = 48;
        private static readonly int _messageWidth = 130;
        private static readonly int _messageHeight = 11;
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static readonly int _inventoryWidth = 130;
        private static readonly int _inventoryHeight = 11;

        public DungeonScreen()
        {
            InventoryConsole = new InventoryConsole(_inventoryWidth, _inventoryHeight);
            StatsConsole = new StatConsole(_statWidth, _statHeight);
            ViewConsole = new Console(_mapWidth, _mapHeight);
            ViewConsole.FillWithRandomGarbage(); // Temporary so we can see where the console is on the screen
            MessageConsole = new MessagesConsole(_messageWidth, _messageHeight);

            // Setup the message header to be as wide as the screen but only 1 character high
            messageHeaderConsole = new Console(_screenWidth, 1);
            messageHeaderConsole.DoUpdate = false;
            messageHeaderConsole.CanUseKeyboard = false;
            messageHeaderConsole.CanUseMouse = false;

            // Draw the line for the header
            messageHeaderConsole.Fill(Color.White, Color.Black, 196, null);
            messageHeaderConsole.SetGlyph(56, 0, 193); // This makes the border match the character console's left-edge border

            // Print the header text
            messageHeaderConsole.Print(2, 0, " Messages ");

            // Move the rest of the consoles into position (ViewConsole is already in position at 0,0)
            ViewConsole.Position = new Point(0, 11);
            StatsConsole.Position = new Point(130, 0);
            MessageConsole.Position = new Point(0, 60);
            messageHeaderConsole.Position = new Point(0, 59);

            // Add all consoles to this console list.
            Add(messageHeaderConsole);
            Add(StatsConsole);
            Add(ViewConsole);
            Add(MessageConsole);

            // Placeholder stuff for the stats screen
            StatsConsole.CharacterName = "Hydorn";
            StatsConsole.MaxHealth = 200;
            StatsConsole.Health = 100;
        }
    }
}
