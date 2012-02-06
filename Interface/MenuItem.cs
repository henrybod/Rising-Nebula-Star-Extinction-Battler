using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hook.Graphics;

namespace teamstairwell.Interface
{
    public class MenuItem
    {
        public Sprite Sprite;
        public Vector2 Position, Dimension; 

        public MenuItem()
        {

        }

        public void Update(GameTime gt)
        {
            if (Sprite != null)
            {
                Sprite.Update(gt);
            }

        }

        public void Draw(SpriteBatch sb)
        {
            if (Sprite != null)
            {
                Sprite.Draw(sb);
            }
        }

    }
}
