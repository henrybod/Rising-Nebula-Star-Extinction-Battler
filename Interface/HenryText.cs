using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell.Interface {

    class HenryText {

        public string Text;
        public SpriteFont Font = RNSEB.GameFont;
        public Vector2 Position = Vector2.Zero;
        public Vector2 Origin = Vector2.Zero;
        public float RotationAngle = 0.0f;
        public SpriteEffects SpriteEffect = SpriteEffects.None;
        public Color Color = Color.Black;
        public float Scale = 1.0f;

        public HenryText(string s = ""){
            Text = s;
            Origin = new Vector2(Font.MeasureString(Text).X / 2, Font.MeasureString(Text).Y / 2);
        }

        public void Draw(SpriteBatch sb) {
            sb.DrawString(Font, Text, Position, Color, RotationAngle, Origin, Scale, SpriteEffect, 0);
        }
        
        //Will we conceivably need an update method?

    }
}
