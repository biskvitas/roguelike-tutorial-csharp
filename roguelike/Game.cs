using roguelike.Core.Systems;
using RogueSharp.Random;
using System;

namespace roguelike
{
    public class Game
    {
        // Singleton of IRandom used throughout the game when generating random numbers
        public static IRandom Random { get; private set; }
        public static readonly SchedulingSystem SchedulingSystem = new SchedulingSystem();
        public static readonly CommandSystem CommandSystem = new CommandSystem();
        public static readonly CombatSystem CombatSystem = new CombatSystem();

        public static void Main(string[] args)
        {
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);
             
            SadConsole.Engine.Initialize("IBM.font", 150, 50);
            SadConsole.Engine.EngineStart += EngineStart;
            SadConsole.Engine.EngineUpdated += EngineUpdated;

            SadConsole.Engine.Run();
        }

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