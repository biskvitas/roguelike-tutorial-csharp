﻿using Microsoft.Xna.Framework;
using roguelike.Consoles;
using SadConsole.Consoles;
using SadConsole.Input;
using System;
using Console = SadConsole.Consoles.Console;

namespace roguelike.Consoles
{
    class DungeonScreen : ConsoleList
    {
        public StatConsole StatsConsole;
        public MessagesConsole MessageConsole;
        public InventoryConsole InventoryConsole;
        public MapConsole MapConsole;

        private Console messageHeaderConsole; // ???

        // console window sizes
        private static readonly int _screenWidth = 150;
        private static readonly int _screenHeight = 50;

        private static readonly int _mapWidth = 130;
        private static readonly int _mapHeight = 39;

        private static readonly int _messageWidth = 130;
        private static readonly int _messageHeight = 5;

        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 44;

        private static readonly int _inventoryWidth = 130;
        private static readonly int _inventoryHeight = 5;

        public DungeonScreen()
        {
            InventoryConsole = new InventoryConsole(_inventoryWidth, _inventoryHeight);
            StatsConsole = new StatConsole(_statWidth, _statHeight);
            MapConsole = new MapConsole(_mapWidth, _mapHeight, 300, 300);
            //MapConsole.FillWithRandomGarbage(); // Temporary so we can see where the console is on the screen
            MessageConsole = new MessagesConsole(_messageWidth, _messageHeight);

            SadConsole.Engine.Keyboard.RepeatDelay = 0.07f;
            SadConsole.Engine.Keyboard.InitialRepeatDelay = 0.07f;

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

            MapConsole.Position = new Point(0, _inventoryHeight);
            StatsConsole.Position = new Point(_inventoryWidth, 0);
            MessageConsole.Position = new Point(0, _inventoryHeight + _mapHeight+1);
            messageHeaderConsole.Position = new Point(0, _inventoryHeight+_mapHeight);

            // Add all consoles to this console list.
            Add(messageHeaderConsole);
            Add(StatsConsole);
            Add(MapConsole);
            Add(MessageConsole);

            // Placeholder stuff for the stats screen
            StatsConsole.CharacterName = "Hydorn";
            StatsConsole.MaxHealth = 200;
            StatsConsole.Health = 100;

            SadConsole.Engine.ActiveConsole = this;
        }

        public override bool ProcessKeyboard(KeyboardInfo info)
        {
            if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Down)))
            {
                MapConsole.MovePlayerBy(new Point(0, 1));
            }
            else if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Up)))
            {
                MapConsole.MovePlayerBy(new Point(0, -1));
            }

            if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Right)))
            {
                MapConsole.MovePlayerBy(new Point(1, 0));
            }
            else if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Left)))
            {
                MapConsole.MovePlayerBy(new Point(-1, 0));
            }

            return false;
        }
    }
}