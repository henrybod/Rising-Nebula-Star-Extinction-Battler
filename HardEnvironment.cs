using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;
using Microsoft.Xna.Framework.Input;

namespace teamstairwell
{
    public class HardEnvironment
    {
        TimeSpan powerUpRate = new TimeSpan(0, 0, 0, 10, 500);
        TimeSpan powerUpTime = TimeSpan.Zero;

        const float playerHitRadius = 3;
        const float bossHitRadius = 65;

        static Vector2 playerSpawn = new Vector2(0, 200);
        static Vector2 bossSpawn = new Vector2(0, -300);

        Player p;
        Boss b;
        //List<Bullet> boo;

        Sprite NumOfBombs;
        Sprite NumOfLives;
        Sprite PauseScreen;

        TextSprite textBombs;
        TextSprite textLives;

        int numBombs;
        int numlives;

        public bool isPaused, pKey, isOver;
        public bool playerDeath, bossDeath;
        double gameOverTimer;
        double hurtTimer, powerTimer;
        bool gumFlag;

        List<Bullet> playerBullets, bossBullets;
        List<BulletSpawner> bulletSpawners;

        Random r;

        public HardEnvironment()
        {
            playerBullets = new List<Bullet>();
            bossBullets = new List<Bullet>();
            bulletSpawners = new List<BulletSpawner>();

            //bulletSpawners.Add(new LineSpawner(new Vector2(-200, -200), new Vector2(1, 0), Vector2.Zero, 200f, 0f, 0f, bossBullets, true, 10));
            //bulletSpawners.Add(new BurstSpawner(new Vector2(), Vector2.Zero, Vector2.Zero, 50f, 0f, .01f, bossBullets, true, 20));

            NumOfBombs = new Sprite(RNSEB.PropSheet, PropSheet.BOMBGET);
            NumOfBombs.Position = new Vector3(RNSEB.RESOLUTION.X / 2 - 25, RNSEB.RESOLUTION.Y / 2 - 25, 0);
            NumOfLives = new Sprite(RNSEB.PropSheet, PropSheet.ONEUP);
            NumOfLives.Position = new Vector3(RNSEB.RESOLUTION.X / 2 - 24, RNSEB.RESOLUTION.Y / 2 - 70, 0);
            NumOfLives.Scale = .70f;
            //BossHPBar = new Sprite(Game1.PanelSheet, PanelSheet.BUTTONFRAME);
            //BossHPBar.Position = new Vector3(Game1.RESOLUTION.X / 2 - 1000, Game1.RESOLUTION.Y / 2 - 700, 0);
            /*boo = new List<Bullet>();
            for (int n = 0; n < 10; n++)
            {
                boo.Add(new Bullet(Vector2.Zero, Vector2.Zero, Vector2.Zero, 0f, new Sprite(Game1.PropSheet, PropSheet.MISSLE)));
            }*/


            p = new Player(playerSpawn, Vector2.Zero, Vector2.Zero, playerHitRadius, playerBullets, 5, 5);
            b = new Boss(bossSpawn, new Vector2(5, 0), Vector2.Zero, bossHitRadius, bulletSpawners, bossBullets, p);
            //rad = new Bullet(Vector2.Zero, Vector2.Zero, Vector2.Zero, 0f, BossHPBar);


            numBombs = p.bomb;
            numlives = p.live;
            PauseScreen = new Sprite(RNSEB.PanelSheet, PanelSheet.REDBAR);
            PauseScreen.Position = new Vector3(RNSEB.RESOLUTION.X / 2 - 600, RNSEB.RESOLUTION.Y / 2 - 400, 0);


            textBombs = new TextSprite(numBombs + "x");
            textBombs.Color = Color.WhiteSmoke;
            textLives = new TextSprite(numlives + "x");
            textLives.Color = Color.WhiteSmoke;

            powerTimer = .5;

            isPaused = false;

            r = new Random();
        }

