using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;

namespace teamstairwell {

    class HenryBattlefield {

        private ContentManager cm;
        private HenrySprite background = new HenrySprite();
        public bool SpinBackground = false;
        public List<HenryBullet> bullets = new List<HenryBullet>();
        public List<HenrySpawner> spawners = new List<HenrySpawner>();
        public HenryPlayer Zihou;
        public HenryBoss Notus;

        public HenryBattlefield(ContentManager cm, string backgroundSprite) {
            this.cm = cm;
            SetBackground(backgroundSprite);
            LoadDefaults();
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
            Zihou = new HenryPlayer(cm, this);
            Zihou.Position = new Vector2(600, 600);
            Notus = new HenryBoss(cm, this);
            Notus.Position = new Vector2(200, 300);
        }

        public void Draw(SpriteBatch sb) {
            background.Draw(sb);
            Zihou.Draw(sb);
            Notus.Draw(sb);
            foreach (HenrySpawner s in spawners)
                s.Draw(sb);
        }

        public void Update(GameTime gt) {
            if (SpinBackground)
                background.Rotation += 0.065f * (float)gt.ElapsedGameTime.TotalSeconds;
            Zihou.Update(gt);
            Notus.Update(gt);
        }

    }
}
