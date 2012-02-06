using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Hook.Graphics.SpriteSheets
{
    public class PrimitiveSheet : SpriteSheet
    {
        public const int LINEPIECE = 0;
        public const int POINTBOX = 1;
        public const int CURSOR = 2;
        public const int FLAG = 3;

        public PrimitiveSheet(Texture2D SheetImage)
            : base()
        {
            this.SheetImage = SheetImage;

            FrameValues = new int[,]{
                {1, 1, 1},   //Line pixel
                {3, 3, 1},   //Point box
                {5, 5, 1},   //Mouse cursor
                {9, 11, 1}   //Mouse cursor
            };
        }
    }
}
