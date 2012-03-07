using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;

namespace teamstairwell.Interface {

    class HenryHealthBar {

        private ContentManager cm;
        private HenrySprite healthTick;
        private HenryBoss ship;
        private float healthAmount = 0.0f;
        public float HealthAmount{
            set{
                if (value < 0)
                    healthAmount = 0.0f;
                else if (value > ship.HealthMax)
                    healthAmount = ship.HealthMax;
                else
                    healthAmount = value;
            }
        }

        public HenryHealthBar(ContentManager cm, HenryBoss ship){
            this.cm = cm;
            this.ship = ship;
            healthTick = new HenrySprite(cm);
            healthTick.LoadContent("HealthTick", true);
            healthTick.Position.Y = -4;
        }

        public void Draw(SpriteBatch sb){
            float ticks = RNSEB.RESOLUTION.X * (ship.Health / (float)ship.HealthMax);
            ticks = (int)(ticks);
            for(int i = 0; i < ticks; i += 3){
                healthTick.Position.X = i;
                healthTick.Draw(sb);
            }
        }
    }
}
