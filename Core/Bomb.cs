using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hook.Graphics;
using Microsoft.Xna.Framework;
using Hook.Graphics.SpriteSheets;

namespace teamstairwell
{
    class Bomb : Bullet
    {
        static Random r;

        public Bomb(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, Sprite sprite)
            : base(position, velocity, acceleration, radius, sprite)
        {
            r = new Random();
        }

        internal static void destroy(List<Bullet> bossBullets, Bomb bomb, Player player)
        {
            for (int n = 0; n < bossBullets.Count; n++)
            {
                if(Vector2.DistanceSquared(bossBullets[n].pos, player.pos) > Vector2.DistanceSquared(bomb.pos, player.pos) && !(bossBullets[n] is Explosion))
                    bossBullets[n].removing = true;
            }

            if (r.NextDouble() > .9)
            {
                float xOff = 100 * (float)(r.NextDouble() - .5);
                float yOff = 100 * (float)(r.NextDouble() - .5);
                float ro = (float)(2 * Math.PI * r.NextDouble());
                Explosion e = new Explosion(bomb.pos + new Vector2(xOff, yOff), Vector2.Zero, Vector2.Zero, 20, new Sprite(RNSEB.PropSheet, PropSheet.SOURCEEXPLOSION));
                e.rot = e.pic.Rotation.Z = ro;
                bossBullets.Add(e);
            }
            /*for (int n = 0; n < 2; n++)
            {
                bossBullets.Add(new Explosion(new Vector2((float)(r.NextDouble() - .5) * Game1.RESOLUTION.X, bomb.pos.Y), Vector2.Zero, Vector2.Zero, 20, new Sprite(Game1.PropSheet, PropSheet.SOURCEEXPLOSION)));
            }*/
        }
    }
}
