using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell.Graphics {

    [Serializable()]
    public class HenryText {

        public string Text;
        [field: NonSerialized]
        public SpriteFont Font;
        public Vector2 Position = Vector2.Zero;
        public Vector2 Origin = Vector2.Zero;
        public float RotationAngle = 0.0f;
        public SpriteEffects SpriteEffect = SpriteEffects.None;
        public Color Color = Color.Black;
        public float Scale = 1.0f;

        public HenryText(Vector2 pos, SpriteFont sf, string s = "") {
            Position = pos;
            Font = sf;
            Text = s;
            Origin = new Vector2(Font.MeasureString(Text).X / 2, Font.MeasureString(Text).Y / 2);
            RNSEB.Win.ClientSizeChanged += new EventHandler<EventArgs>(ResolutionChange); //subscribe to window change event
        }

        public void Draw(SpriteBatch sb) {
            sb.DrawString(Font, Text, Position, Color, RotationAngle, Origin, Scale, SpriteEffect, 0);
        }

        private void ResolutionChange(object sender, EventArgs e) {
            GameWindow window = (GameWindow)sender;
            Position *= (new Vector2(window.ClientBounds.Width, window.ClientBounds.Height)) / RNSEB.RESOLUTION;
            Scale *= window.ClientBounds.Width / RNSEB.RESOLUTION.X;
        }

        public void ReloadContent(string font) {
            //Font = RNSEB.Fonts[font];
        }
    }
}
