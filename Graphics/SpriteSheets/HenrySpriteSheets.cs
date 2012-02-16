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
            //they must be defined in the order they are drawn on their respective sheet!!!

            //interface
            AddSprite(ref All, "SpriteSheets/Cursor", "Cursor", 11, 19, 1);
            AddSprite(ref All, "SpriteSheets/Button", "Button", 229, 130, 3);
            AddSprite(ref All, "SpriteSheets/TitleBackground", "MenuBackground", 1750, 1454, 1);

            //zihou
            AddSprite(ref All, "SpriteSheets/PlayerSheet", "PlayerIdle", 40, 40, 4);
            AddSprite(ref All, "SpriteSheets/PlayerSheet", "PlayerHit", 40, 40, 4);
            AddSprite(ref All, "SpriteSheets/PlayerSheet", "PlayerDeath", 40, 40, 6);

            //boss
            AddSprite(ref All, "SpriteSheets/PlayerSheet", "BossIdle", 200, 200, 2);
            AddSprite(ref All, "SpriteSheets/PlayerSheet", "BossHit", 200, 200, 3);
            AddSprite(ref All, "SpriteSheets/PlayerSheet", "BossDeath", 200, 200, 5);

            //zihou
            AddSprite(ref All, "SpriteSheets/PlayerSheet", "PlayerShiftLeft", 40, 40, 3);
            AddSprite(ref All, "SpriteSheets/PlayerSheet", "PlayerGoLeft", 40, 40, 2);
            AddSprite(ref All, "SpriteSheets/PlayerSheet", "PlayerShiftRight", 40, 40, 3);
            AddSprite(ref All, "SpriteSheets/PlayerSheet", "PlayerGoRight", 40, 40, 2);

            //boss
            AddSprite(ref All, "SpriteSheets/PlayerSheet", "BossField", 500, 519, 1);
            AddSprite(ref All, "SpriteSheets/PlayerSheet", "BossSillyFace", 200, 200, 4);

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
