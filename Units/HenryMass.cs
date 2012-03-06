using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;

namespace teamstairwell {

    class HenryMass : HenrySprite {
        //Spawners (ships) and bullets will inherit from this class their properties of motion.

        //variables to be set upon instantiation
        protected float mass;
        public Vector2 Velocity = Vector2.Zero;
        public Vector2 acceleration = Vector2.Zero;
        protected float damping;
        protected bool mayLeaveScreen;

        public HenryMass(ContentManager cm, float mass, Vector2 initPos, Vector2 initVel, float damping, bool mayLeaveScreen) : base(cm) {
            this.mass = mass;
            Position = initPos;
            Velocity = initVel;
            this.damping = damping;
            this.mayLeaveScreen = mayLeaveScreen;
        }




        public new void Update(GameTime gt){
            //motion
            float time = (float)gt.ElapsedGameTime.TotalSeconds;
            Velocity += acceleration * time;
            Velocity *= (1 - damping * time); //damping
            Vector2 newPos = Position + Velocity * time;
            if(!mayLeaveScreen){
                if (newPos.X < 0 || newPos.X > RNSEB.RESOLUTION.X) {
                    newPos.X = Position.X;
                    Velocity.X = -Velocity.X;
                }
                if (newPos.Y < 0 || newPos.Y > RNSEB.RESOLUTION.Y){
                    newPos.Y = Position.Y;
                    Velocity.Y = -Velocity.Y;
                }
            }
            Position = newPos;
            base.Update(gt);
        }

    }
}
