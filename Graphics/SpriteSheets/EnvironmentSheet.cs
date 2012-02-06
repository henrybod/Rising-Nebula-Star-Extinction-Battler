using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Hook.Graphics.SpriteSheets
{
    public class EnvironmentSheet : SpriteSheet
    {
        public const int LRECTANGLE = 0;
        public const int LTRIANGLE = 1;
        public const int SISOTRI = 2;
        public const int SRIGHTTRI = 3;
        public const int SSQUARE = 4;
        public const int TSQUARE = 5;

        public EnvironmentSheet(Texture2D SheetImage)
            : base()
        {
            this.SheetImage = SheetImage;

            //enraged games against diction stories
            FrameValues = new int[,]{
                {1200, 200, 1},   //Rectangle
                {600, 200, 1},   //Triangle
                {100, 100, 1},   //Triangle
                {100, 100, 1},   //Triangle
                {100, 100, 1},   //Square
                {50, 50, 1}   //Square
            };
        }
    }
}
