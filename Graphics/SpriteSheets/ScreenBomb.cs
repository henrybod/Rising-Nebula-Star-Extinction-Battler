using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Hook.Graphics;

namespace teamstairwell.Graphics.SpriteSheets
{
    public class ScreenBomb : SpriteSheet
    {
        public const int SCREENBOMB = 0;

        public ScreenBomb(Texture2D SheetImage)
            : base()
        {
            this.SheetImage = SheetImage;

            FrameValues = new int[,]{
                {140, 140, 7} 
            };
        }
    }
}
