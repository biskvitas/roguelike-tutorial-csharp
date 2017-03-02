using Microsoft.Xna.Framework;
using SadConsole.Consoles;
using SadConsole.Input;
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

        private readonly int dungeonWidth = 130;
        private readonly int dungeonHeight = 44;

        // console window sizes
        private static readonly int _screenWidth = 150;
        private static readonly int _screenHeight = 50;

        private static readonly int _mapWidth = 130;
        private static readonly int _mapHeight = 44;

        private static readonly int _messageWidth = 130;
        private static readonly int _messageHeight = 5;

        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 35;

        private static readonly int _inventoryWidth = 20;
        private static readonly int _inventoryHeight = 15;

        public DungeonScreen()
        {
            InventoryConsole = new InventoryConsole(_inventoryWidth, _inventoryHeight);
            InventoryConsole.FillWithRandomGarbage();
            StatsConsole = new StatConsole(_statWidth, _statHeight);
            MapConsole = new MapConsole(_mapWidth, _mapHeight, dungeonWidth, dungeonHeight);
            //MapConsole.FillWithRandomGarbage(); // Temporary so we can see where the console is on the screen
            MessageConsole = new MessagesConsole(_messageWidth, _messageHeight);

            SadConsole.Engine.Keyboard.RepeatDelay = 0.07f;
            SadConsole.Engine.Keyboard.InitialRepeatDelay = 0.07f;

            // Setup the message header to be as wide as the screen but only 1 character high
            messageHeaderConsole = new Console(_messageWidth, 1);
            messageHeaderConsole.DoUpdate = false;
            messageHeaderConsole.CanUseKeyboard = false;
            messageHeaderConsole.CanUseMouse = false;

            // Draw the line for the header
            messageHeaderConsole.Fill(Color.White, Color.Black, 196, null);
            messageHeaderConsole.SetGlyph(56, 0, 193); // This makes the border match the character console's left-edge border

            // Print the header text
            messageHeaderConsole.Print(2, 0, " Messages ");

            MapConsole.Position = new Point(0, 0);
            StatsConsole.Position = new Point(_messageWidth, 0);
            InventoryConsole.Position = new Point(_messageWidth, _statHeight);
            MessageConsole.Position = new Point(0, _mapHeight + 1);
            messageHeaderConsole.Position = new Point(0, _mapHeight);

            // Add all consoles to this console list.
            Add(messageHeaderConsole);
            Add(StatsConsole);
            Add(MapConsole);
            Add(MessageConsole);
            Add(InventoryConsole);

            SadConsole.Engine.ActiveConsole = this;
        }

        public override bool ProcessKeyboard(KeyboardInfo info)
        {
            bool didPlayerAct = false;
            if (!Game.CommandSystem.IsPlayerTurn)
            {
                Game.CommandSystem.ActivateMonsters();
            }
            
            if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Down)))
            {
                MapConsole.MovePlayerBy(new Point(0, 1));
                didPlayerAct = true;
            }
            else if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Up)))
            {
                MapConsole.MovePlayerBy(new Point(0, -1));
                didPlayerAct = true;
            }

            if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Right)))
            {
                MapConsole.MovePlayerBy(new Point(1, 0));
                didPlayerAct = true;
            }
            else if (info.KeysPressed.Contains(AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Left)))
            {
                MapConsole.MovePlayerBy(new Point(-1, 0));
                didPlayerAct = true;
            }

            if (didPlayerAct)
            {
                Game.CommandSystem.EndPlayerTurn();
            }

            return false;
        }
    }
}
