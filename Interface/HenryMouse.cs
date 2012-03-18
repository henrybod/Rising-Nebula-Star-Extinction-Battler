using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using teamstairwell.Graphics;

namespace teamstairwell.Interface {
    //the mouse is simply a sprite that moves with the mouse input
    class HenryMouse : HenrySprite {

        private bool targeting = false;
        private Color currentColor;

        public HenryMouse(ContentManager cm) : base(cm){
            LoadContent("Cursor", true);
        }

        public new void Update(GameTime gt){
            base.Position = RNSEB.Input.GetCursor();

            //change to target cursor on battlefield
            if (RNSEB.CurrentScreen == "Battlefield"){
                targeting = true;
                Oscillate = true;
                currentColor = RNSEB.TheBattlefield.BossMode ? Color.Red : Color.Blue;
            }else{
                targeting = false;
                Oscillate = false;
                currentColor = Color.White;
            }
            base.FrameSpeed = 10;
            base.Update(gt);
        }

        public new void Draw(SpriteBatch sb){
            if (targeting) {
                sb.Draw(Texture, Position, viewRect, currentColor, Rotation, Origin, 0.75f, SpriteEffects.None, 0);
                sb.Draw(Texture, Position, viewRect, currentColor, Rotation + (float)Math.PI / 2, Origin, 0.75f, SpriteEffects.None, 0);
                sb.Draw(Texture, Position, viewRect, currentColor, Rotation + (float)Math.PI, Origin, 0.75f, SpriteEffects.None, 0);
                sb.Draw(Texture, Position, viewRect, currentColor, Rotation - (float)Math.PI / 2, Origin, 0.75f, SpriteEffects.None, 0);
            } else
            base.Draw(sb);
        }
    }
}
