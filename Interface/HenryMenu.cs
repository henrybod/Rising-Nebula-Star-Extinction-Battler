using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;

namespace teamstairwell.Interface {

    [Serializable()]
    public class HenryMenu : HenryScreen {

        protected string music = "MenuMusic";
        public Dictionary<string, HenryButton> Buttons = new Dictionary<string, HenryButton>();
        protected List<HenryText> texts = new List<HenryText>();
        
        public HenryMenu(ContentManager cm, string backgroundSprite = "MenuBackground"){

            this.SetBackground(backgroundSprite);
        }

        public void AddButton(float percentX, float percentY, string text, RNSEB.OnClick callbackfunc, string spriteNormal = "ButtonNormal", string spriteHighlight = "ButtonHighlight", string spritePress = "ButtonClick", float scale = 0.6f) {
            int x = (int)(percentX * RNSEB.RESOLUTION.X);
            int y = (int)(percentY * RNSEB.RESOLUTION.Y);
            HenryButton b = new HenryButton(x, y, text, callbackfunc, RNSEB.cm, spriteNormal, spriteHighlight, spritePress);
            b.Scale = scale;
            
            Buttons.Add(text, b);
        }

        public void AddText(float percentX, float percentY, SpriteFont sf, Color c, string text) {
            int x = (int)(percentX * RNSEB.RESOLUTION.X);
            int y = (int)(percentY * RNSEB.RESOLUTION.Y);
            HenryText t = new HenryText(new Vector2(x, y), sf, text);
            t.Color = c;
            texts.Add(t);
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);
            foreach(HenryButton b in Buttons.Values)
                b.Draw(sb);
            foreach(HenryText t in texts)
                t.Draw(sb);
        }

        public override void Update(GameTime gt){
            base.Update(gt);
            foreach(HenryButton b in Buttons.Values)
                b.Update(gt);
            RNSEB.Audio.PlayMusic(music);
        }

        public void clearTexts()
        {
            texts.Clear();
        }

        public void clearButtons()
        {
            Buttons.Clear();
        }

    }
}
