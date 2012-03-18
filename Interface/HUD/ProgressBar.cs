using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;

namespace teamstairwell.Interface.HUD {
    public class ProgressBar {

        private HenrySprite shieldTick;
        private Rectangle area;
        public float Quantity = 0;
        public float QuantityMax = 0;
        public float BarRotation = 0;
        public float TickRotation = 0;

        public ProgressBar(ContentManager cm, Rectangle area, string spriteName){
            shieldTick = new HenrySprite(cm);
            shieldTick.LoadContent(spriteName, false);
            shieldTick.Scale = area.Height / shieldTick.Size.Y;
            this.area = area;
        }

        public void Draw(SpriteBatch sb){
            //calculate the number of sprite draws to fill the bar
            int ticks = (int)(area.Width / shieldTick.Size.X * (Quantity / QuantityMax));
            for (int i = 0; i < ticks; i++){
                shieldTick.Position.Y = area.Y + i * shieldTick.Size.X * (float)Math.Sin(BarRotation);
                shieldTick.Position.X = area.X + i * shieldTick.Size.X * (float)Math.Cos(BarRotation);
                shieldTick.Rotation = BarRotation + TickRotation;
                shieldTick.Draw(sb);
            }
        }
    }
}