        public void update(GameTime gt)
        {
            if (!gumFlag && RNSEB.GUMBALLMODE)
            {
                b.normalSprite = new Sprite(RNSEB.PlayerSheet, PlayerSheet.FACE);
                //b.normalSprite.FrameRate = 10;
                b.damageSprite = new Sprite(RNSEB.PlayerSheet, PlayerSheet.OLDFACE);
                b.damageSprite.FrameRate = 20f;
                b.boomSprite = new Sprite(RNSEB.PlayerSheet, PlayerSheet.OLDFACE);
                gumFlag = true;
            }

            KeyboardState kbs = Keyboard.GetState();
            if (kbs.IsKeyDown(Keys.P) && !pKey)
            {
                isPaused = !isPaused;
                pKey = true;
            }
            if (kbs.IsKeyUp(Keys.P))
            {
                pKey = false;
            }

            if (!isPaused)
            {
                if(!(p.removing && p.removeTime <= 0))
                {
                    p.update(gt);
                }
                if (!(b.removing && b.removeTime <= 0))
                {
                    b.update(gt);
                }

                if (p.pos.Y < -150.0f)
                {
                    b.fireDeathRay(p.pos);
                }

                /*rad.pos.X = b.pos.X;
                rad.pos.Y = b.pos.Y + b.activeRad;
                rad.update(gt);*/
                /*for (int n = 0; n < 10; n++)
                {
                    boo[n].update(gt);
                    if (n % 2 == 0)
                    {
                        boo[n].pos = b.pos + new Vector2(-125, 0) + b.activeRad * new Vector2((float)Math.Cos(Math.PI * n / 9), (float)Math.Sin(Math.PI * n / 9));
                    }
                    else
                    {
                        boo[n].pos = b.pos + new Vector2(125, 0) + b.activeRad * new Vector2((float)Math.Cos(Math.PI * n / 9), (float)Math.Sin(Math.PI * n / 9));
                    }
                }*/

                spawnpup(gt);

                numBombs = p.bomb;
                numlives = p.live;

                textBombs.Text = numBombs + "x";
                textBombs.Position = new Vector3(RNSEB.RESOLUTION.X / 2 - 40, RNSEB.RESOLUTION.Y / 2 - 25, 0);
                textBombs.Update(gt);
                textLives.Text = numlives + "x";
                textLives.Position = new Vector3(RNSEB.RESOLUTION.X / 2 - 48, RNSEB.RESOLUTION.Y / 2 - 67, 0);
                textLives.Update(gt);

                if (powerTimer > 0)
                {
                    powerTimer -= gt.ElapsedGameTime.TotalSeconds;
                    if (powerTimer <= 0)
                    {
                        powerTimer = .5;
                        for (int n = 0; n < bulletSpawners.Count; n++)
                        {
                            bulletSpawners[n].incPower();
                        }
                    }
                }
                for (int n = 0; n < bulletSpawners.Count; n++)
                {
                    bulletSpawners[n].update(gt);
                    if (bulletSpawners[n].power < 1 && !(bulletSpawners[n] is AimSpawner))
                    {
                        bulletSpawners[n].rotvel = (float)(r.NextDouble() - .5) * .25f;
                    }
                }
                for (int n = 0; n < bossBullets.Count; n++)
                {
                    bossBullets[n].update(gt);
                }
                for (int n = 0; n < playerBullets.Count; n++)
                {
                    if (playerBullets[n] is Seeker)
                    {
                        Seeker mySeek;
                        mySeek = (Seeker)playerBullets[n];
                        mySeek.lockOn(b.pos);
                    }
                    playerBullets[n].update(gt);
                    if (playerBullets[n] is Bomb)
                    {
                        Bomb.destroy(bossBullets, (Bomb)playerBullets[n], p);
                    }
                }

                checkCollision(gt);

                clearRemoves();

                if (p.live <= 0 || b.health <= 0)
                {
                    if (!playerDeath && p.live <= 0)
                    {
                        playerDeath = true;
                        RNSEB.SoundEffs["MissleSound"].Play();
                        gameOverTimer = 5;
                    }

                    if (!bossDeath && b.health <= 0)
                    {
                        bossDeath = true;
                        RNSEB.SoundEffs["MissleSound"].Play();
                        gameOverTimer = 5;
                    }

                    if (gameOverTimer > 0)
                    {
                        gameOverTimer -= gt.ElapsedGameTime.TotalSeconds;
                        if (gameOverTimer <= 0)
                        {
                            gameOverTimer = 0;
                        }
                    }

                    if (gameOverTimer <= 0)
                    {
                        isOver = true;
                    }
                }
            }
        }

