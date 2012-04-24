﻿using System;
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
        private ProgressBar chargeBar, superBar;
        private HenryWeapon myWeapon;
        private Vector2 position;
        private HenryText text;

        public WeaponIndicator(HenryWeapon myWeapon, Vector2 pos, string text = "") {
            icon = new HenrySprite(RNSEB.cm);
            icon.LoadContent(myWeapon.IconName, true);
            icon.Position = pos;
            this.position = pos;
            this.myWeapon = myWeapon;

            chargeBar = new ProgressBar(RNSEB.cm, new Rectangle((int)(icon.Position.X+icon.Size.X-4), (int)(icon.Position.Y+icon.Size.Y), (int)icon.Size.Y, 3), "WeaponRechargeTick");
            //chargeBar = new ProgressBar(RNSEB.cm, new Rectangle((int)icon.Position.X, (int)icon.Position.Y+(int)icon.Size.Y, (int)icon.Size.Y, (int)icon.Size.X), "WeaponRechargeTick");
            chargeBar.BarRotation = -(float)Math.PI / 2.0f;
            chargeBar.QuantityMax = 1 / myWeapon.rateOfFire;
            chargeBar.Quantity = chargeBar.QuantityMax;

            this.text = new HenryText(this.position + new Vector2(10, 10), RNSEB.ButtonFont, text);
            this.text.Color = Color.White;
            this.text.Scale = 0.6f;
        }

        public void Draw(SpriteBatch sb) {
            icon.Draw(sb); 
            chargeBar.Draw(sb);
            text.Draw(sb);
        }

        public void Update(GameTime gt) {
            icon.Update(gt);
            chargeBar.Quantity = myWeapon.timeSinceLastFired;
            chargeBar.Update(gt);
        }
    }
}
