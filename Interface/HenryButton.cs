using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using teamstairwell.Graphics;

namespace teamstairwell.Interface {

    [Serializable()]
    public class HenryButton : HenrySprite {

        public HenryText buttonText;
        public enum ButtonState {
            Normal,
            Highlighted,
            Pressed,
            JustClicked
        };
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
            
            //RNSEB.Win.ClientSizeChanged += new EventHandler<EventArgs>(ResolutionChange); //subscribe to window change event
        }

        public new void Update(GameTime gt) {
            if(!Enabled || currState == GetButtonState()) {
                base.Update(gt);
                return;
            }

            //check mouse hover/click
            if(GetButtonState() == ButtonState.Pressed) {
                //mouse is pressing button
                LoadContent(buttonClick, true);
                buttonText.Color = Color.Aqua;
                if (!(this is HenryUpgradeButton))
                    RNSEB.Audio.PlayEffect("ButtonClick");
            } else if (GetButtonState() == ButtonState.Highlighted) {
                //mouse is hovering over button
                LoadContent(buttonHighlight, true);
                buttonText.Color = Color.Aqua;
                if(currState != ButtonState.Pressed) RNSEB.Audio.PlayEffect("ButtonRollover");
            } else if (GetButtonState() == ButtonState.Normal) {
                //mouse isn't over button
                LoadContent(buttonNormal, true);
                buttonText.Color = Color.Red;
                RNSEB.Audio.PlayEffect("ButtonRollover");
            } else if (GetButtonState() == ButtonState.JustClicked){
                //I've been clicked! Ouch! Right in the eye! Son of a ...!
                LoadContent(buttonNormal, true);
                buttonText.Color = Color.Red;
                onClick();
            }
            
            base.Update(gt); //update view (in case base.frame has changed)
            currState = GetButtonState();
        }

        public new void Draw(SpriteBatch sb) {
            //draw the button sprite first
            if(Enabled) base.Draw(sb);
            else //draw grayed out button
                sb.Draw(Texture, Position, viewRect, Color.Gray,
                    Rotation, Origin, Scale, SpriteEffects.None, 0);

            buttonText.Draw(sb); //then draw the text on top of it
        }

        public ButtonState GetButtonState() {
            if(!Enabled) return ButtonState.Normal;
            ButtonState bs = ButtonState.Normal;
            if(RNSEB.Input.MouseIsIn(Position, Size)) bs = ButtonState.Highlighted;
            if(RNSEB.Input.MouseIsIn(Position, Size) && RNSEB.Input.GetKey("Mouse1")) bs = ButtonState.Pressed;
            if(currState == ButtonState.Pressed && bs == ButtonState.Highlighted) bs = ButtonState.JustClicked;
            return bs;
        }

                
        private void ResolutionChange(object sender, EventArgs e){
            GameWindow window = (GameWindow)sender;
            Position *= (new Vector2(window.ClientBounds.Width, window.ClientBounds.Height)) / RNSEB.RESOLUTION;
        }

        public new void ReloadContent() {
            buttonText = new HenryText(Position, RNSEB.ButtonFont, "");
            base.ReloadContent();
        }
    }
}
