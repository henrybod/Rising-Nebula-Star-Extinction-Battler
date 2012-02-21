using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;

namespace teamstairwell.Interface {

    class HenryMenu {
        private string music = "MenuMusic";
        private ContentManager cm;
        private List<HenryButton> buttons = new List<HenryButton>();
        private List<HenryText> texts = new List<HenryText>();
        private HenrySprite background = new HenrySprite();
        public bool SpinBackground = true;
        
        public HenryMenu(ContentManager cm){
            this.cm = cm;
        }

        public void SetBackground(string spriteName){
            background.LoadContent(cm, spriteName);
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

        public void AddButton(float percentX, float percentY, string text, RNSEB.HenryScreen link, string spriteName = "Button", float scale = 0.6f) {
            int x = (int)(percentX * RNSEB.RESOLUTION.X);
            int y = (int)(percentY * RNSEB.RESOLUTION.Y);
            HenryButton b = new HenryButton(x, y, text, link, cm, spriteName);
            b.Scale = scale;
            if (spriteName != "Button")
                b.Animate = true;
            
            buttons.Add(b);
        }

        public void AddText(float percentX, float percentY, SpriteFont sf, Color c, string text) {
            int x = (int)(percentX * RNSEB.RESOLUTION.X);
            int y = (int)(percentY * RNSEB.RESOLUTION.Y);
            HenryText t = new HenryText(new Vector2(x, y), sf, text);
            t.Color = c;
            texts.Add(t);
        }

        public void Draw(SpriteBatch sb) {
            background.Draw(sb);
            foreach(HenryButton b in buttons)
                b.Draw(sb);
            foreach(HenryText t in texts)
                t.Draw(sb);
        }

        public void Update(GameTime gt){
            if(SpinBackground)
                background.Rotation += 0.065f * (float)gt.ElapsedGameTime.TotalSeconds;
            foreach(HenryButton b in buttons)
                b.Update(gt);
            RNSEB.Audio.Play(music);
        }

        

    }
}
