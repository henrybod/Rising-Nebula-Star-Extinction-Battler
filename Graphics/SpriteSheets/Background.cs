using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Hook.Graphics;

namespace teamstairwell.Graphics.SpriteSheets
{
    public class Background : SpriteSheet
    {
        public const int BACKGROUND = 0;

        public Background(Texture2D SheetImage)
            : base()
        {
            this.SheetImage = SheetImage;

            FrameValues = new int[,]{
                {1500, 1500, 1} 
            };
        }
    }
}
