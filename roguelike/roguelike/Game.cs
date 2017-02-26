using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using roguelike.Core;
using roguelike.Systems;
using RogueSharp.Random;
using System;

namespace roguelike
{
    public class Game
    {
	    private const string FontFileName = "terminal8x8.png";
        private static bool _renderRequired = true;

        public static MessageLog MessageLog { get; private set; }
        public static CommandSystem CommandSystem { get; private set; }
        public static DungeonMap DungeonMap { get; private set; }
        public static Player Player { get; set; }
        // Singleton of IRandom used throughout the game when generating random numbers
        public static IRandom Random { get; private set; }
		public static SchedulingSystem SchedulingSystem { get; private set; }

		public static void Main(string[] args)
        {
            /*
			SchedulingSystem = new SchedulingSystem();
			CommandSystem = new CommandSystem();
			MessageLog = new MessageLog();

			// Establish the seed for the random number generator from the current time
			int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);
                  
            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 20, 15, 8);
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.UpdatePlayerFieldOfView();

            MessageLog.Add("The rogue arrives on level 1");
            MessageLog.Add($"Level created with seed '{seed}'");

            /*
            Content.RootDirectory = "Content";
            var sadConsoleComponent = new SadConsole.EngineGameComponent(this, () => {
                using (var stream = System.IO.File.OpenRead("Fonts/Cheepicus12.font"))
                    SadConsole.Engine.DefaultFont = SadConsole.Serializer.Deserialize<SadConsole.Font>(stream);

                SadConsole.Engine.DefaultFont.ResizeGraphicsDeviceManager(_graphics, _screenWidth, _screenHeight, 0, 0);
                SadConsole.Engine.UseMouse = true;
                SadConsole.Engine.UseKeyboard = true;

                _mapConsole = new Console(_mapWidth, _mapHeight);
                _messageConsole = new Console(_messageWidth, _messageHeight);
                _statConsole = new Console(_statWidth, _statHeight);
                _inventoryConsole = new Console(_inventoryWidth, _inventoryHeight);

                _mapConsole.Position = new Point(0, _inventoryHeight);
                _messageConsole.Position = new Point(0, _screenHeight - _messageHeight);
                _statConsole.Position = new Point(_mapWidth, 0);
                _inventoryConsole.Position = new Point(0, 0);

                SadConsole.Engine.ConsoleRenderStack.Add(_mapConsole);
                SadConsole.Engine.ConsoleRenderStack.Add(_messageConsole);
                SadConsole.Engine.ConsoleRenderStack.Add(_statConsole);
                SadConsole.Engine.ConsoleRenderStack.Add(_inventoryConsole);

                SadConsole.Engine.ActiveConsole = _mapConsole;
            });
            Components.Add(sadConsoleComponent);
            */

            SadConsole.Engine.Initialize("IBM.font", 150, 71);

            SadConsole.Engine.EngineStart += EngineStart;
            SadConsole.Engine.EngineUpdated += EngineUpdated;

            SadConsole.Engine.Run();
        }

        /*
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
        */

        private static void EngineStart(object sender, EventArgs e)
        {
            SadConsole.Engine.ConsoleRenderStack.Clear();
            SadConsole.Engine.ActiveConsole = null;

            GameWorld.Start();
        }

        private static void EngineUpdated(object sender, EventArgs e)
        {

        }
    }
}