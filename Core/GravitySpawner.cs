using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;

namespace teamstairwell
{
    class GravitySpawner : BulletSpawner
    {
        List<Bullet> bullets;

        public GravitySpawner(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, float rotation, float rotationVelocity, List<Bullet> bossBullets, Boolean retract, int numberofshots, double rateoffire)
            : base(position, velocity, acceleration, radius, rotation, rotationVelocity, bossBullets, retract, numberofshots, rateoffire)
        {
            bullets = bossBullets;
            normalSprite = new Sprite(RNSEB.PropSheet, PropSheet.MAGNETGUY);
            selectSprite = new Sprite(RNSEB.PropSheet, PropSheet.MAGNETGUYAURA);
            pic = normalSprite;
            //pic = new Sprite(Game1.PropSheet, PropSheet.GRAVITYSOURCE);
        }

        public override void SpawnB()
        {
            for (int n = 0; n < bullets.Count; n++)
            {
                Vector2 distance = bullets[n].pos - pos;
                float length = distance.Length();
                distance.Normalize();
                bullets[n].vel -= 15 * power / length * distance;
            }
        }
    }
}
