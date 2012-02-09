#region Using Statements
using System;
using System.Collections.Generic;
//using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;
#endregion

namespace teamstairwell
{
    public class Entity
    {
        const double inflateTime = 40;
        double inflateTimer;

        internal Vector2 pos;
        internal Vector2 vel;
        internal Vector2 acc;
        internal float rot;
        internal float rotvel;
        internal float rotacc;
        internal float hitrad, baseRad, scale;
        internal Sprite pic;
        internal bool removing, trackPlayer;
        internal Player p;
        internal double removeTime, splitTimer;
        internal List<Bullet> bossBullets;
        Random r;

        public Entity()
        {
            pic = null;
            pos = Vector2.Zero;
            vel = Vector2.Zero;
            acc = Vector2.Zero;
            rot = rotvel = rotacc = 0;
            hitrad = 0;
            removing = false;

            pic = new Sprite(RNSEB.PlayerSheet, PlayerSheet.PNORMAL);
        }

        public Entity(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius)
        {
            pos = position;
            vel = velocity;
            acc = acceleration;
            rot = rotvel = rotacc = 0;
            hitrad = radius;
            removing = false;
            r = new Random();

            pic = new Sprite(RNSEB.PlayerSheet, PlayerSheet.PNORMAL);
        }

        public Entity(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, Player pl)
        {
            pos = position;
            vel = velocity;
            acc = acceleration;
            rot = rotvel = rotacc = 0;
            hitrad = baseRad = radius;
            removing = false;
            p = pl;
            trackPlayer = true;
            inflateTimer = 0;
            r = new Random();

            pic = new Sprite(RNSEB.PlayerSheet, PlayerSheet.PNORMAL);
        }

        public Entity(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, List<Bullet> bBs)
        {
            pos = position;
            vel = velocity;
            acc = acceleration;
            rot = rotvel = rotacc = 0;
            hitrad = baseRad = radius;
            removing = false;
            bossBullets = bBs;
            splitTimer = 2;
            r = new Random();

            pic = new Sprite(RNSEB.PlayerSheet, PlayerSheet.PNORMAL);
        }

        public virtual void update(GameTime gt)
        {
            if (trackPlayer)
            {
                Vector2 dir = p.pos - pos;
                dir.Normalize();
                acc = .025f * dir;

                inflateTimer += gt.ElapsedGameTime.TotalSeconds;
                if (inflateTimer >= inflateTime)
                {
                    inflateTimer = inflateTime;
                }
                hitrad = 25 + (float)(inflateTimer / inflateTime) * 125;
                scale = .5f + (float)(inflateTimer / inflateTime) * 5.0f;
                pic.Scale = scale;
            }

            if (splitTimer > 0)
            {
                splitTimer -= gt.ElapsedGameTime.TotalSeconds;
                if (splitTimer <= 0)
                {
                    if (r.NextDouble() > .35)
                    {
                        if (RNSEB.GUMBALLMODE)
                        {
                            bossBullets.Add(new Bullet(pos, new Vector2(vel.Y, -vel.X), Vector2.Zero, 15, new Sprite(RNSEB.PropSheet, PropSheet.GUM1), bossBullets));
                        }
                        else
                        {
                            bossBullets.Add(new Bullet(pos, new Vector2(vel.Y, -vel.X), Vector2.Zero, 15, new Sprite(RNSEB.PropSheet, PropSheet.ORANGEBULLET), bossBullets));
                        }
                    }
                    if (r.NextDouble() > .35)
                    {
                        if (RNSEB.GUMBALLMODE)
                        {
                            bossBullets.Add(new Bullet(pos, new Vector2(-vel.Y, vel.X), Vector2.Zero, 15, new Sprite(RNSEB.PropSheet, PropSheet.GUM1), bossBullets));
                        }
                        else
                        {
                            bossBullets.Add(new Bullet(pos, new Vector2(-vel.Y, vel.X), Vector2.Zero, 15, new Sprite(RNSEB.PropSheet, PropSheet.ORANGEBULLET), bossBullets));
                        }
                    }
                    remove();
                }
            }

            vel += acc;
            pos += vel;
            rotvel += rotacc;
            rot += rotvel;

            pic.Update(gt);
            pic.Position.X = pos.X;
            pic.Position.Y = pos.Y;
            pic.Rotation.Z = rot;

            if (removing)
            {
                if (removeTime > 0)
                {
                    removeTime -= gt.ElapsedGameTime.TotalSeconds;

                    if (removeTime < 0)
                    {
                        removeTime = 0;
                    }
                }
            }
        }

        public void draw(SpriteBatch sb)
        {
            pic.Draw(sb, rot);
        }

        public void remove()
        {
            removing = true;
        }
        public void remove(double time)
        {
            removing = true;
            removeTime = time;
        }
        public static bool collideFuckMAtt(Entity a, Entity b)
        {
            return Vector2.Distance(a.pos, b.pos) < a.hitrad + b.hitrad;
        }
    }
}
