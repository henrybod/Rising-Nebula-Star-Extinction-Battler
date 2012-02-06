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
using teamstairwell.Graphics.SpriteSheets;
#endregion

namespace teamstairwell
{
    public class Player : Entity
    {
        List<Bullet> pBs;
        public int live;
        public int bomb;
        public TimeSpan invisibleTime = TimeSpan.Zero;
        TimeSpan invisibleSpan = new TimeSpan(0, 0, 0, 2, 500);
        public bool invisible;
        KeyboardState oldstate = Keyboard.GetState();

        public bool fury;
        public bool surge;
        public bool tracker;
        public bool angleGun;
        public bool angleRocket;
        public bool rotTurretR;
        float rotTurretAngleR = 0;
        public bool rotTurretL;
        float rotTurretAngleL = (float)Math.PI;
        public TimeSpan trackTime = TimeSpan.Zero;
        public TimeSpan trackSpan = new TimeSpan(0, 0, 0, 10, 0);
        public TimeSpan furyTime = TimeSpan.Zero;
        public TimeSpan furySpan = new TimeSpan(0, 0, 0, 7, 0);

        Sprite normalSprite, shieldSprite, boomSprite, bankLeftSprite, bankRightSprite;

        public Player(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, List<Bullet> playerBullets, int lives, int bombs)
            : base(position, velocity, acceleration, radius)
        {
            live = lives;
            bomb = bombs;
            pBs = playerBullets;
            invisible = false;

            normalSprite = new Sprite(Game1.PlayerSheet, PlayerSheet.PNORMAL);
            shieldSprite = new Sprite(Game1.PlayerSheet, PlayerSheet.PSHIELD);
            boomSprite = new Sprite(Game1.PlayerSheet, PlayerSheet.PBOOM);
            bankLeftSprite = new Sprite(Game1.PlayerSheet, PlayerSheet.PGOLEFT);
            bankRightSprite = new Sprite(Game1.PlayerSheet, PlayerSheet.PGORIGHT);

            pic = normalSprite;
        }

        public override void update(GameTime time)
        {
            Console.WriteLine(pos);

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up) && state.IsKeyUp(Keys.Down))
            {
                vel.Y = -5;
            }
            else if (state.IsKeyDown(Keys.Down) && state.IsKeyUp(Keys.Up))
            {
                vel.Y = 5;
            }
            else
                vel.Y = 0;

            if (state.IsKeyDown(Keys.Left) && state.IsKeyUp(Keys.Right))
            {
                vel.X = -5;
                if (pic == normalSprite || pic == bankRightSprite)
                {
                    pic = bankLeftSprite;
                }
            }
            else if (state.IsKeyDown(Keys.Right) && state.IsKeyUp(Keys.Left))
            {
                vel.X = 5;
                if (pic == normalSprite || pic == bankLeftSprite)
                {
                    pic = bankRightSprite;
                }
            }
            else
            {
                vel.X = 0;
                if (pic == bankRightSprite || pic == bankLeftSprite)
                {
                    pic = normalSprite;
                }
            }

