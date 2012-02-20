using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;

namespace teamstairwell.Interface {

    class HenryBattlefield {

        private ContentManager cm;
        private HenrySprite background = new HenrySprite();
        public bool SpinBackground = false;
        public List<HenryBullet> bullets = new List<HenryBullet>();
        public List<HenrySpawner> spawners = new List<HenrySpawner>();
        private HenryPlayer zihou;
        private HenryBoss notus;
        //public HenryHealthbar BossHP, ZihouSP;

        public HenryBattlefield(ContentManager cm) {
            this.cm = cm;
        }

        public void SetBackground(string filename) {
            background.LoadContent(cm, filename);
            background.Scale = (float)(2.0d * Math.Sqrt((double)RNSEB.RESOLUTION.X
                                                      * (double)RNSEB.RESOLUTION.X / 4.0d
                                                      + (double)RNSEB.RESOLUTION.Y
                                                      * (double)RNSEB.RESOLUTION.Y / 4.0d
                                                    ) / (double)background.Size.Y);
            background.CenterOrigin();
            background.Position.X = (float)RNSEB.RESOLUTION.X / 2.0f;
            background.Position.Y = (float)RNSEB.RESOLUTION.Y / 2.0f;
        }

        public void LoadDefaults(){
            //adds boss and player
            zihou = new HenryPlayer(cm);
            zihou.Position = new Vector2(600, 600);
        }

        public void AddSpawner(Vector2 initPos, Vector2 initDir, Vector2 initVel, float clipRadius, string filename) {
            //will be called by boss
        }

        public void AddBullet(Vector2 initPos, Vector2 initDir, Vector2 initVel, float clipRadius, string filename) {
            //will be called by spawners, player
        }

        public void Draw(SpriteBatch sb) {
            background.Draw(sb);
            zihou.Draw(sb);
            //notus.Update(gt);
            foreach (HenryBullet b in bullets)
                b.Draw(sb);
            foreach (HenrySpawner s in spawners)
                s.Draw(sb);
        }

        public void Update(GameTime gt) {
            if (SpinBackground)
                background.Rotation += 0.065f * (float)gt.ElapsedGameTime.TotalSeconds;
            zihou.Update(gt);
            //notus.Update(gt);
        }



    }
}
