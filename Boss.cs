using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell
{
    class Boss : Entity
    {
        bool leftMD, rightMD;
        int curWheel, prevWheel;

        double dTimer;

        Sprite selectTarget;
        //Sprite startRot, endRot;
        //bool rotDrag;
        public Sprite normalSprite, damageSprite, boomSprite;
        Sprite leftSphere, rightSphere;

        Player pla;

        List<BulletSpawner> spawnlst;
        List<Bullet> bulletlst;
        BulletSpawner selectedSpawner;
        TimeSpan spawnRate = new TimeSpan(0, 0, 0, 1, 500);
        TimeSpan spawnTime = TimeSpan.Zero;
        public int health = 5000;
        public int activeRad = 260;
        Sprite hbartick;

        public Boss(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, List<BulletSpawner> spawnerList, List<Bullet> bulletList, Player player)
            : base(position, velocity, acceleration, radius)
        {
            if (Game1.GUMBALLMODE)
            {
                normalSprite = new Sprite(Game1.PlayerSheet, PlayerSheet.FACE);
                damageSprite = new Sprite(Game1.PlayerSheet, PlayerSheet.OLDFACE);
                damageSprite.FrameRate = 20f;
                boomSprite = new Sprite(Game1.PlayerSheet, PlayerSheet.OLDFACE);
            }
            else
            {
                normalSprite = new Sprite(Game1.PlayerSheet, PlayerSheet.BOSSNORMAL);
                damageSprite = new Sprite(Game1.PlayerSheet, PlayerSheet.BOSSHIT);
                damageSprite.FrameRate = 20f;
                boomSprite = new Sprite(Game1.PlayerSheet, PlayerSheet.BOSSBOOM);
            }
            
            hbartick = new Sprite(Game1.PropSheet, PropSheet.HEALTHTICK);
            pic = normalSprite;

            leftSphere = new Sprite(Game1.PlayerSheet, PlayerSheet.BENERGYSPHERE);
            leftSphere.Position = new Vector3(pos.X - 125, pos.Y, 0);
            rightSphere = new Sprite(Game1.PlayerSheet, PlayerSheet.BENERGYSPHERE);
            rightSphere.Position = new Vector3(pos.X + 125, pos.Y, 0);

            
            selectTarget = new Sprite(Game1.PropSheet, PropSheet.TARGET);
            //startRot = new Sprite(Game1.PrimitiveSheet, PrimitiveSheet.FLAG);
            //endRot = new Sprite(Game1.PrimitiveSheet, PrimitiveSheet.FLAG);

            pla = player;

            spawnlst = spawnerList;
            bulletlst = bulletList;

            curWheel = prevWheel = Mouse.GetState().ScrollWheelValue;
        }

        public override void update(GameTime gt)
        {
            // we're fighting our way through a gumball machine

            //Console.WriteLine("BOSS" + pos);
            acc.X = -.001f * pos.X / 9;

            spawn(gt);

            activate();

            processMouseInput();

            if (!removing)
            {
                if (dTimer > 0)
                {
                    dTimer -= gt.ElapsedGameTime.TotalSeconds;
                    if (dTimer <= 0)
                    {
                        dTimer = 0;
                        pic = normalSprite;
                    }
                }
            }

            
            if (selectedSpawner != null && !selectedSpawner.removing)
            {
                selectTarget.Update(gt);
                selectTarget.Position = new Vector3(selectedSpawner.pos.X, selectedSpawner.pos.Y, 0);
                //startRot.Update(gt);
                //startRot.Position = new Vector3(selectedSpawner.rotStartPoint.X + selectedSpawner.pos.X, selectedSpawner.rotStartPoint.Y + selectedSpawner.pos.Y, 0);
                //endRot.Update(gt);
                //endRot.Position = new Vector3(selectedSpawner.rotEndPoint.X + selectedSpawner.pos.X, selectedSpawner.rotEndPoint.Y + selectedSpawner.pos.Y, 0);
            }
            
            leftSphere.Update(gt);
            rightSphere.Update(gt);

            base.update(gt);

            leftSphere.Position = new Vector3(pos.X - 125, pos.Y, 0);
            rightSphere.Position = new Vector3(pos.X + 125, pos.Y, 0);
        }

        public void hurt()
        {
            dTimer = .15f;
            pic = damageSprite;
            pic.Position = damageSprite.Position = normalSprite.Position = new Vector3(pos.X, pos.Y, 0);
        }

        private void activate()
        {
            foreach (BulletSpawner bs in spawnlst)
            {
                Vector2 lpos = pos;
                lpos.X -= 125;
                Vector2 rpos = pos;
                rpos.X += 125;
                if (Vector2.Distance(lpos, bs.pos) > activeRad && Vector2.Distance(rpos, bs.pos) > activeRad && !bs.active)
                {
                    bs.active = true;
                    bs.shots = (bs.power + 2) / 2 * bs.shots;
                }
            }
        }

        public new void draw(SpriteBatch sb)
        {
            leftSphere.Draw(sb);
            rightSphere.Draw(sb);

            for (int i = 0; i < health / 5; i += 2)
            {
                hbartick.Position = new Vector3(i - 600, -370, 0);
                hbartick.Draw(sb);
            }

            base.draw(sb);

            if (selectedSpawner != null && !selectedSpawner.removing)
            {
                selectTarget.Draw(sb);
                selectedSpawner.drawQueuePoints(sb);

                /*if (rotDrag)
                {
                    startRot.Draw(sb);
                    endRot.Draw(sb);
                }*/
            }
        }

        public void processMouseInput()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mP = new Vector2(mouseState.X, mouseState.Y) - Game1.RESOLUTION / 2;
            curWheel = mouseState.ScrollWheelValue;

            BulletSpawner wheelProbe = mouseOverSpawner(mP);
            if (wheelProbe != null)
            {
                if (curWheel > prevWheel)
                {
                    wheelProbe.rotvel += .01f;
                }
                else if (curWheel < prevWheel)
                {
                    wheelProbe.rotvel -= .01f;
                }
            }

            /*if (rotDrag && selectedSpawner != null)
            {
                selectedSpawner.rotEndPoint = mP - selectedSpawner.pos;
            }*/
            BulletSpawner probe = mouseOverSpawner(mP);
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (!leftMD)
                {
                    leftMD = true;

                    if (probe != null)
                    {
                        selectedSpawner = probe;
                        //selectedSpawner.pic = selectedSpawner.selectSprite;
                        selectedSpawner.incPower();
                        foreach (BulletSpawner bs in spawnlst)
                        {
                            if (bs != probe)
                            {
                                bs.pic = bs.normalSprite;
                            }
                        }
                        //rotDrag = true;
                        //selectedSpawner.rotStartPoint = mP - selectedSpawner.pos;
                    }
                    /*else
                    {
                        if (selectedSpawner != null)
                        {
                            selectedSpawner.addQueuePoint(mP);
                        }
                    }*/
                }
            }
            else
            {
                leftMD = false;
            }

            if (mouseState.RightButton == ButtonState.Pressed && !rightMD)
            {
                rightMD = true;
                if (selectedSpawner != null)
                {
                    selectedSpawner.addQueuePoint(mP);
                }
            }
            if (mouseState.RightButton == ButtonState.Released)
            {
                rightMD = false;
            }

            prevWheel = curWheel;
        }

        public BulletSpawner mouseOverSpawner(Vector2 mP)
        {
            for (int b = 0; b < spawnlst.Count; b++)
            {
                if ((mP - spawnlst[b].pos).Length() < spawnlst[b].hitrad)
                {
                    return spawnlst[b];
                }
            }
            return null;
        }

        public void spawn(GameTime gt)
        {
            if (gt.TotalGameTime - spawnTime > spawnRate)
            {
                Random r = new Random();
                int chooser = r.Next(7);
                Vector2 spawnPos = pos;
                spawnPos.Y += 35;
                float velmag = .5f;
                float radius = 35;
                Vector2 veldir = getVelDir();
                if (vel.X > 0)
                {
                    spawnPos.X += 50;
                }
                else
                {
                    spawnPos.X -= 50;
                }
                //chooser = 5;
                switch (chooser)
                {
                    case 0:
                        spawnlst.Add(new BulletSpawner(spawnPos, veldir * velmag, Vector2.Zero, radius, 0f, 0f, bulletlst, r.Next(2) < 1, 50, .3));
                        break;
                    case 1:
                        spawnlst.Add(new BurstSpawner(spawnPos, veldir * 1f, Vector2.Zero, radius, 0f, 0f, bulletlst, r.Next(2) < 1, 15, 2, 3));
                        break;
                    case 2:
                        spawnlst.Add(new LineSpawner(spawnPos, veldir * velmag, Vector2.Zero, radius, 0f, 0f, bulletlst, r.Next(2) < 1, 20, 2, 5));
                        break;
                    case 3:
                        spawnlst.Add(new AimSpawner(spawnPos, veldir * velmag, Vector2.Zero, radius, 0f, 0f, bulletlst, r.Next(2) < 1, 20, .25, pla));
                        break;
                    case 4:
                        spawnlst.Add(new GravitySpawner(spawnPos, veldir * velmag, new Vector2(0, .01f), radius, 0f, 0f, bulletlst, r.Next(2) < 1, 20, .25));
                        break;
                    case 5:
                        spawnlst.Add(new RollSpawner(spawnPos, veldir * velmag, new Vector2(0, .01f), radius, 0f, 0f, bulletlst, r.Next(2) < 1, 2, 5, pla));
                        break;
                    case 6:
                        spawnlst.Add(new SplitSpawner(spawnPos, veldir * .5f * velmag, new Vector2(0, .01f), radius, 0f, 0f, bulletlst, r.Next(2) < 1, 5, 2));
                        break;
                }

                //lst.Add(new BulletSpawner(pos, new Vector2(0, 5), Vector2.Zero, 0f, new Sprite(Game1.PlayerSheet, PlayerSheet.GROUNDIDLE), 0f, 0f));
                spawnTime = gt.TotalGameTime;
            }

        }

        private Vector2 getVelDir()
        {
            Random r = new Random();
            float angle = (float)r.NextDouble() * (float)Math.PI * 4 / 6 + (float)Math.PI / 6;
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public void fireDeathRay(Vector2 playerPos)
        {
            Vector2 atPlayer = (new Vector2(playerPos.X - pos.X, playerPos.Y - pos.Y));
            atPlayer.Normalize();
            bulletlst.Add(new Bullet(pos, 2 * atPlayer, Vector2.Zero, 7.5f, new Sprite(Game1.PropSheet, PropSheet.GUM0)));
        }

        internal void death()
        {
            boomSprite.Position = normalSprite.Position;
            pic = boomSprite;
            base.remove(1);
        }
    }
}
