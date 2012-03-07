using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;

namespace teamstairwell.Interface {

    class HenryMenu : HenryScreen {
        private string music = "MenuMusic";
        private ContentManager cm;
        private List<HenryButton> buttons = new List<HenryButton>();
        private List<HenryUpgradeButton> upgradeButtons = new List<HenryUpgradeButton>();
        private List<HenryText> texts = new List<HenryText>();
        private HenrySprite background;
        public bool SpinBackground = true;
        
        public HenryMenu(ContentManager cm, string backgroundSprite = "MenuBackground"){
            this.cm = cm;
            this.background = new HenrySprite(cm);
            this.SetBackground(backgroundSprite);
        }

        public void SetBackground(string spriteName){
            background.LoadContent(spriteName, false);
            //background image is scaled so as to prevent the edges from ever showing during rotation
            background.Scale = (float)(2.0d * Math.Sqrt((double)RNSEB.RESOLUTION.X
                                                      * (double)RNSEB.RESOLUTION.X / 4.0d
                                                      + (double)RNSEB.RESOLUTION.Y
                                                      * (double)RNSEB.RESOLUTION.Y / 4.0d
                                                    ) / (double)background.Size.Y);
            background.CenterOrigin();
            background.Position.X = (float)RNSEB.RESOLUTION.X / 2.0f;
            background.Position.Y = (float)RNSEB.RESOLUTION.Y / 2.0f;
        }

        public void AddButton(float percentX, float percentY, string text, RNSEB.OnClick callbackfunc, string spriteNormal = "ButtonNormal", string spriteHighlight = "ButtonHighlight", string spritePress = "ButtonClick", float scale = 0.6f) {
            int x = (int)(percentX * RNSEB.RESOLUTION.X);
            int y = (int)(percentY * RNSEB.RESOLUTION.Y);
            HenryButton b = new HenryButton(x, y, text, callbackfunc, cm, spriteNormal, spriteHighlight, spritePress);
            b.Scale = scale;
            
            buttons.Add(b);
        }

        public void AddUpgradeButton(float percentX, float percentY, HenryBattlefield batt, ContentManager cm, string normalSprite, string highlightedSprite, RNSEB.HenryUpgrade up, string description) {
            int x = (int)(percentX * RNSEB.RESOLUTION.X);
            int y = (int)(percentY * RNSEB.RESOLUTION.Y);
            HenryUpgradeButton b = new HenryUpgradeButton(x, y, description, cm, batt, normalSprite, highlightedSprite, highlightedSprite, up);
            b.Scale = 1.5f;
            upgradeButtons.Add(b);
        }

        public void AddText(float percentX, float percentY, SpriteFont sf, Color c, string text) {
            int x = (int)(percentX * RNSEB.RESOLUTION.X);
            int y = (int)(percentY * RNSEB.RESOLUTION.Y);
            HenryText t = new HenryText(new Vector2(x, y), sf, text);
            t.Color = c;
            texts.Add(t);
        }

        public new void Draw(SpriteBatch sb) {
            background.Draw(sb);
            foreach(HenryButton b in buttons)
                b.Draw(sb);
            foreach(HenryUpgradeButton u in upgradeButtons)
                u.Draw(sb);
            foreach(HenryText t in texts)
                t.Draw(sb);
        }

        public new void Update(GameTime gt){
            if(SpinBackground)
                background.Rotation += 0.065f * (float)gt.ElapsedGameTime.TotalSeconds;
            foreach(HenryButton b in buttons)
                b.Update(gt);
            foreach (HenryUpgradeButton u in upgradeButtons)
                u.Update(gt);
            RNSEB.Audio.PlayMusic(music);
        }

        

    }
}
