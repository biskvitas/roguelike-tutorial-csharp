using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelike
{
    static class GameWorld
    {
        public static Consoles.DungeonScreen DungeonScreen;

        public static void Start()
        {
            DungeonScreen = new Consoles.DungeonScreen();
            SadConsole.Engine.ConsoleRenderStack.Add(DungeonScreen);
            DungeonScreen.MessageConsole.PrintMessage("Welcome to THE GAME...");
        }
    }
}
