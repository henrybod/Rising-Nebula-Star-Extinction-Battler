using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hook.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace teamstairwell.Interface
{
    public class PlayerOneVictory : Screen
    {
        TextSprite POVictory= new TextSprite("THE HUMANS WIN!");
        
        public PlayerOneVictory() : base() 
        {
           
        }

        public void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public void Draw(SpriteBatch sb)
        {
            POVictory.Draw(sb);
            base.Draw(sb);
        }

    }
}
