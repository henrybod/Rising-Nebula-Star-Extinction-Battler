using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;

namespace teamstairwell.Interface {

    class HenryShieldBar {

        private ContentManager cm;
        private HenrySprite shieldTick = new HenrySprite();
        private HenryPlayer ship;
        private float shieldAmount = 0.0f;
        public float ShieldAmount{
            set{
                if (value < 0)
                    shieldAmount = 0.0f;
                else if (value > ship.ShieldMax)
                    shieldAmount = ship.ShieldMax;
                else
                    shieldAmount = value;
            }
        }

        public HenryShieldBar(ContentManager cm, HenryPlayer ship){
            this.cm = cm;
            this.ship = ship;
            shieldTick.LoadContent(cm, "ShieldTick");
            shieldTick.Position.Y = RNSEB.RESOLUTION.Y - 20;
        }
        public void Draw(SpriteBatch sb){
            float ticks = RNSEB.RESOLUTION.X * (ship.Shield / (float)ship.ShieldMax);
            ticks = (int)ticks;
            for(int i = 0; i < ticks; i++){
                shieldTick.Position.X = i;
                shieldTick.Draw(sb);
            }
        }
    }
}
