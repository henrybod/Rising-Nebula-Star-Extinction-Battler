using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Hook.Graphics
{
    public class SpriteSheet
    {
        public Texture2D SheetImage;
        public int[,] FrameValues;  //Width, height, # of frames

        public SpriteSheet()
        {
        }

        public int GetFrameWidth(int SpriteType)
        {
            return FrameValues[SpriteType, 0];
        }
        public int GetFrameHeight(int SpriteType)
        {
            return FrameValues[SpriteType, 1];
        }
        public int GetFrameCount(int SpriteType)
        {
            return FrameValues[SpriteType, 2];
        }

        public Texture2D GetSheet()
        {
            return SheetImage;
        }
    }
}
