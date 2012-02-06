using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Hook.Graphics.SpriteSheets
{
    public class ParticlePlayerSheet : SpriteSheet
    {
        public const int FIRST = 0;
        public const int SECOND = 1;
        public const int THIRD = 2;
        public const int FOURTH = 3;

        public const int PIXELCHOP = 2;

        public ParticlePlayerSheet(Texture2D SheetImage)
            : base()
        {
            this.SheetImage = SheetImage;

            //int FrameWidth = Game1.PlayerSheet.GetFrameWidth(PlayerSheet.TEMPREST) / PIXELCHOP;
            //int FrameHeight = Game1.PlayerSheet.GetFrameHeight(PlayerSheet.TEMPREST) / PIXELCHOP;

            FrameValues = new int[,]{
                {20, 20, 1},   //1
                {20, 20, 1},    //2
                {20, 20, 1},     //3
                {15, 15, 1}    //4
            };
        }
    }
}
