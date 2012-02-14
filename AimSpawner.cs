using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;

namespace teamstairwell
{
    class AimSpawner : BulletSpawner
    {
        Player pla;
        public AimSpawner(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, float rotation, float rotationVelocity, List<Bullet> bossBullets, Boolean retract, int numberofshots, double rateoffire, Player player)
            : base(position, velocity, acceleration, radius, rotation, rotationVelocity, bossBullets, retract, numberofshots, rateoffire)
        {
            normalSprite = new Sprite(RNSEB.PropSheet, PropSheet.SKULL);
            selectSprite = new Sprite(RNSEB.PropSheet, PropSheet.SKULLAURA);
            pic = normalSprite;
            pla = player;
        }

        public override void SpawnB()
        {
            float vx = (float)Math.Cos(rot + Math.PI / 2);
            float vy = (float)Math.Sin(rot + Math.PI / 2);

            Vector2 topla = new Vector2(pla.pos.X - pos.X, pla.pos.Y - pos.Y);
            topla.Normalize();
            rot = (float)Math.Acos(topla.X) - (float)Math.PI / 2;
            float ax = 0;
            float ay = 0;
            /*if (doRetract)
            {
                ax = -.005f * vx;
                ay = -.005f * vy;
            }*/
            if (RNSEB.GUMBALLMODE)
            {
                bBs.Add(new Bullet(pos, (.5f + power * 3 / 9) * topla, new Vector2(ax, ay), 7.5f, new Sprite(RNSEB.PropSheet, PropSheet.GUM1)));
            }
            else
            {
                bBs.Add(new Bullet(pos, (.5f + power * 3 / 9) * topla, new Vector2(ax, ay), 7.5f, new Sprite(RNSEB.PropSheet, PropSheet.BULLETA)));
            }
        }
    }
}
