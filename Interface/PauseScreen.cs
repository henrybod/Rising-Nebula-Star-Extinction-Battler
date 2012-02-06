using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hook.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace teamstairwell.Interface
{
    public class PauseScreen : Screen
    {
        TextSprite Paused;
        public PauseScreen() : base() 
        {
            Paused = new TextSprite("GAME BE PAUSED!");
            Paused.Color = Color.White;
            Paused.Position = new Vector3(Game1.RESOLUTION.X / 2 - 40, Game1.RESOLUTION.Y / 2 - 60, 0);
        }

        public void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public void Draw(SpriteBatch sb )
        {
            base.Draw(sb);
            Paused.Draw(sb);
        }

    }
}
