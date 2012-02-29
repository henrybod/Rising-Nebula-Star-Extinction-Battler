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

    class HenryUpgradeButton : HenrySprite {
 
        private HenryText description;
        private RNSEB.HenryUpgrade up;
        private HenryBattlefield battlefield;
        private enum ButtonState {
            Normal,
            Highlighted,
            Pressed
        };
        private ButtonState prevState, currState;
        private bool used = false, drawDescription = false;
        private string normalSprite, highlightedSprite;


        public HenryUpgradeButton(int x, int y, HenryBattlefield b, ContentManager cm, string normalSprite, string highlightedSprite, RNSEB.HenryUpgrade up, string description) : base(cm) {
            base.LoadContent(normalSprite, true); //load the button background
            base.Position = new Vector2(x, y); //set the button position where specified
            base.CenterOrigin(); //draw button measuring from center of texture
            base.Oscillate = true;
            base.cm = cm; //store the content manager reference so we can use it later to swap button textures
            this.battlefield = b; //store the battlefield so we use it to reference the player / boss later to grant our upgrade
            this.normalSprite = normalSprite;
            this.highlightedSprite = highlightedSprite;
            this.up = up;
            this.description = new HenryText(new Vector2(RNSEB.RESOLUTION.X / 2.0f, RNSEB.RESOLUTION.Y / 1.2f), RNSEB.TextFont, description);
        }

        public new void Update(GameTime gt) {
            prevState = currState; //current state is now old
            //check mouse hover/click
            if(RNSEB.Input.MouseIsIn(Position, Size)){
                //i'm being hovered over
                drawDescription = true;
                currState = ButtonState.Highlighted;

                if(!used && RNSEB.Input.GetKey("Mouse1")){
                    //i'm being clicked! ow!
                    base.LoadContent(highlightedSprite, true);
                    currState = ButtonState.Pressed;
                }

            }else{
                //i'm not being hovered over
                drawDescription = false;
                currState = ButtonState.Normal;
            }

            if(currState == ButtonState.Highlighted && prevState == ButtonState.Pressed){
                if(!used) battlefield.Zihou.AddUpgrade(up);
                used = true;
            }
                
            
            base.Update(gt); //update view (in case base.frame has changed)
        }

        public new void Draw(SpriteBatch sb) {
            base.Draw(sb); //draw the button sprite first
            if(drawDescription) description.Draw(sb); //then draw the text on top of it
        }
    }
}
