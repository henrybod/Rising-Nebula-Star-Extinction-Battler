using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;
using Microsoft.Xna.Framework;

namespace teamstairwell
{
    class SplitSpawner : BulletSpawner
    {
        List<Bullet> bullets;

        public SplitSpawner(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, float rotation, float rotationVelocity, List<Bullet> bossBullets, Boolean retract, int numberofshots, double rateoffire)
            : base(position, velocity, acceleration, radius, rotation, rotationVelocity, bossBullets, retract, numberofshots, rateoffire)
        {
            bullets = bossBullets;
            normalSprite = new Sprite(Game1.PropSheet, PropSheet.HEXSPAWN);
            selectSprite = new Sprite(Game1.PropSheet, PropSheet.HEXSPAWNAURA);
            pic = normalSprite;
            //pic = new Sprite(Game1.PropSheet, PropSheet.GRAVITYSOURCE);
        }

        public override void SpawnB()
        {
            if (Game1.GUMBALLMODE)
            {
                bullets.Add(new Bullet(pos, vel.Length() * new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot)), Vector2.Zero, 15, new Sprite(Game1.PropSheet, PropSheet.GUM1), bullets));
            }
            else
            {
                bullets.Add(new Bullet(pos, vel.Length() * new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot)), Vector2.Zero, 15, new Sprite(Game1.PropSheet, PropSheet.ORANGEBULLET), bullets));
            }
        }
    }
}
