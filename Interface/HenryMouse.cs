using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace teamstairwell.Interface {
    class HenryMouse : HenrySprite {
        MouseState oldState = new MouseState();
        MouseState newState = new MouseState();

        public HenryMouse(ContentManager cm){
            LoadContent(cm, "buttonBlank");
            oldState = Mouse.GetState();
        }

        public void Update(GameTime gt){
            newState = Mouse.GetState();

        }
    }
}
