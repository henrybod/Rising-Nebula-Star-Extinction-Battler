using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Hook.Graphics;
using Microsoft.Xna.Framework.Input;

namespace teamstairwell
{
    public class Bullet : Entity
    {
        public Bullet(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, Sprite sprite)
            : base(position, velocity, acceleration, radius)
        {
            pos = position;
            vel = velocity;
            acc = acceleration;
            hitrad = radius;

            if (sprite != null)
            {
                pic = sprite;
            }
        }

        //Creates bullet with related sprite rotation and velocity.
        public Bullet(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, Sprite sprite, float angle)
            : base(position, velocity, acceleration, radius)
        {
            pos = position;
            vel = velocity;
            acc = acceleration;
            hitrad = radius;

            rot = angle;
            angle = angle + MathHelper.ToRadians(90);
            vel.X = (float)Math.Cos(angle) * velocity.X;
            vel.Y = (float)Math.Sin(angle) * velocity.Y;

            if (sprite != null)
            {
                pic = sprite;
            }
        }

        //Creates a bullet with SEPARATE sprite rotation and velocity (not trigonometrically related)
        public Bullet(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, Sprite sprite, float angle, float drawAngle)
            : base(position, velocity, acceleration, radius)
        {
            pos = position;
            vel = velocity;
            acc = acceleration;
            hitrad = radius;

            rot = drawAngle;
            angle = angle + MathHelper.ToRadians(90);
            vel.X = (float)Math.Cos(angle) * velocity.X;
            vel.Y = (float)Math.Sin(angle) * velocity.Y;

            if (sprite != null)
            {
                pic = sprite;
            }
        }

        public Bullet(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, Sprite sprite, Player p)
            : base(position, velocity, acceleration, radius, p)
        {
            pos = position;
            vel = velocity;
            acc = acceleration;
            hitrad = radius;

            if (sprite != null)
            {
                pic = sprite;
            }
        }

        public Bullet(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, Sprite sprite, List<Bullet>bBs)
            : base(position, velocity, acceleration, radius, bBs)
        {
            pos = position;
            vel = velocity;
            acc = acceleration;
            hitrad = radius;

            if (sprite != null)
            {
                pic = sprite;
            }
        }
    }
}
