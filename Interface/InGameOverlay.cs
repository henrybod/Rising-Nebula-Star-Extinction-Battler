using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hook.Graphics.SpriteSheets;
using Hook.Graphics;
using teamstairwell.Graphics.SpriteSheets;
using Microsoft.Xna.Framework.Media;

namespace teamstairwell.Interface
{
    public class InGameOverlay : Screen
    {
        bool backFlag;
        Sprite Sbackground = new Sprite(Game1.Background, Background.BACKGROUND); 
        
        public GameEnvironment Env;

        public InGameOverlay()
            : base()
        {
            MenuBack = null;
            Env = new GameEnvironment();
        }

        public new void Update(GameTime gt)
        {
            if (!backFlag && Game1.GUMBALLMODE)
            {
                Sbackground = new Sprite(Game1.GumBackground, GumBackground.BACKGROUND);
                backFlag = true;
            }

            Sbackground.Update(gt);
            Sbackground.Rotation.Z += .002f;
            base.Update(gt);
            Env.update(gt);
        }

        public new void Draw(SpriteBatch sb)
        {
            Sbackground.Draw(sb);
            Env.draw(sb); 
            base.Draw(sb);
        }

        public void LinkButtons()
        {

        }


    }
}
