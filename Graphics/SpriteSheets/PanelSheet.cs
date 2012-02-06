using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Hook.Graphics.SpriteSheets
{
    public class PanelSheet : SpriteSheet
    {
        public const int PLAYBUTTONFRAME = 0;
        public const int STORYBUTTONFRAME = 1;
        public const int INSTRBUTTONFRAME = 2;
        public const int EXITBUTTONFRAME = 3;
        public const int REDBAR = 4;
        public const int BACKBUTTONFRAME = 5;
        public const int PAGEONE = 6;
        public const int PAGETWO = 7;
        public const int HOPELESSMODE = 8;
        public const int NORMALMODE = 9;

        public PanelSheet(Texture2D SheetImage)
            : base()
        {
            this.SheetImage = SheetImage;

            FrameValues = new int[,]{
                {230, 130, 1},   //PlayFrame
                {230, 130, 1},   //StoryFrame
                {230, 130, 1},   //InstruFrame
                {230, 130, 1},    //ExitFrame
                {230, 100, 1},   //RedBar
                {230, 130, 1},  //Backfram
                {230, 130, 1},  //page 1
                {230, 130, 1},  //page 2
                {230, 130, 1}, //hopeless
                {230, 130, 1}, //normalmode 
            };
        }
    }
}