            if (state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift))
            {
                vel = vel / 2;
            }

            if (state.IsKeyDown(Keys.Z))
            {
                Bullet b = new Bullet(pos, new Vector2(0, -12), new Vector2(0, 0), 0f, new Sprite(Game1.PropSheet, PropSheet.LASER));
                pBs.Add(b);
                if (fury)
                {
                    Vector2 lpos = pos;
                    lpos.X -= 7;
                    Vector2 rpos = pos;
                    rpos.X += 7;
                    pBs.Add(new Bullet(lpos, new Vector2(0, -12), new Vector2(0, 0), 0f, new Sprite(Game1.PropSheet, PropSheet.LASER)));
                    pBs.Add(new Bullet(rpos, new Vector2(0, -12), new Vector2(0, 0), 0f, new Sprite(Game1.PropSheet, PropSheet.LASER)));
                    lpos.X -= 11;
                    rpos.X += 11;
                    pBs.Add(new Bullet(lpos, new Vector2(-1, -12), new Vector2(0, 0), 0f, new Sprite(Game1.PropSheet, PropSheet.LASER)));
                    pBs.Add(new Bullet(rpos, new Vector2(1, -12), new Vector2(0, 0), 0f, new Sprite(Game1.PropSheet, PropSheet.LASER)));
                }
                if (surge)
                {
                    Vector2 lpos = pos;
                    lpos.X -= 7;
                    Vector2 rpos = pos;
                    rpos.X += 7;
                    pBs.Add(new Bullet(lpos, new Vector2(0, -12), new Vector2(0, 0), 0f, new Sprite(Game1.PropSheet, PropSheet.GOLDLASER)));
                    pBs.Add(new Bullet(rpos, new Vector2(0, -12), new Vector2(0, 0), 0f, new Sprite(Game1.PropSheet, PropSheet.GOLDLASER)));
                }
                if (angleGun)
                {
                    Vector2 lpos = pos;
                    lpos.X -= 11;
                    Vector2 rpos = pos;
                    rpos.X += 11;
                    pBs.Add(new Bullet(lpos, new Vector2(-2, -6), new Vector2(0, -2), 0f, new Sprite(Game1.PropSheet, PropSheet.MISSLE)));
                    pBs.Add(new Bullet(rpos, new Vector2(2, -6), new Vector2(0, -2), 0f, new Sprite(Game1.PropSheet, PropSheet.MISSLE)));
                }
                if (angleRocket)
                {
                    Vector2 lpos = pos;
                    lpos.X -= 7;
                    Vector2 rpos = pos;
                    rpos.X += 7;
                    pBs.Add(new Bullet(lpos, new Vector2(-15, 0), new Vector2(.1f, -6), 0f, new Sprite(Game1.PropSheet, PropSheet.ROCKET)));
                    pBs.Add(new Bullet(rpos, new Vector2(15, 0), new Vector2(-.1f, -6), 0f, new Sprite(Game1.PropSheet, PropSheet.ROCKET)));
                }
                if (rotTurretR)
                {
                    pBs.Add(turretRotR());
                }
                if (rotTurretL)
                {
                    pBs.Add(turretRotL());
                }
                //shoot
            }

            if (state.IsKeyDown(Keys.X) && oldstate.IsKeyUp(Keys.X))
            {
                if (bomb > 0)
                {
                    int numbomb = 20;
                    for (int i = 0; i < numbomb; i++)
                    {
                        Vector2 v = new Vector2((float)Math.Cos(Math.PI * 2 * i / numbomb), (float)Math.Sin(Math.PI * 2 * i / numbomb));
                        pBs.Add(new Bomb(pos, v * 10, Vector2.Zero, 200f, new Sprite(Game1.DaBomb, ScreenBomb.SCREENBOMB)));
                    }
                    bomb--;
                    invisible = true;
                    invisibleTime = time.TotalGameTime;
                }
                //bomb
            }

            if (invisible)
            {
                pic = shieldSprite;
                if (time.TotalGameTime - invisibleTime > invisibleSpan)
                {
                    invisible = false;
                    pic = normalSprite;
                }
            }

            if (fury)
            {
                if (time.TotalGameTime - furyTime > furySpan)
                {
                    fury = false;
                }
            }

            oldstate = Keyboard.GetState();
            Vector2 oldPos = pos;
            base.update(time);

            if (GameEnvironment.entityOffScreen(this))
            {
                pos = oldPos;
            }
        }

        internal void death()
        {
            boomSprite.Position = normalSprite.Position;
            pic = boomSprite;
            base.remove(1);
        }

        internal Bullet turretRotR()
        {
            rotTurretAngleR++;
            rotTurretAngleR %= (float)Math.PI;
            Vector2 veldir = new Vector2((float)Math.Cos(rotTurretAngleR + (float)Math.PI), (float)Math.Sin(rotTurretAngleR + (float)Math.PI));

            return new Bullet(pos, veldir * 4, Vector2.Zero, 0f, new Sprite(Game1.PropSheet, PropSheet.MISSLE));
        }

        internal Bullet turretRotL()
        {
            rotTurretAngleL--;
            rotTurretAngleL %= (float)Math.PI;
            Vector2 veldir = new Vector2((float)Math.Cos(rotTurretAngleL), (float)Math.Sin(rotTurretAngleL));

            return new Bullet(pos, veldir * 4, Vector2.Zero, 0f, new Sprite(Game1.PropSheet, PropSheet.MISSLE));
        }
    }
}