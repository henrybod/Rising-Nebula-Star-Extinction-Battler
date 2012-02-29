using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using teamstairwell.Graphics;

namespace teamstairwell.Interface {

    public class HenryButton : HenrySprite {
 
        private HenryText buttonText;
        private string link;
        private enum ButtonState {
            Normal,
            Highlighted,
            Pressed
        };
        private ButtonState prevState;
        private ButtonState currState;
        private string buttonNormal, buttonHighlight, buttonClick;


        public HenryButton(int x, int y, string text, string link, ContentManager cm, string spriteNormal, string spriteHighlight, string spritePress) : base(cm) {

            buttonNormal = spriteNormal;
            buttonHighlight = spriteHighlight;
            buttonClick = spritePress;

            base.LoadContent(buttonNormal, true); //load the button background
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
            if(RNSEB.Input.MouseIsIn(Position, Size)){
                if(RNSEB.Input.GetKey("Mouse1")) {
                    //mouse is pressing button
                    if(currState != ButtonState.Pressed){
                        LoadContent(buttonClick, true);
                        buttonText.Color = Color.Aqua;
                        currState = ButtonState.Pressed;
                        RNSEB.Audio.PlayEffect("ButtonClick");
                    }
                } else {
                    //mouse is hovering over button
                    if(currState != ButtonState.Highlighted){
                        LoadContent(buttonHighlight, true);
                        buttonText.Color = Color.Aqua;
                        if(currState != ButtonState.Pressed) RNSEB.Audio.PlayEffect("ButtonRollover");
                        currState = ButtonState.Highlighted;
                    }
                }
            } else {
                //mouse isn't over button
                if(currState != ButtonState.Normal){
                    LoadContent(buttonNormal, true);
                    buttonText.Color = Color.Red;
                    currState = ButtonState.Normal;
                    RNSEB.Audio.PlayEffect("ButtonRollover");
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
