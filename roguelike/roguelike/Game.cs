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
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);
            /*
			SchedulingSystem = new SchedulingSystem();
			CommandSystem = new CommandSystem();
			MessageLog = new MessageLog();                

            /*
            Content.RootDirectory = "Content";
            var sadConsoleComponent = new SadConsole.EngineGameComponent(this, () => {
                using (var stream = System.IO.File.OpenRead("Fonts/Cheepicus12.font"))
                    SadConsole.Engine.DefaultFont = SadConsole.Serializer.Deserialize<SadConsole.Font>(stream);

                SadConsole.Engine.DefaultFont.ResizeGraphicsDeviceManager(_graphics, _screenWidth, _screenHeight, 0, 0);
                SadConsole.Engine.UseMouse = true;
                SadConsole.Engine.UseKeyboard = true;
            });
            Components.Add(sadConsoleComponent);
            */

            SadConsole.Engine.Initialize("IBM.font", 150, 50);

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