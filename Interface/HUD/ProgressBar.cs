using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;

namespace teamstairwell.Interface.HUD {
    [Serializable()]
    public class ProgressBar {
        //a HUD element to display a progress bar. border and number display only work correctly when bar is horizontal.

        public HenrySprite tick;
        public HenrySprite borderPixel;
        public HenryText text;
        public Rectangle area;
        private float quantity = 0;
        public float Quantity {
            get { return quantity; }
            set {
                if(value >= 0) quantity = value;
                if(quantity > quantityMax) quantity = quantityMax;
            }
        }
        private float quantityMax = 0;
        public float QuantityMax {
            get { return quantityMax; }
            set { if(value >= 0) quantityMax = value; }
        }
        public float BarRotation = 0;
        public float TickRotation = 0;
        public bool ShowValue = false, ShowBorder = false;
        private int valueAccuracy = 0;
        public int ValueAccuracy {
            get { return valueAccuracy; }
            set { if(value >= 0) valueAccuracy = value; }
        }



        public ProgressBar(ContentManager cm, Rectangle area, string spriteName) {
            tick = new HenrySprite(cm);
            text = new HenryText(new Vector2(area.Center.X, area.Center.Y-15), cm.Load<SpriteFont>("ButtonFont"));
            text.Color = Color.White;
            tick.LoadContent(spriteName, true, 5);
            tick.Scale = area.Height / tick.Size.Y;
            this.area = area;
            borderPixel = new HenrySprite(cm);
            borderPixel.LoadContent("WhiteDot", false);

            //RNSEB.Win.ClientSizeChanged += new EventHandler<EventArgs>(ResolutionChange);
        }

        public void Draw(SpriteBatch sb) {
            //calculate the number of sprite draws to fill the bar
            int ticks = (int)(area.Width / tick.Size.X * (Quantity / QuantityMax));
            for (int i = 0; i < ticks; i++) {
                tick.Position.Y = area.Y + i * tick.Size.X * (float)Math.Sin(BarRotation);
                tick.Position.X = area.X + i * tick.Size.X * (float)Math.Cos(BarRotation);
                tick.Rotation = BarRotation + TickRotation;
                tick.Draw(sb);
            }

            //update font (if enabled) and display
            if (ShowValue) {
                text.Text = Quantity.ToString("n"+ValueAccuracy);
                text.Draw(sb);
            }

            //draw a border (if enabled) and display
            if (ShowBorder) {
                borderPixel.Position.Y = area.Top - 1;
                for(int i = 0; i < area.Width; i++) {
                    borderPixel.Position.X = area.Left+i;
                    borderPixel.Draw(sb);
                }
                borderPixel.Position.Y = area.Bottom + 1;
                for (int i = 0; i < area.Width; i++) {
                    borderPixel.Position.X = area.Left + i;
                    borderPixel.Draw(sb);
                }
                borderPixel.Position.X = area.Left - 1;
                for (int i = 0; i < area.Height; i++) {
                    borderPixel.Position.Y = area.Top + i;
                    borderPixel.Draw(sb);
                }
                borderPixel.Position.X = area.Right + 1;
                for (int i = 0; i < area.Height; i++) {
                    borderPixel.Position.Y = area.Top + i;
                    borderPixel.Draw(sb);
                }
            }
        }

        public void Update(GameTime gt) {
            tick.Update(gt);
        }

        public void Diag() {
            Console.WriteLine("ProgressBar Diagnostic:");
            Console.WriteLine(" > Location (" + area.X + ", " + area.Y + ")");
            Console.WriteLine(" > Value (" + Quantity + " / " + QuantityMax + ")");
        }

        private void ResolutionChange(object sender, EventArgs e) {
            GameWindow window = (GameWindow)sender;
            //Position *= (new Vector2(window.ClientBounds.Width, window.ClientBounds.Height)) / RNSEB.RESOLUTION;
            float scale = window.ClientBounds.Width / RNSEB.RESOLUTION.X;
            area.Height = (int)(area.Height * scale);
            area.Width = (int)(area.Width * scale);
            area.X = (int)(area.X * scale);
            area.Y = (int)(area.Y * scale);
            Diag();
        }
    }
}
