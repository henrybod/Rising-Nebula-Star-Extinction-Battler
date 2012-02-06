using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Hook.Graphics;

namespace teamstairwell.Graphics.SpriteSheets
{
    public class TitleSheet : SpriteSheet
    {
        public const int TITLESHEET = 0;

        public TitleSheet(Texture2D SheetImage)
            : base()
        {
            this.SheetImage = SheetImage;

            FrameValues = new int[,]{
                {1000, 1000, 1} 
            };
        }
    }
}
   
