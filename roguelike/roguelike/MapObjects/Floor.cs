using Microsoft.Xna.Framework;
using SadConsole;

namespace roguelike.MapObjects
{
    public class Floor : MapObjectBase 
    {
        public Floor() : base(Color.DarkGray, Color.Transparent, 46) {}

        public override void RenderToCell(Cell sadConsoleCell, bool isFov, bool isExplored)
        {
            base.RenderToCell(sadConsoleCell, isFov, isExplored);

            if (isFov)
            {
                sadConsoleCell.GlyphIndex = 46;
            }
        }

        public override void RemoveCellFromView(Cell sadConsoleCell)
        {
            base.RemoveCellFromView(sadConsoleCell);

            sadConsoleCell.GlyphIndex = 0;
        }
    }
}
