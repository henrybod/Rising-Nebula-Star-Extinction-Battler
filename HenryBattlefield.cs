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
        private List<HenryEntity> entities = new List<HenryEntity>();
        private HenrySprite background = new HenrySprite();
        public bool SpinBackground = false;
        //public HenryHealthbar BossHP, ZihouHP, ZihouSP;

        public HenryBattlefield(ContentManager cm) {
            this.cm = cm;
        }

        public void SetBackground(string filename) {
            background.LoadContent(cm, filename);
            //background.Scale = (float)RNSEB.RESOLUTION.X / (float)background.Size.Width; //todo: recalc scale to hide edges
            background.Scale = (float)(2.0d * Math.Sqrt((double)RNSEB.RESOLUTION.X
                                                      * (double)RNSEB.RESOLUTION.X / 4.0d
                                                      + (double)RNSEB.RESOLUTION.Y
                                                      * (double)RNSEB.RESOLUTION.Y / 4.0d
                                                    ) / (double)background.Size.Y);
            background.CenterOrigin();
            background.Position.X = (float)RNSEB.RESOLUTION.X / 2.0f;
            background.Position.Y = (float)RNSEB.RESOLUTION.Y / 2.0f;
        }

        public void AddEntity(Vector2 initPos, float clipRadius, string filename) {
            
        }

        public void Draw(SpriteBatch sb) {
            background.Draw(sb);
            foreach (HenryEntity e in entities)
                e.Draw(sb);
        }

        public void Update(GameTime gt) {
            if (SpinBackground)
                background.Rotation += 0.065f * (float)gt.ElapsedGameTime.TotalSeconds;
            foreach (HenryEntity e in entities)
                e.Update(gt);

        }



    }
}
