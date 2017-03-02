using roguelike.Utils;
using SadConsole;

namespace roguelike.MapObjects
{
    public class Door : MapObjectBase
	{      
		public Door(int character): base(Colors.Door, Colors.DoorBackground, character) {}

        public bool IsOpen { get; set; }
		public char Symbol { get; set; }

        public override void RenderToCell(Cell sadConsoleCell, bool isFov, bool isExplored)
        {
            base.RenderToCell(sadConsoleCell, isFov, isExplored);

            if (isFov)
            {
                sadConsoleCell.GlyphIndex = IsOpen ? 45 : 43;
            }
        }

        public override void RemoveCellFromView(Cell sadConsoleCell)
        {
            base.RemoveCellFromView(sadConsoleCell);
            sadConsoleCell.GlyphIndex = 0;
        }        
	}
}
