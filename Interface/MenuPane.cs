using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 

namespace teamstairwell.Interface
{
    public class MenuPane
    {
        public List<MenuItem> PaneList;

        public Vector2 Position;

        public MenuPane()
        {
            PaneList = new List<MenuItem>(); 
        }

        public void AddMenuItem(MenuItem MI)
        {
            MI.Sprite.Position = new Vector3((Position.X + MI.Position.X), (Position.Y + MI.Position.Y), 0);

            if (MI is Button)
            {
                ((Button)MI).buttonText.Position = new Vector3((Position.X + MI.Position.X),(Position.Y + MI.Position.Y) ,0);
            }
            PaneList.Add(MI);
        }

        public void Update(GameTime gt)
        {
            foreach (MenuItem MI in PaneList)
            {
                if (MI is Button)
                {
                    ((Button)MI).Update(gt);
                }
                else
                {
                    MI.Update(gt);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (MenuItem MI in PaneList)
            {
                if (MI is Button)
                {
                    ((Button)MI).Draw(sb);
                }
                else
                {
                    MI.Draw(sb); 
                }
            }
        }

    }
}
