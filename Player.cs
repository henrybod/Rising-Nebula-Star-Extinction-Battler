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
using System.Diagnostics;
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
        MouseState oldmouse = Mouse.GetState();

        public bool fury;
        public bool surge;
        public bool tracker;
        public bool angleGun;
        public bool angleRocket;
        public bool rotTurretR;
        float rotTurretAngleR;
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

            normalSprite = new Sprite(RNSEB.PlayerSheet, PlayerSheet.PNORMAL);
            shieldSprite = new Sprite(RNSEB.PlayerSheet, PlayerSheet.PSHIELD);
            boomSprite = new Sprite(RNSEB.PlayerSheet, PlayerSheet.PBOOM);
            bankLeftSprite = new Sprite(RNSEB.PlayerSheet, PlayerSheet.PGOLEFT);
            bankRightSprite = new Sprite(RNSEB.PlayerSheet, PlayerSheet.PGORIGHT);

            pic = normalSprite;
        }

        public void processMouseInput(GameTime time)
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mP = new Vector2(mouseState.X, mouseState.Y) - RNSEB.RESOLUTION / 2;

            //Calculate ship rotation from mouse cursor position
            float angle = (float)Math.Atan2(mP.Y - pos.Y, mP.X - pos.X);
            angle = angle + MathHelper.ToRadians(90);
            //rotTurretAngleR += angle - rot;
            //Debug.Print("Angle: {0}", angle - rot);
            rot = angle;
            //rot = (float)Math.Atan2(mP.Y - pos.Y, mP.X - pos.X);
            //rot = rot + MathHelper.ToRadians(90);

            //Left Mouse Click Fires Primary
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Bullet b = new Bullet(pos, new Vector2(-12, -12), new Vector2(0, 0), 0f, new Sprite(RNSEB.PropSheet, PropSheet.LASER), rot);
                pBs.Add(b);
                if (fury)
                {
                    Vector2 lpos = pos;
                    lpos.X += (float)Math.Cos(rot) * -7;
                    lpos.Y += (float)Math.Sin(rot) * -7;
                    Vector2 rpos = pos;
                    rpos.X += (float)Math.Cos(rot) * 7;
                    rpos.Y += (float)Math.Sin(rot) * 7;
                    pBs.Add(new Bullet(lpos, new Vector2(-12, -12), new Vector2(0, 0), 0f, new Sprite(RNSEB.PropSheet, PropSheet.LASER), rot - .08f));
                    pBs.Add(new Bullet(rpos, new Vector2(-12, -12), new Vector2(0, 0), 0f, new Sprite(RNSEB.PropSheet, PropSheet.LASER), rot + .08f));
                    lpos.X += (float)Math.Cos(rot) * -11;
                    lpos.Y += (float)Math.Sin(rot) * -11;
                    rpos.X += (float)Math.Cos(rot) * 11;
                    rpos.Y += (float)Math.Sin(rot) * 11;
                    pBs.Add(new Bullet(lpos, new Vector2(-12, -12), new Vector2(0, 0), 0f, new Sprite(RNSEB.PropSheet, PropSheet.LASER), rot - .16f));
                    pBs.Add(new Bullet(rpos, new Vector2(-12, -12), new Vector2(0, 0), 0f, new Sprite(RNSEB.PropSheet, PropSheet.LASER), rot + .16f));
                }
                if (surge)
                {
                    Vector2 lpos = pos;
                    lpos.X += (float)Math.Cos(rot) * -8;
                    lpos.Y += (float)Math.Sin(rot) * -8;
                    Vector2 rpos = pos;
                    rpos.X += (float)Math.Cos(rot) * 8;
                    rpos.Y += (float)Math.Sin(rot) * 8;
                    pBs.Add(new Bullet(lpos, new Vector2(-12, -12), new Vector2(0, 0), 0f, new Sprite(RNSEB.PropSheet, PropSheet.GOLDLASER), rot));
                    pBs.Add(new Bullet(rpos, new Vector2(-12, -12), new Vector2(0, 0), 0f, new Sprite(RNSEB.PropSheet, PropSheet.GOLDLASER), rot));
                }
                if (angleGun)
                {
                    Vector2 lpos = pos;
                    lpos.X += (float)Math.Cos(rot) * -11;
                    lpos.Y += (float)Math.Sin(rot) * -11;
                    Vector2 rpos = pos;
                    rpos.X += (float)Math.Cos(rot) * 11;
                    rpos.Y += (float)Math.Sin(rot) * 11;

                    Vector2 laccel = new Vector2(0, 0);
                    laccel.X = (float)Math.Sin(rot) * 2;
                    laccel.Y = (float)Math.Cos(rot) * -2;
                    //pBs.Add(new Bullet(lpos, new Vector2(-2, -6), new Vector2(0, -2), 0f, new Sprite(RNSEB.PropSheet, PropSheet.MISSLE)));
                    //pBs.Add(new Bullet(rpos, new Vector2(2, -6), new Vector2(0, -2), 0f, new Sprite(RNSEB.PropSheet, PropSheet.MISSLE)));
                    pBs.Add(new Bullet(lpos, new Vector2(-6, -6), laccel, 0f, new Sprite(RNSEB.PropSheet, PropSheet.MISSLE), rot - 0.31f));
                    pBs.Add(new Bullet(rpos, new Vector2(-6, -6), laccel, 0f, new Sprite(RNSEB.PropSheet, PropSheet.MISSLE), rot + 0.31f));
                }
                if (angleRocket)
                {
                    Vector2 lpos = pos;
                    lpos.X += (float)Math.Cos(rot) * -7;
                    lpos.Y += (float)Math.Sin(rot) * -7;
                    Vector2 rpos = pos;
                    rpos.X += (float)Math.Cos(rot) * 7;
                    rpos.Y += (float)Math.Sin(rot) * 7;

                    Vector2 laccel = new Vector2(0, 0);
                    laccel.X = (float)Math.Sin(rot) * 6;
                    laccel.Y = (float)Math.Cos(rot) * -6;

                    //pBs.Add(new Bullet(lpos, new Vector2(-15, 0), new Vector2(.1f, -6), 0f, new Sprite(RNSEB.PropSheet, PropSheet.ROCKET)));
                    //pBs.Add(new Bullet(rpos, new Vector2(15, 0), new Vector2(-.1f, -6), 0f, new Sprite(RNSEB.PropSheet, PropSheet.ROCKET)));
                    pBs.Add(new Bullet(lpos, new Vector2(-15, -15), laccel, 0f, new Sprite(RNSEB.PropSheet, PropSheet.ROCKET), rot - 1.5f, rot));
                    pBs.Add(new Bullet(rpos, new Vector2(-15, -15), laccel, 0f, new Sprite(RNSEB.PropSheet, PropSheet.ROCKET), rot + 1.5f, rot));
                }
                //rotTurretR = true;
                if (rotTurretR)
                {
                    pBs.Add(turretRotR());
                }
                rotTurretL = true;
                if (rotTurretL)
                {
                    pBs.Add(turretRotL());
                }
                //shoot
            }

            //Right Mouse Click triggers Bomb
            if (mouseState.RightButton == ButtonState.Pressed && oldmouse.RightButton == ButtonState.Released)
            {
                if (bomb > 0)
                {
                    int numbomb = 20;
                    for (int i = 0; i < numbomb; i++)
                    {
                        Vector2 v = new Vector2((float)Math.Cos(Math.PI * 2 * i / numbomb), (float)Math.Sin(Math.PI * 2 * i / numbomb));
                        pBs.Add(new Bomb(pos, v * 10, Vector2.Zero, 200f, new Sprite(RNSEB.DaBomb, ScreenBomb.SCREENBOMB)));
                    }
                    bomb--;
                    invisible = true;
                    invisibleTime = time.TotalGameTime;
                }
                //bomb
            }

            oldmouse = mouseState;
        }

        public override void update(GameTime time)
        {
            Console.WriteLine(pos);

            KeyboardState state = Keyboard.GetState();

            processMouseInput(time);

            if (state.IsKeyDown(Keys.W) && state.IsKeyUp(Keys.S))
            {
                vel.Y = -5;
            }
            else if (state.IsKeyDown(Keys.S) && state.IsKeyUp(Keys.W))
            {
                vel.Y = 5;
            }
            else
                vel.Y = 0;

            if (state.IsKeyDown(Keys.A) && state.IsKeyUp(Keys.D))
            {
                vel.X = -5;
                if (pic == normalSprite || pic == bankRightSprite)
                {
                    pic = bankLeftSprite;
                }
            }
            else if (state.IsKeyDown(Keys.D) && state.IsKeyUp(Keys.A))
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
            if (rotTurretAngleR > rot + Math.PI)
            {
                rotTurretAngleR += (float)Math.PI;
            }
            rotTurretAngleR++;
            rotTurretAngleR %= (float)Math.PI * 2;
            Vector2 veldir = new Vector2((float)Math.Cos(rotTurretAngleR - MathHelper.ToRadians(60)), (float)Math.Sin(rotTurretAngleR - MathHelper.ToRadians(60)));
            //Vector2 veldir = new Vector2((float)Math.Cos(rotTurretAngleR), (float)Math.Sin(rotTurretAngleR));
            /*rotTurretAngleR++;
            rotTurretAngleR %= (float)Math.PI;
            Vector2 veldir = new Vector2((float)Math.Cos(rotTurretAngleR + (float)Math.PI), (float)Math.Sin(rotTurretAngleR + (float)Math.PI));*/

            return new Bullet(pos, veldir * -4, Vector2.Zero, 0f, new Sprite(RNSEB.PropSheet, PropSheet.MISSLE));
        }

        internal Bullet turretRotL()
        {
            /*if (rotTurretAngleL < rot)
            {
                rotTurretAngleL -= (float)Math.PI;
            }
            rotTurretAngleL--;
            rotTurretAngleL %= (float)Math.PI * 2;
            Vector2 veldir = new Vector2((float)Math.Cos(rotTurretAngleL), (float)Math.Sin(rotTurretAngleL));*/
            rotTurretAngleL--;
            rotTurretAngleL %= (float)Math.PI;
            Vector2 veldir = new Vector2((float)Math.Cos(rotTurretAngleL), (float)Math.Sin(rotTurretAngleL));

            return new Bullet(pos, veldir * 4, Vector2.Zero, 0f, new Sprite(RNSEB.PropSheet, PropSheet.MISSLE));
        }
    }
}