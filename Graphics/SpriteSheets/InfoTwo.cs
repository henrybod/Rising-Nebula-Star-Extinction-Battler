using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Hook.Graphics;

namespace teamstairwell.Graphics.SpriteSheets
{
    public class InfoTwo : SpriteSheet
    {
        public const int INFOTWO = 0;

        public InfoTwo(Texture2D SheetImage)
            : base()
        {
            this.SheetImage = SheetImage;

            FrameValues = new int[,]{
                {1200, 750, 1} 
            };
        }
    }
}
