using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;
using System.Runtime.Serialization;

namespace teamstairwell.Interface {

    [Serializable()]
    public class HenryScreen {

        //this is a just a parent for the menus and battlefield so I can make a dictionary of them

        //one thing all screens have in common is a background, so that is handled here

        protected HenrySprite background = new HenrySprite(RNSEB.cm);
        public bool SpinBackground = true;

        public virtual void Update(GameTime gt) {
            if (SpinBackground)
                background.Rotation += 0.065f * (float)gt.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(SpriteBatch sb) {
            background.Draw(sb);
        }

        public void SetBackground(string spriteName) {
            background.LoadContent(spriteName, false);
            //background image is scaled so as to prevent the edges from ever showing during rotation
            background.Scale = (float)(2.0d * Math.Sqrt((double)RNSEB.RESOLUTION.X
                                                      * (double)RNSEB.RESOLUTION.X / 4.0d
                                                      + (double)RNSEB.RESOLUTION.Y
                                                      * (double)RNSEB.RESOLUTION.Y / 4.0d
                                                    ) / (double)background.Size.Y);
            background.CenterOrigin();
            background.Position.X = (float)RNSEB.RESOLUTION.X / 2.0f;
            background.Position.Y = (float)RNSEB.RESOLUTION.Y / 2.0f;
        }

        
    }
}
