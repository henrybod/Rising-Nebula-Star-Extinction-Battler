using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;

namespace teamstairwell
{
    class TwinSpawner : BulletSpawner
    {
        float split;
        public TwinSpawner(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, float rotation, float rotationVelocity, List<Bullet> bossBullets, Boolean retract, int numberofshots, double rateoffire, float midAngle)
            : base(position, velocity, acceleration, radius, rotation, rotationVelocity, bossBullets, retract, numberofshots, rateoffire)
        {
            split = midAngle;
        }

        public override void SpawnB()
        {
            float vx = (float)Math.Cos(rot);
            float vy = (float)Math.Sin(rot);

            float ax = 0;
            float ay = 0;

            if (doRetract)
            {
                ax = -.005f * vx;
                ay = -.005f * vy;
            }

            bBs.Add(new Bullet(pos, new Vector2(2 * vx, 2 * vy), new Vector2(ax, ay), 7.5f, new Sprite(Game1.PropSheet, PropSheet.GUM0)));
            
            vx = (float)Math.Cos(rot + split);
            vy = (float)Math.Sin(rot + split);

            ax = 0;
            ay = 0;

            if (doRetract)
            {
                ax = -.005f * vx;
                ay = -.005f * vy;
            }
            bBs.Add(new Bullet(pos, new Vector2(2 * vx, 2 * vy), new Vector2(ax, ay), 7.5f, new Sprite(Game1.PropSheet, PropSheet.GUM0)));
            
            base.SpawnB();
        }
    }
}
