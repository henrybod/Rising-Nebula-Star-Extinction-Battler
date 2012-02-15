using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Graphics.SpriteSheets {
    public class HenrySpriteSheets {

        public Dictionary<string, SpriteInfo> All = new Dictionary<string, SpriteInfo>();

        public HenrySpriteSheets() {
            //define all the sprites to be used in the game
            AddSprite(ref All, "SpriteSheets/Cursor", "Cursor", 11, 19, 1);
            AddSprite(ref All, "SpriteSheets/Button", "Button", 229, 130, 3);
            AddSprite(ref All, "SpriteSheets/TitleBackground", "MenuBackground", 1750, 1454, 1);
            //here shall be a whole lotta code

















        }

        private void AddSprite(ref Dictionary<string, SpriteInfo> sprites, string spriteSheet, string spriteName, int width, int height, int spriteCount) {
            SpriteInfo temp = new SpriteInfo();
            temp.Filename = spriteSheet;
            
            //calc Y //look at all previous sprites in this sheet and add their heights together
            temp.Y = 0;
            foreach(SpriteInfo s in sprites.Values){
                if(s.Filename == temp.Filename) temp.Y += s.Height;
            }
            temp.Height = height;
            temp.Width = width;
            temp.FrameCount = spriteCount;

            sprites.Add(spriteName, temp);
        }
    }

    public struct SpriteInfo {
        public int Y;
        public int Width;
        public int Height;
        public int FrameCount;
        public string Filename;
    }
}