        public void draw(SpriteBatch sb)
        {
            if (isPaused == true)
            {
                PauseScreen.Draw(sb);
            }
            else
            {
                for (int n = 0; n < playerBullets.Count; n++)
                {
                    playerBullets[n].draw(sb);
                }
                for (int n = 0; n < bossBullets.Count; n++)
                {
                    bossBullets[n].draw(sb);
                }
                for (int n = 0; n < bulletSpawners.Count; n++)
                {
                    bulletSpawners[n].draw(sb);
                }

                if (textBombs != null)
                    textBombs.Draw(sb);
                if (textLives != null)
                    textLives.Draw(sb);
                //BossHPBar.Draw(sb);
                if (NumOfBombs != null)
                    NumOfBombs.Draw(sb);
                if (NumOfBombs != null)
                    NumOfLives.Draw(sb);

                //rad.draw(sb);


                if (!(p.removing && p.removeTime <= 0))
                {
                    p.draw(sb);
                }
                if (!(b.removing && b.removeTime <= 0))
                {
                    b.draw(sb);
                }

                /*for (int n = 0; n < 10; n++)
                {
                    boo[n].draw(sb);
                }*/
            }
        }

        public void spawnpup(GameTime gt)
        {
            if (gt.TotalGameTime - powerUpTime > powerUpRate)
            {
                Random r = new Random();
                int broch = r.Next(3);
                Vector2 posi = new Vector2(0, -200);
                if (broch == 0)
                {
                    posi.X = -200;
                }
                else if (broch == 1)
                {
                    posi.X = 0;
                }
                else
                {
                    posi.X = 200;
                }

                String type;
                Sprite spr;
                int blog = r.Next(8);
                if (blog >= 7)
                {
                    type = "rotTurretR";
                    spr = new Sprite(RNSEB.PropSheet, PropSheet.D);
                }
                else if (blog >= 6)
                {
                    type = "angleRocket";
                    spr = new Sprite(RNSEB.PropSheet, PropSheet.ROCKET);
                }
                else if (blog >= 5)
                {
                    type = "angleGun";
                    spr = new Sprite(RNSEB.PropSheet, PropSheet.G);
                }
                else if (blog >= 4)
                {
                    type = "bomb";
                    spr = new Sprite(RNSEB.PropSheet, PropSheet.BOMBGET);
                }
                else if (blog >= 3)
                {
                    type = "life";
                    spr = new Sprite(RNSEB.PropSheet, PropSheet.ONEUP);
                }
                else if (blog >= 2)
                {
                    type = "gun";
                    spr = new Sprite(RNSEB.PropSheet, PropSheet.K);
                }
                else if (blog >= 1)
                {
                    type = "surge";
                    spr = new Sprite(RNSEB.PropSheet, PropSheet.P);
                }
                else
                {
                    type = "rotTurretL";
                    spr = new Sprite(RNSEB.PropSheet, PropSheet.M);
                }

                bossBullets.Add(new Powerup(posi, new Vector2(0, 3), Vector2.Zero, 20f, spr, type));
                powerUpTime = gt.TotalGameTime;
            }
        }

