using roguelike.Core.Systems;
using RogueSharp.Random;
using System;

namespace roguelike
{
    public class Game
    {
        // Singleton of IRandom used throughout the game when generating random numbers
        public static IRandom Random { get; private set; }
		public static SchedulingSystem SchedulingSystem { get; private set; }

		public static void Main(string[] args)
        {
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);
             
            SadConsole.Engine.Initialize("Fonts/IBM.font", 150, 50);
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