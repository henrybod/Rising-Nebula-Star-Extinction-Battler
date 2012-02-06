using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Hook.Graphics;

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
