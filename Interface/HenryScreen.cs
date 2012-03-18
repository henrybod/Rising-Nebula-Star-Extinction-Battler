using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell.Interface {

    public class HenryScreen {

        //this is a just a parent for the menus and battlefield so I can make a dictionary of them
        virtual public void Update(GameTime gt){}
        virtual public void Draw(SpriteBatch sb) {}

    }
}
