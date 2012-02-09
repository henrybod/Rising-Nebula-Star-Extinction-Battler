using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;

namespace teamstairwell.Interface {
    public class HenryButton : HenrySprite {
 
        public TextSprite buttonText;

        public HenryButton(int x, int y, string text, ContentManager cm, string filename = "SpriteSheets/ButtonBlank") {
            base.LoadContent(cm, filename); //load the button background
            buttonText = new TextSprite(text); //load the button text
            buttonText.Color = Color.Black; //set the text color
            
            this.Position = new Vector2(x, y); //set the button position where specified
            buttonText.Position.X = x + this.Texture.Width / 2.0f; //set text position to center of button
            buttonText.Position.Y = y + this.Texture.Height / 2.0f; //set text position to center of button
            
        }

        public void Update(GameTime gt) {
            base.Update(gt, Vector2.Zero, Vector2.Zero); //buttons have no speed or direction
            buttonText.Update(gt); //should they have updating text?
        }

        public new void Draw(SpriteBatch sb) {
            base.Draw(sb);
            buttonText.Draw(sb);
        }


    }
}
