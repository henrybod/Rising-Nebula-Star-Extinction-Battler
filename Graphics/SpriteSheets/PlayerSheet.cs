using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Hook.Graphics.SpriteSheets
{
    public class PlayerSheet : SpriteSheet
    {
        public const int PNORMAL = 0;
        public const int PSHIELD = 1;
        public const int PBOOM = 2;
        public const int BOSSNORMAL = 3;
        public const int BOSSHIT = 4;
        public const int BOSSBOOM = 5;
        public const int PSHIFTRIGHT = 6;
        public const int PGOLEFT = 7;
        public const int PSHIFTLEFT = 8;
        public const int PGORIGHT = 9;
        public const int BENERGYSPHERE = 10;
        public const int FACE = 11;
        public const int OLDFACE = 12;

        public PlayerSheet(Texture2D SheetImage)
            : base()
        {
            this.SheetImage = SheetImage;

            FrameValues = new int[,]{
                {40, 40, 4}, //player
                {40, 40, 4}, //player
                {40, 40, 6}, //player
                {200, 200, 2}, //boss
                {200, 200, 3}, //boss
                {200, 246, 5}, //boss
                {40, 40, 3}, //player
                {40, 40, 2}, //player
                {40, 40, 3}, //player
                {40, 40, 2}, //player
                {500, 519, 1}, //energy-sphere
                {200, 200, 4}, //BOSSFACE
                {200, 200, 4} //BOSSFACE
            };

            /*FrameValues = new int[,]{
                {40, 60, 2}, //temp player
                {40, 60, 6}, //player run
                {40, 60, 1}, //player jump
                {40, 60, 1}, //player jump 2
                {40, 60, 1}, //player fall
                {40, 60, 1}, //player fall 2
                {40, 60, 4}, //player walk
                {40, 60, 4} // player movement dust
            };*/
        }
    }
}
