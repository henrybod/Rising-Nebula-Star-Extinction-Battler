using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;
using teamstairwell.Graphics;

namespace teamstairwell.Interface {
    public class HenryButton : HenrySprite {
 
        private HenryText buttonText;
        private ContentManager cm;
        private RNSEB.HenryScreen link;
        private enum ButtonState {
            Normal,
            Highlighted,
            Pressed
        };
        private ButtonState prevState;
        private ButtonState currState;


        public HenryButton(int x, int y, string text, RNSEB.HenryScreen link, ContentManager cm, string spriteName) {
            base.LoadContent(cm, spriteName); //load the button background
            this.Position = new Vector2(x, y); //set the button position where specified
            this.CenterOrigin(); //draw button measuring from center of texture

            buttonText = new HenryText(this.Position, RNSEB.ButtonFont, text);
            buttonText.Color = Color.Red; //set the text color
            this.cm = cm; //store the content manager reference so we can use it later to swap button textures

            this.link = link; //store where a click will take us
        }

        public new void Update(GameTime gt) {
            prevState = currState; //current state is now old
            //check mouse hover/click
            if(HenryInput.MouseIsIn(Position, Size)){
                if(HenryInput.M1) {
                    //mouse is pressing button
                    if(currState != ButtonState.Pressed){
                        if(!this.Animate) base.Frame = 2;
                        buttonText.Color = Color.Aqua;
                        currState = ButtonState.Pressed;
                        RNSEB.Audio.Play("ButtonClick");
                    }
                } else {
                    //mouse is hovering over button
                    if(currState != ButtonState.Highlighted){
                        if (!this.Animate) base.Frame = 1;
                        buttonText.Color = Color.Aqua;
                        if(currState != ButtonState.Pressed) RNSEB.Audio.Play("ButtonRollover");
                        currState = ButtonState.Highlighted;

                    }
                }
            } else {
                //mouse isn't over button
                if(currState != ButtonState.Normal){
                    if(!this.Animate) base.Frame = 0;
                    buttonText.Color = Color.Red;
                    currState = ButtonState.Normal;
                    RNSEB.Audio.Play("ButtonRollover");
                }
            }

            if(currState == ButtonState.Highlighted && prevState == ButtonState.Pressed)
                RNSEB.CurrentScreen = this.link;
            
            base.Update(gt); //update view (in case base.frame has changed)
        }

        public new void Draw(SpriteBatch sb) {
            base.Draw(sb); //draw the button sprite first
            buttonText.Draw(sb); //then draw the text on top of it
        }
    }
}
