using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace teamstairwell.Interface {
    //the mouse is simply a sprite that moves with the mouse input
    class HenryMouse : HenrySprite {

        public void Update(GameTime gt){
            base.Position = new Vector2(HenryInput.MouseX, HenryInput.MouseY);
            //also change cursor
        }

        public new void Draw(SpriteBatch sb){
            base.Draw(sb);
        }
    }
}
