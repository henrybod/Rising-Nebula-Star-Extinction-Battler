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
        private enum ButtonState {
            Normal,
            Highlighted,
            Pressed
        };
        private ButtonState prevState;
        private ButtonState currState;
        private string buttonNormal, buttonHighlight, buttonClick;
        public RNSEB.OnClick onClick;
        public bool Enabled = true;


        public HenryButton(int x, int y, string text,  RNSEB.OnClick onClick, ContentManager cm, string spriteNormal, string spriteHighlight, string spritePress) : base(cm) {
            
            buttonNormal = spriteNormal;
            buttonHighlight = spriteHighlight;
            buttonClick = spritePress;
            base.LoadContent(buttonNormal, true); //load the button background
            this.Position = new Vector2(x, y); //set the button position where specified
            this.CenterOrigin(); //draw button measuring from center of texture

            buttonText = new HenryText(this.Position, RNSEB.ButtonFont, text);
            buttonText.Color = Color.Red; //set the text color
            this.cm = cm; //store the content manager reference so we can use it later to swap button textures

            this.onClick = onClick; //store the lambda function to call when clicked
        }

        public new void Update(GameTime gt) {
            if(!Enabled) return;
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

            if(currState == ButtonState.Highlighted && prevState == ButtonState.Pressed){
                //I've been clicked! Ouch! Right in the eye! Son of a ...!
                LoadContent(buttonNormal, true);
                buttonText.Color = Color.Red;
                onClick();
            }
            
            base.Update(gt); //update view (in case base.frame has changed)
        }

        public new void Draw(SpriteBatch sb) {
            //draw the button sprite first
            if(Enabled) base.Draw(sb);
            else
                sb.Draw(Texture, Position, viewRect, Color.Gray,
                    Rotation, Origin, Scale, SpriteEffects.None, 0);

            buttonText.Draw(sb); //then draw the text on top of it
        }
    }
}
