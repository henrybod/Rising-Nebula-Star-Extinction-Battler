using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using teamstairwell.Graphics;
using Microsoft.Xna.Framework;
using Hook.Graphics;

namespace teamstairwell {
    class HenryEntity : HenrySprite {
        public Vector2 Direction, Speed, Acceleration;
        public SpriteSheet Ss = new SpriteSheet();
        
        
        public HenryEntity(string filename){
            //look into spritesheets
            //Ss.FrameValues
        }

        public void Update(GameTime gt){         
            Position += Direction * Speed * (float)gt.ElapsedGameTime.TotalSeconds;
        }
    }
}
