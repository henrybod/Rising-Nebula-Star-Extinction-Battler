/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using teamstairwell.Graphics;

namespace teamstairwell.Interface {

    class HenryUpgradeButton : HenrySprite {
 
        private HenryText buttonText;
        private enum ButtonState {
            Normal,
            Highlighted,
            Pressed
        };
        private enum UpgradeStatus {
            Obtainable,
            Unobtainable,
            Obtained
        };
        private ButtonState prevState;
        private ButtonState currState;
        private string buttonNormal, buttonHighlight, buttonClick;
        private bool used = false;
        private RNSEB.HenryUpgrade upgrade;

        public HenryUpgradeButton(int x, int y, string text, ContentManager cm, string spriteNormal, string spriteHighlight, string spritePress, RNSEB.HenryUpgrade up) : base(cm) {

            buttonNormal = spriteNormal;
            buttonHighlight = spriteHighlight;
            buttonClick = spritePress;

            base.LoadContent(buttonNormal, true); //load the button background
            this.Position = new Vector2(x, y); //set the button position where specified
            this.CenterOrigin(); //draw button measuring from center of texture

            buttonText = new HenryText(new Vector2(0.5f * RNSEB.RESOLUTION.X, 0.9f * RNSEB.RESOLUTION.Y), RNSEB.TextFont, text);
            buttonText.Color = Color.White; //set the text color
            this.cm = cm; //store the content manager reference so we can use it later to swap button textures
            upgrade = up;
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

            if (currState == ButtonState.Highlighted && prevState == ButtonState.Pressed){
                RNSEB.TheBattlefield.Zihao.AddUpgrade(upgrade);
                used = true;
            }
            
            base.Update(gt);
        }

        public new void Draw(SpriteBatch sb) {
            if(used){
                sb.Draw(Texture, Position, viewRect, Color.Gray,
                    Rotation, Origin, Scale, SpriteEffects.None, 0);
            }else
                base.Draw(sb);
            if(currState == ButtonState.Highlighted || currState == ButtonState.Pressed)
                buttonText.Draw(sb);
        }
    }
}
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using teamstairwell.Graphics;

namespace teamstairwell.Interface {

    class HenryUpgradeButton : HenryButton {

        private string name = "";
        private string description = "";
        private bool displayingText = false;
        private HenrySprite selectorSquare;
        public RNSEB.HenryUpgrade Upgrade;
        public string Requirement = "NA";
        public enum UpgradeStatus {
            Obtainable,
            Unobtainable,
            Obtained
        };
        public UpgradeStatus Status = UpgradeStatus.Unobtainable;

        public HenryUpgradeButton(string name, int column, int row, RNSEB.HenryUpgrade up, string description)
            : base((int)(0.2f*RNSEB.RESOLUTION.X + column * 0.15f * RNSEB.RESOLUTION.X),
                   (int)(0.2f*RNSEB.RESOLUTION.Y + row * 0.125f * RNSEB.RESOLUTION.Y),
                   "", new RNSEB.OnClick(()=>{}), RNSEB.cm,
                   name+"Icon", name+"Icon", name+"Icon") {

            this.name = name;
            this.Upgrade = up;
            this.description = description;
            this.Scale = 1.5f;

            selectorSquare = new HenrySprite(cm);
            selectorSquare.LoadContent("SelectorSquare", true, 10);
            selectorSquare.CenterOrigin();
            selectorSquare.Position = Position;
            selectorSquare.Scale = 1.5f;
        }

        public new void Update(GameTime gt) {
            displayingText = (GetButtonState() != ButtonState.Normal);

            Enabled = (Status == UpgradeStatus.Obtainable);

            if (Status == UpgradeStatus.Obtained)
                selectorSquare.Color = Color.Blue;
            else if (Status == UpgradeStatus.Unobtainable)
                selectorSquare.Color = Color.DimGray;
            else //is obtainable
                if (GetButtonState() == ButtonState.Normal)
                    selectorSquare.Color = Color.White;
                else if (GetButtonState() == ButtonState.Highlighted)
                    selectorSquare.Color = Color.Green;
                else if (GetButtonState() == ButtonState.Pressed)
                    selectorSquare.Color = Color.DarkGreen;

            selectorSquare.Update(gt);
            base.Update(gt);
        }

        public new void Draw(SpriteBatch sb) {
            if(displayingText)
                sb.DrawString(RNSEB.TextFont, description, new Vector2(0.2f * RNSEB.RESOLUTION.X, 0.9f * RNSEB.RESOLUTION.Y), Color.White);
            
            selectorSquare.Draw(sb);
            base.Draw(sb);
        }
    }
}