using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell.Interface {

    class HenryMenu {

        private ContentManager cm;
        private List<HenryButton> buttons = new List<HenryButton>();
        //private List<HenryTextField> texts = new List<HenryTextField>();
        private HenrySprite background = new HenrySprite();
        public bool SpinBackground = false;

        public HenryMenu(ContentManager cm){
            this.cm = cm;
        }

        public void SetBackground(string filename){
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

        public void AddButton(float percentX, float percentY, string text, string filename = "SpriteSheets/ButtonNormal") {
            int x = (int)(percentX * RNSEB.RESOLUTION.X);
            int y = (int)(percentY * RNSEB.RESOLUTION.Y);
            HenryButton b = new HenryButton(x, y, text, cm, filename);
            b.Scale = 0.6f;
            buttons.Add(b);
        }

        public void Draw(SpriteBatch sb) {
            background.Draw(sb);
            foreach(HenryButton b in buttons)
                b.Draw(sb);
        }

        public void Update(GameTime gt){
            if(SpinBackground)
                background.Rotation += 0.065f * (float)gt.ElapsedGameTime.TotalSeconds;
            foreach(HenryButton b in buttons)
                b.Update(gt);
            
        }

        

    }
}
