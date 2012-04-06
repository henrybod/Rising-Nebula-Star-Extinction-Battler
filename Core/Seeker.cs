using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Hook.Graphics;

namespace teamstairwell
{
    class Seeker : Bullet
    {
        Vector2 target;
        float targRot;

        public Seeker(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, Sprite sprite, float rot)
            : base(position, velocity, acceleration, radius, sprite, rot)
        {
            target = new Vector2(0, 0);
        }

        public void lockOn(Vector2 targ)
        {
            target = targ;
            /*
            targRot = (float)(Math.Atan2(target.Y - pos.Y, target.X - pos.X) + Math.PI);

            float clockWise = (float)(((rot + 2 * Math.PI) - targRot) % (2 * Math.PI));
            float counterWise = (float)(((targRot + 2 * Math.PI) - rot) % (2 * Math.PI));

            if (clockWise >= counterWise)
            {
                rot += MathHelper.ToRadians(5);
            }
            else
            {
                rot -= MathHelper.ToRadians(5);
            }*/
            rot = (float)(Math.Atan2(target.Y - pos.Y, target.X - pos.X) + Math.PI/2);
            vel.X = (float)Math.Cos(rot + Math.PI/2) * -5;
            vel.Y = (float)Math.Sin(rot + Math.PI/2) * -5;
        }
    }
}