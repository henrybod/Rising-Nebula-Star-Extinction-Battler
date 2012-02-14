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
 
        private HenryText buttonText;
        private ContentManager cm;

        public HenryButton(int x, int y, string text, ContentManager cm, string filename) {
            base.LoadContent(cm, filename); //load the button background
            buttonText = new HenryText(text); //load the textbox
            buttonText.Color = Color.Teal; //set the text color
            //word wrap the text

            this.Position = new Vector2(x, y); //set the button position where specified
            this.CenterOrigin(); //place button measuring from center of texture
            buttonText.Position.X = x;// + this.Texture.Width / 2.0f; //set text position to center of button
            buttonText.Position.Y = y;// + this.Texture.Height / 2.0f; //set text position to center of button

            this.cm = cm; //store the content manager reference so we can use it later to swap button textures
            
        }

        public void Update(GameTime gt) {
            base.Update(gt, Vector2.Zero, Vector2.Zero); //buttons have no speed or direction
            //check mouse hover/click
            if(HenryInput.MouseIsIn(this.Position, this.Size))
                if(HenryInput.M1) LoadContent(cm, "SpriteSheets/ButtonClick");
                else LoadContent(cm, "SpriteSheets/ButtonHighlight");
            else LoadContent(cm, "SpriteSheets/ButtonNormal");
            //todo: also change text color
            
        }

        public new void Draw(SpriteBatch sb) {
            base.Draw(sb); //draw the button sprite first
            buttonText.Draw(sb); //then draw the text on top of it
        }


    }
}
