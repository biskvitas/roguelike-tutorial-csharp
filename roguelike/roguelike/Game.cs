using RLNET;
using roguelike.Core;
using roguelike.Systems;
using RogueSharp.Random;
using System;
using System.Text;

namespace roguelike
{
    public class Game
    {
	    private const string FontFileName = "terminal8x8.png";

		// The screen height and width are in number of tiles
		private static readonly int _screenWidth = 150;
        private static readonly int _screenHeight = 70;
        private static RLRootConsole _rootConsole;

        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int _mapWidth = 130;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;

        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int _messageWidth = 130;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;

        // The stat console is to the right of the map and display player and monster stats
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static RLConsole _statConsole;

        // Above the map is the inventory console which shows the players equipment, abilities, and items
        private static readonly int _inventoryWidth = 130;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;

        private static bool _renderRequired = true;

        public static MessageLog MessageLog { get; private set; }
        public static CommandSystem CommandSystem { get; private set; }
        public static DungeonMap DungeonMap { get; private set; }
        public static Player Player { get; set; }
        // Singleton of IRandom used throughout the game when generating random numbers
        public static IRandom Random { get; private set; }
		public static SchedulingSystem SchedulingSystem { get; private set; }

		public static void Main()
        {
			SchedulingSystem = new SchedulingSystem();
			CommandSystem = new CommandSystem();
			MessageLog = new MessageLog();

			// Establish the seed for the random number generator from the current time
			int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);
                  
            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 20, 15, 8);
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.UpdatePlayerFieldOfView();

			// Tell RLNet to use the bitmap font that we specified and that each tile is 8 x 8 pixels
			_rootConsole = new RLRootConsole(FontFileName, _screenWidth, _screenHeight, 8, 8, 1f, $"RougeSharp V3 Tutorial - Level 1 - Seed {seed}");
            // Initialize the sub consoles that we will Blit to the root console
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);


            MessageLog.Add("The rogue arrives on level 1");
            MessageLog.Add($"Level created with seed '{seed}'");

            //colour consoles and add tags
            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood);
            _inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);

            // Attach Events
            _rootConsole.Update += OnRootConsoleUpdate;
            _rootConsole.Render += OnRootConsoleRender;

            // Begin RLNET's game loop
            _rootConsole.Run();
        }

        // Event handler for RLNET's Update event
        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
		{
			bool didPlayerAct = false;
			RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();

			if (!CommandSystem.IsPlayerTurn)
			{
				CommandSystem.ActivateMonsters();
				_renderRequired = true;
			}

			if (keyPress == null) return;
		
			switch (keyPress.Key)
			{
				case RLKey.Up:
					didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
					break;
				case RLKey.Down:
					didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
					break;
				case RLKey.Left:
					didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
					break;
				case RLKey.Right:
					didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
					break;
				case RLKey.Escape:
					_rootConsole.Close();
					break;
			}
			
			if (didPlayerAct)
			{
				_renderRequired = true;
				CommandSystem.EndPlayerTurn();
			}
		}
    

        // Event handler for RLNET's Render event
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            if (_renderRequired)
            {
                _mapConsole.Clear();
                _statConsole.Clear();
                _messageConsole.Clear();

                MessageLog.Draw(_messageConsole);
                DungeonMap.Draw(_mapConsole, _statConsole);
                Player.Draw(_mapConsole, DungeonMap);
                Player.DrawStats(_statConsole);

                // Blit the sub consoles to the root console in the correct locations
                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

                // Tell RLNET to draw the console that we set
                _rootConsole.Draw();

                _renderRequired = false;
            }
        }
    }
}