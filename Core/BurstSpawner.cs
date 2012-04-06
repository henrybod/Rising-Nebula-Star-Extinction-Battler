using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;

namespace teamstairwell
{
    class BurstSpawner : BulletSpawner
    {
        int bulletNum;

        public BurstSpawner(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, float rotation, float rotationVelocity, List<Bullet> bossBullets, Boolean retract, int bulletCount, int numberofshots, double rateoffire)
            : base(position, velocity, acceleration, radius, rotation, rotationVelocity, bossBullets, retract, numberofshots, rateoffire)
        {
            bulletNum = bulletCount;
            doRetract = retract;
            normalSprite = new Sprite(RNSEB.PropSheet, PropSheet.RADIAL);
            selectSprite = new Sprite(RNSEB.PropSheet, PropSheet.RADIALAURA);
            pic = normalSprite;
        }

        public override void SpawnB()
        {
            bulletNum = 3 * power;
            for (float i = 0.0f; i < bulletNum; i += 1.0f)
            {
                float vx = (float)Math.Cos(((2.0f * Math.PI) * (i / bulletNum)) + rot);
                float vy = (float)Math.Sin(((2.0f * Math.PI) * (i / bulletNum)) + rot);

                float ax = 0;
                float ay = 0;
                if (doRetract)
                {
                    ax = -.02f * vx;
                    ay = -.02f * vy;
                }

                if (RNSEB.GUMBALLMODE)
                {
                    bBs.Add(new Bullet(pos, new Vector2(2 * vx, 2 * vy), new Vector2(ax, ay), 7.5f, new Sprite(RNSEB.PropSheet, PropSheet.GUM1)));
                }
                else
                {
                    bBs.Add(new Bullet(pos, new Vector2(2 * vx, 2 * vy), new Vector2(ax, ay), 7.5f, new Sprite(RNSEB.PropSheet, PropSheet.BULLETB)));
                }
            }
        }
    }
}
