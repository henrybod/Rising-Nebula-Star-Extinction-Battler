using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;
using Microsoft.Xna.Framework;

namespace teamstairwell
{
    class Explosion : Bullet
    {
        public Explosion(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, Sprite sprite)
            : base(position, velocity, acceleration, radius, sprite)
        {
            sprite.Position = new Vector3(position.X, position.Y, 0);
            sprite.FrameRate = 7;
            remove(.75);
        }
    }
}
