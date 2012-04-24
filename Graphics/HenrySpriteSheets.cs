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
            AddSprite(ref All, "SpriteSheets/Button", "ButtonNormal", 229, 130, 1);
            AddSprite(ref All, "SpriteSheets/Button", "ButtonHighlight", 229, 130, 1);
            AddSprite(ref All, "SpriteSheets/Button", "ButtonClick", 229, 130, 1);
            AddSprite(ref All, "SpriteSheets/TitleBackground", "MenuBackground", 1750, 1454, 1);
            AddSprite(ref All, "SpriteSheets/Background", "BattlefieldBackground", 1500, 1500, 1);

            //Player Sheet
                //Zihao
                AddSprite(ref All, "SpriteSheets/PlayerSheet", "PlayerIdle", 40, 40, 4);
                AddSprite(ref All, "SpriteSheets/PlayerSheet", "PlayerHit", 40, 40, 4);
                AddSprite(ref All, "SpriteSheets/PlayerSheet", "PlayerDeath", 40, 40, 6);

                //Notus
                AddSprite(ref All, "SpriteSheets/PlayerSheet", "BossIdle", 200, 200, 2);
                AddSprite(ref All, "SpriteSheets/PlayerSheet", "BossHit", 200, 200, 3);
                AddSprite(ref All, "SpriteSheets/PlayerSheet", "BossDeath", 200, 200, 6);
                AddSprite(ref All, "SpriteSheets/PlayerSheet", "Emptiness", 49, 49, 1);

                //Zihao
                AddSprite(ref All, "SpriteSheets/PlayerSheet", "PlayerShiftLeft", 40, 40, 3);
                AddSprite(ref All, "SpriteSheets/PlayerSheet", "PlayerGoLeft", 40, 40, 2);
                AddSprite(ref All, "SpriteSheets/PlayerSheet", "PlayerShiftRight", 40, 40, 3);
                AddSprite(ref All, "SpriteSheets/PlayerSheet", "PlayerGoRight", 40, 40, 2);

                //Notus
                AddSprite(ref All, "SpriteSheets/PlayerSheet", "BossStaticField", 500, 519, 1);
                AddSprite(ref All, "SpriteSheets/PlayerSheet", "BossSillyFace", 200, 200, 4);

            //Prop Sheet
                AddSprite(ref All, "SpriteSheets/PropSheet", "BulletPea", 15, 15, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "BulletRainbow", 15, 15, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "BulletMissle", 15, 15, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "Skull", 45, 45, 5);
                AddSprite(ref All, "SpriteSheets/PropSheet", "SkullAura", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/PropSheet", "Line", 45, 45, 5);
                AddSprite(ref All, "SpriteSheets/PropSheet", "LineAura", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/PropSheet", "Radial", 45, 45, 5);
                AddSprite(ref All, "SpriteSheets/PropSheet", "RadialAura", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/PropSheet", "HorizontalSpawner", 45, 45, 5);
                AddSprite(ref All, "SpriteSheets/PropSheet", "HorizontalSpawnerAura", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/PropSheet", "CrossSpawner", 45, 45, 5);
                AddSprite(ref All, "SpriteSheets/PropSheet", "CrossSpawnerAura", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/PropSheet", "PowerUpLife", 25, 25, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "PowerUpBomb", 25, 25, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "HealthTick", 3, 20, 1);
                AddSprite(ref All, "SpriteSheets/PropSheet", "TinyRecticle", 5, 5, 1);
                AddSprite(ref All, "SpriteSheets/PropSheet", "Cursor", 11, 20, 9);
                AddSprite(ref All, "SpriteSheets/PropSheet", "Explosion", 50, 50, 7);
                AddSprite(ref All, "SpriteSheets/PropSheet", "BulletFireball", 15, 15, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "BulletPlasma", 15, 15, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "BulletBlueFuzz", 15, 15, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "PeaSpawner", 45, 45, 5);
                AddSprite(ref All, "SpriteSheets/PropSheet", "PeaSpawnerAura", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/PropSheet", "BulletLaser", 15, 15, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "BulletRedFuzz", 30, 30, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "Marble", 60, 60, 16);
                AddSprite(ref All, "SpriteSheets/PropSheet", "Magnet", 45, 45, 5);
                AddSprite(ref All, "SpriteSheets/PropSheet", "MagnetAura", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/PropSheet", "Crosshair", 60, 60, 2);
                AddSprite(ref All, "SpriteSheets/PropSheet", "Buoy", 30, 30, 1);
                AddSprite(ref All, "SpriteSheets/PropSheet", "BulletGoldLaser", 15, 15, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "BulletSeeker", 15, 15, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "PowerUpG", 25, 25, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "PowerUpM", 25, 25, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "PowerUpK", 25, 25, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "PowerUpD", 25, 25, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "PowerUpP", 25, 25, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "HexSpawner", 45, 45, 5);
                AddSprite(ref All, "SpriteSheets/PropSheet", "HexSpawnerAura", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/PropSheet", "PowerUpR", 25, 25, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "Drone", 32, 29, 4);
                AddSprite(ref All, "SpriteSheets/PropSheet", "ArseLightning", 150, 45, 2);

            //HUD Sheet
                AddSprite(ref All, "SpriteSheets/HUD", "ShieldTick", 3, 20, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "WeaponRechargeTick", 1, 3, 8);
                AddSprite(ref All, "SpriteSheets/HUD", "WhiteDot", 1, 1, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "EmptyIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "BasicLaserIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "PulsePhaserIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "QuadLaserIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "PF4Icon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "PF5Icon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "PF6Icon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "TwinRocketsIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "SeekersIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "SpiralRocketsIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "EnergyBombIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "DronesIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "ShieldCapacity1Icon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "ShieldRecharge1Icon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "ShieldRecoveryIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "PeaShooterIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "PhotonTorpedoIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "PlasmaTorpedoIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "BF4Icon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "BF5Icon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "BF6Icon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "RingOfFireIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "HorizontalPlasmaWallIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "VerticalPlasmaWallIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "HulkIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "BD5Icon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "StaticFieldIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "KevlarIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "NanoregenerativeSubsystemsIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "TritaniumBulkheadsIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "AutoLasersIcon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "WeaponOverchargeTick", 1, 4, 8);
                AddSprite(ref All, "SpriteSheets/HUD", "SelectorSquare", 57, 58, 7);
                AddSprite(ref All, "SpriteSheets/HUD", "ShieldCapacity2Icon", 45, 45, 1);
                AddSprite(ref All, "SpriteSheets/HUD", "ShieldRecharge2Icon", 45, 45, 1);

            //misc
                AddSprite(ref All, "SpriteSheets/BombAnimSheet", "EnergyBomb", 140, 140, 7);
        }

        private void AddSprite(ref Dictionary<string, SpriteInfo> sprites, string spriteSheet, string spriteName, int width, int height, int spriteCount) {
            SpriteInfo temp = new SpriteInfo();
            temp.Filename = spriteSheet;
            
            //calc Y (look at all previous sprites in this sheet and add their heights together)
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
        public int Y; //how many pixels down the png is the top of the sprite?
        public int Width; //how wide is dat sprite
        public int Height; //how tall is dat sprite
        public int FrameCount; //how many frames does it got
        public string Filename; //what png is it in
    }
}
