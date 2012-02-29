using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Hook.Graphics.SpriteSheets
{
    public class PropSheet : SpriteSheet
    {
        /*public const int WHEEL = 0;
        public const int THICKLINK = 1;
        public const int THINLINK = 2;
        public const int PLAYER = 3;
        public const int START = 4;
        public const int FINISH = 5;
        public const int DIE = 6;*/

        public const int GUM0 = 0;
        public const int GUM1 = 1;
        public const int MISSLE = 2;
        public const int SKULL = 3;
        public const int SKULLAURA = 4;
        public const int LINE = 5;
        public const int LINEAURA = 6;
        public const int RADIAL = 7;
        public const int RADIALAURA = 8;
        public const int HORIZON = 9;
        public const int HORIZONAURA = 10;
        public const int CROSS = 11;
        public const int CROSSAURA = 12;
        public const int ONEUP = 13;
        public const int BOMBGET = 14;
        public const int HEALTHTICK = 15;
        public const int TINYRETICLE = 16;
        public const int DOOMCURSOR = 17;
        public const int SOURCEEXPLOSION = 18;
        public const int BULLETA = 19;
        public const int BULLETB = 20;
        public const int BULLETC = 21;
        public const int GRAVITYSOURCE = 22;
        public const int GRAVITYSOURCEAURA = 23;
        public const int LASER = 24;
        public const int ORANGEBULLET = 25;
        public const int LARGEBULLET = 26;
        public const int MAGNETGUY = 27;
        public const int MAGNETGUYAURA = 28;
        public const int TARGET = 29;
        public const int BOUY = 30;
        public const int GOLDLASER = 31;
        public const int ROCKET = 32;
        public const int G = 33;
        public const int M = 34;
        public const int K = 35;
        public const int D = 36;
        public const int P = 37;
        public const int HEXSPAWN = 38;
        public const int HEXSPAWNAURA = 39;
        public const int R = 40;


        public PropSheet(Texture2D SheetImage)
            : base()
        {
            this.SheetImage = SheetImage;

            FrameValues = new int[,]{
                {15, 15, 4},   //GUM0
                {15, 15, 4},   //GUM1
                {15, 15, 4},   //MISSLE
                {45, 45, 5},   //SKULL
                {45, 45, 1},   //SKULLAURA
                {45, 45, 1},   //LINE
                {45, 45, 1},   //LINEAURA
                {45, 45, 1},   //RADIAL
                {45, 45, 1},   //RADIALAURA
                {45, 45, 1},   //Horizontal Spawner A
                {45, 45, 1},   //Horizontal Spawner B
                {45, 45, 1},   //Cross Spawner A
                {45, 45, 1},   //Cross Spawner B
                {25, 25, 4},   //1-up
                {25, 25, 4},   //BombGet
                { 3, 20, 1},   //HealthTick
                { 5,  5, 1},   //TinyReticle
                {10, 20, 1},   //DoomCursor
                {50, 50, 4},   //SourceExplosion
                {15, 15, 4},   //BulletA
                {15, 15, 4},   //BulletB
                {15, 15, 4},   //BulletC
                {45, 45, 1},   //GravitySource
                {45, 45, 1},    //GravitySourceActive
                {15, 15, 4},   //laser
                {30, 30, 4},   //orangefuzz
                {60, 60, 16},   //largeball
                {45, 45, 5},   //magnetguy
                {45, 45, 1},   //auramagnet
                {60, 60, 2},   //target
                {30, 30, 1},    //bouy
                {15, 15, 4},   //gold laser
                {15, 15, 4},   //missle
                {25, 25, 4},   //G
                {25, 25, 4},    //M
                {25, 25, 4},   //K
                {25, 25, 4},   //D
                {25, 25, 4},    //P
                {45, 45, 5},   //Hex Spawner
                {45, 45, 1},    //Hex Spawner Highlighted
                {25, 25, 4}    //R
            };

            /*FrameValues = new int[,]{
                {505, 507, 1},   //Wheel
                {30, 20, 1},    //Thick Chain Link
                {30, 10, 1},     //Thin Chain Link
                {40, 60, 2},     //Player
                {40, 40, 1},     //Level start
                {40, 40, 1},     //Level finish
                {80, 80, 1}     //5 dot die
            };*/
        }
    }
}
