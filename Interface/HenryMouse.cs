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

        public new void Update(GameTime gt){
            base.Position = new Vector2(HenryInput.MouseX, HenryInput.MouseY);
            //also change cursor
            //we need some sort of target / crosshair thing for the battlefield
            base.Animate = true;
            base.FrameSpeed = 10;
            base.Update(gt);
        }

        public new void Draw(SpriteBatch sb){
            base.Draw(sb);
        }
    }
}
