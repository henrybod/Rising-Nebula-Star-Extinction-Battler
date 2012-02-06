using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;

namespace teamstairwell
{
    class RollSpawner : BulletSpawner
    {
        List<Bullet> bullets;
        Player p;

        public RollSpawner(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, float rotation, float rotationVelocity, List<Bullet> bossBullets, Boolean retract, int numberofshots, double rateoffire, Player pl)
            : base(position, velocity, acceleration, radius, rotation, rotationVelocity, bossBullets, retract, numberofshots, rateoffire)
        {
            bullets = bossBullets;
            normalSprite = new Sprite(Game1.PropSheet, PropSheet.GRAVITYSOURCE);
            selectSprite = new Sprite(Game1.PropSheet, PropSheet.GRAVITYSOURCEAURA);
            pic = normalSprite;
            p = pl;
            //pic = new Sprite(Game1.PropSheet, PropSheet.GRAVITYSOURCE);
        }

        public override void SpawnB()
        {
            if (Game1.GUMBALLMODE)
            {
                bullets.Add(new Bullet(pos, Vector2.Zero, Vector2.Zero, 30, new Sprite(Game1.PropSheet, PropSheet.GUM1), p));
            }
            else
            {
                bullets.Add(new Bullet(pos, Vector2.Zero, Vector2.Zero, 30, new Sprite(Game1.PropSheet, PropSheet.LARGEBULLET), p));
            }
        }
    }
}
