using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Hook.Graphics;

namespace teamstairwell.Graphics.SpriteSheets
{
    public class TitleBackground : SpriteSheet
    {
        public const int TITLEBACKGROUND = 0;

        public TitleBackground(Texture2D SheetImage)
            : base()
        {
            this.SheetImage = SheetImage;

            FrameValues = new int[,]{
                {1700, 1454, 1} 
            };
        }
    }
}
