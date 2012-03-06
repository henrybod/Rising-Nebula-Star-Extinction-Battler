using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;
using teamstairwell.Interface;

namespace teamstairwell {

    class HenryBattlefield : HenryScreen {

        private string music = "Level1Music";
        private ContentManager cm;
        private HenrySprite background;
        public bool SpinBackground = false;
        public List<HenryBullet> bullets = new List<HenryBullet>();
        public List<HenrySpawner> spawners = new List<HenrySpawner>();
        public HenryPlayer Zihao;
        public HenryBoss Notus;
        public int LevelNumber = 1;

        public HenryBattlefield(ContentManager cm, string backgroundSprite) {
            this.cm = cm;
            background = new HenrySprite(cm);
            SetBackground(backgroundSprite);
            LoadDefaults();
            SpinBackground = true;
        }

        public void SetBackground(string filename) {
            background.LoadContent(filename, false);
            background.Scale = (float)(2.0d * Math.Sqrt((double)RNSEB.RESOLUTION.X
                                                      * (double)RNSEB.RESOLUTION.X / 4.0d
                                                      + (double)RNSEB.RESOLUTION.Y
                                                      * (double)RNSEB.RESOLUTION.Y / 4.0d
                                                    ) / (double)background.Size.Y);
            background.CenterOrigin();
            background.Position.X = (float)RNSEB.RESOLUTION.X / 2.0f;
            background.Position.Y = (float)RNSEB.RESOLUTION.Y / 2.0f;
        }

        public void LoadDefaults() {
            //adds boss and player
            Zihao = new HenryPlayer(cm, this, 100, new Vector2(600, 600), new Vector2(0,0), 0.9999999999999f);
            Notus = new HenryBoss(cm, this, 1000,new Vector2(200, 300), new Vector2(0,0), 0.5f);
            spawners.Add(Notus);
        }

        public override void Draw(SpriteBatch sb) {
            background.Draw(sb);
            Notus.Draw(sb);
            Zihao.Draw(sb);
        }

        public override void Update(GameTime gt) {
            //link to upgrade, pause menus
            if (RNSEB.Input.GetKey("Pause"))
                RNSEB.CurrentScreen = "PauseMenu";
            if (1 - Notus.Health/(float)Notus.HealthMax > LevelNumber/10.0f) {
                LevelNumber++;
                music = "Level" + LevelNumber.ToString() + "Music";
                RNSEB.CurrentScreen = "PlayerUpgradeMenu";
            }

            if (SpinBackground)
                background.Rotation += 0.065f * (float)gt.ElapsedGameTime.TotalSeconds;
            Zihao.Update(gt);
            Notus.Update(gt);
            RNSEB.Audio.PlayMusic(music);
        }
    }
}
