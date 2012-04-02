using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;

namespace teamstairwell.Interface.HUD {

    public class WeaponIndicator {
        
        private HenrySprite icon;
        private ProgressBar bar;
        private HenryWeapon myWeapon;
        private Vector2 position;
        private HenryText text;

        public WeaponIndicator(HenryWeapon myWeapon, Vector2 pos, string text = "") {
            icon = new HenrySprite(RNSEB.cm);
            icon.LoadContent(myWeapon.IconName, true);
            icon.Position = pos;
            this.position = pos;
            this.myWeapon = myWeapon;

            bar = new ProgressBar(RNSEB.cm, new Rectangle((int)(icon.Position.X+icon.Size.X-4), (int)(icon.Position.Y+icon.Size.Y), (int)icon.Size.Y, 3), "WeaponRechargeTick");
            bar.BarRotation = -(float)Math.PI / 2.0f;
            bar.QuantityMax = 1 / myWeapon.rateOfFire;
            bar.Quantity = bar.QuantityMax;

            this.text = new HenryText(this.position + new Vector2(10, 10), RNSEB.ButtonFont, text);
            this.text.Color = Color.White;
            this.text.Scale = 0.6f;
        }

        public void Draw(SpriteBatch sb) {
            icon.Draw(sb); 
            bar.Draw(sb);
            text.Draw(sb);
        }

        public void Update(GameTime gt) {
            icon.Update(gt);
            bar.Quantity = myWeapon.timeSinceLastFired;
            bar.Update(gt);
        }
    }
}