        public void checkCollision(GameTime gt)
        {
            for (int n = 0; n < playerBullets.Count; n++)
            {
                if (entityOffScreen(playerBullets[n]))
                {
                    playerBullets[n].remove();
                }
                else if (Entity.collideFuckMAtt(playerBullets[n], b))
                {
                    if (!(playerBullets[n] is Bomb))
                    {
                        playerBullets[n].remove();
                    }

                    b.health--;

                    if (b.health <= 0)
                    {
                        if (!b.removing)
                        {
                            b.death();
                        }
                    }
                    else
                    {
                        b.hurt();
                        if (hurtTimer > 0)
                        {
                            hurtTimer -= gt.ElapsedGameTime.TotalSeconds;
                            if (hurtTimer <= 0)
                            {
                                hurtTimer = 0;
                            }
                        }
                        if (hurtTimer == 0)
                        {
                            RNSEB.SoundEffs["MegaDead"].Play();
                            hurtTimer = 1;
                        }

                    }
                }
            }
            for (int n = 0; n < bossBullets.Count; n++)
            {
                if (entityOffScreen(bossBullets[n]))
                {
                    bossBullets[n].remove();
                }
                else if (Entity.collideFuckMAtt(bossBullets[n], p))
                {

                    if (bossBullets[n] is Powerup)
                    {
                        ((Powerup)bossBullets[n]).getUp(p, gt);
                    }
                    else if (!p.invisible)
                    {
                        p.live--;

                        p.surge = false;
                        p.angleGun = false;
                        p.angleRocket = false;
                        p.rotTurretL = false;
                        p.rotTurretR = false;

                        if (p.live <= 0)
                        {
                            if (!p.removing)
                            {
                                p.death();
                            }
                        }
                        else
                        {
                            p.invisible = true;
                            if (hurtTimer > 0)
                            {
                                hurtTimer -= gt.ElapsedGameTime.TotalSeconds;
                                if (hurtTimer <= 0)
                                {
                                    hurtTimer = 0;
                                }
                            }
                            if (hurtTimer == 0)
                            {
                                RNSEB.SoundEffs["MegaDead"].Play();
                                hurtTimer = 0;
                            }

                            p.invisibleTime = gt.TotalGameTime;
                        }
                    }
                    bossBullets[n].remove();
                }
            }
            for (int n = 0; n < bulletSpawners.Count; n++)
            {
                if (entityOffScreen(bulletSpawners[n]))
                {
                    bulletSpawners[n].remove();
                }
            }
            if (Entity.collideFuckMAtt(p, b))
            {
                if (!p.invisible)
                {
                    p.live--;

                    p.surge = false;
                    p.angleGun = false;
                    p.angleRocket = false;
                    p.rotTurretL = false;
                    p.rotTurretR = false;

                    if (p.live <= 0)
                    {
                        if (!p.removing)
                        {
                            p.death();
                        }
                    }
                    else
                    {
                        p.invisible = true;
                        if (hurtTimer > 0)
                        {
                            hurtTimer -= gt.ElapsedGameTime.TotalSeconds;
                            if (hurtTimer <= 0)
                            {
                                hurtTimer = 0;
                            }
                        }
                        if (hurtTimer == 0)
                        {
                            RNSEB.SoundEffs["MegaDead"].Play();
                            hurtTimer = 0;
                        }

                        p.invisibleTime = gt.TotalGameTime;
                    }
                }
            }
        }

        public static bool entityOffScreen(Entity e)
        {
            if (e.pos.X + e.hitrad < -RNSEB.RESOLUTION.X / 2
                || e.pos.X - e.hitrad > RNSEB.RESOLUTION.X / 2
                || e.pos.Y + e.hitrad < -RNSEB.RESOLUTION.Y / 2
                || e.pos.Y - e.hitrad > RNSEB.RESOLUTION.Y / 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void clearRemoves()
        {
            for (int n = 0; n < bulletSpawners.Count; n++)
            {
                if (bulletSpawners[n].removing == true && bulletSpawners[n].removeTime == 0)
                {
                    bulletSpawners.RemoveAt(n);
                    n--;
                }
            }
            for (int n = 0; n < bossBullets.Count; n++)
            {
                if (bossBullets[n].removing == true && bossBullets[n].removeTime == 0)
                {
                    bossBullets.RemoveAt(n);
                    n--;
                }
            }
            for (int n = 0; n < playerBullets.Count; n++)
            {
                if (playerBullets[n].removing == true && playerBullets[n].removeTime == 0)
                {
                    playerBullets.RemoveAt(n);
                    n--;
                }
            }
        }
        public static Vector3 Projection(Vector3 Canvas, Vector3 Light)
        {
            float Length = Vector3.Dot(Light, Canvas) / Canvas.Length();
            Vector3 DirectionVector = Canvas / Canvas.Length();
            Vector3 Shadow = Length * DirectionVector;
            return Shadow;
        }
    }
}
