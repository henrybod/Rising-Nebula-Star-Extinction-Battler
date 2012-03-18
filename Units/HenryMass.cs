using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;

namespace teamstairwell {
    [Serializable]
   public class HenryMass : HenrySprite {
        //Spawners (ships) and bullets will inherit from this class their properties of motion.

        //variables to be set upon instantiation
        protected float mass;
        protected Vector2 velocity = Vector2.Zero;
        protected Vector2 acceleration = Vector2.Zero;
        protected float damping;
        protected bool mayLeaveScreen;

        public HenryMass(ContentManager cm, float mass, Vector2 initPos, Vector2 initVel, float damping, bool mayLeaveScreen) : base(cm) {
            this.mass = mass;
            Position = initPos;
            velocity = initVel;
            this.damping = damping;
            this.mayLeaveScreen = mayLeaveScreen;
        }




        public new void Update(GameTime gt){
            //motion
            float time = (float)gt.ElapsedGameTime.TotalSeconds;
            velocity += acceleration * time;
            velocity *= (1 - damping * time); //damping
            Vector2 newPos = Position + velocity * time;
            if(!mayLeaveScreen){
                if (newPos.X < 0 || newPos.X > RNSEB.RESOLUTION.X) {
                    newPos.X = Position.X;
                    velocity.X = 0;
                }
                if (newPos.Y < 0 || newPos.Y > RNSEB.RESOLUTION.Y){
                    newPos.Y = Position.Y;
                    velocity.Y = 0;
                }
            }
            Position = newPos;
            base.Update(gt);
        }

    }
}
