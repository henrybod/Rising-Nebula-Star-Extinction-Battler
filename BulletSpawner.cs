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
    class BulletSpawner : Entity
    {
        const int maxQueue = 5;

        List<Sprite> flagSprites;
        //Sprite startRot, endRot;
        List<Vector2> queuePoints;
        //public Vector2 rotStartPoint, rotEndPoint;
        internal float preRot;
        internal int power;

        public Sprite normalSprite, selectSprite;

        internal Boolean doRetract;
        internal List<Bullet> bBs;
        internal Boolean active;
        double timer;
        internal int shots;
        internal double shotrate;

        public BulletSpawner(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, float rotation, float rotationVelocity, List<Bullet> bossBullets, Boolean retract, int numberofshots, double rateoffire)
            : base(position, velocity, acceleration, radius)
        {
            shotrate = rateoffire;
            active = false;
            rot = rotation;
            doRetract = retract;
            rotvel = rotationVelocity;
            bBs = bossBullets;
            timer = 0;
            shots = numberofshots;

            normalSprite = new Sprite(RNSEB.PropSheet, PropSheet.LINE);
            selectSprite = new Sprite(RNSEB.PropSheet, PropSheet.LINEAURA);
            pic = normalSprite;

            queuePoints = new List<Vector2>();
            flagSprites = new List<Sprite>();
            for (int f = 0; f < maxQueue; f++)
            {
                flagSprites.Add(new Sprite(RNSEB.PropSheet, PropSheet.BOUY));
            }
            //startRot = new Sprite(Game1.PrimitiveSheet, PrimitiveSheet.FLAG);
            //endRot = new Sprite(Game1.PrimitiveSheet, PrimitiveSheet.FLAG);
        }

        public virtual void SpawnB()
        {
            float vx = (float)Math.Cos(rot);
            float vy = (float)Math.Sin(rot);

            float ax = 0;
            float ay = 0;
            if (doRetract)
            {
                ax = -.005f * vx;
                ay = -.005f * vy;
            }
            if (RNSEB.GUMBALLMODE)
            {
                bBs.Add(new Bullet(pos, new Vector2(1f + power * 3 / 10 * vx, .5f + power * 3 / 10 * vy), new Vector2(ax, ay), 7.5f, new Sprite(RNSEB.PropSheet, PropSheet.GUM1)));
            }
            else
            {
                bBs.Add(new Bullet(pos, new Vector2(1f + power * 3 / 10 * vx, .5f + power * 3 / 10 * vy), new Vector2(ax, ay), 7.5f, new Sprite(RNSEB.PropSheet, PropSheet.BULLETC)));
            }
            //lst.Add(new Bullet(pos, new Vector2(0, 2), Vector2.Zero, 7.5f, new Sprite(Game1.PropSheet, PropSheet.THICKLINK)));
        }

        public void removeFirstQueue()
        {
            if (queuePoints.Count > 0)
            {
                queuePoints.RemoveAt(0);
            }
        }

        public void addQueuePoint(Vector2 point)
        {
            if (queuePoints.Count < maxQueue)
            {
                queuePoints.Add(point);
            }
        }

        public void drawQueuePoints(SpriteBatch sb)
        {
            for (int f = 0; f < queuePoints.Count; f++)
            {
                flagSprites[f].Position = new Vector3(queuePoints[f].X, queuePoints[f].Y, 0);
                flagSprites[f].Draw(sb);
            }
            //startRot.Position = new Vector3(rotStartPoint.X, rotStartPoint.Y, 0);
            //endRot.Position = new Vector3(rotEndPoint.X, rotEndPoint.Y, 0);
        }

        public void incPower()
        {
            power++;
            if (power > 9)
            {
                power = 9;
            }

            //shotrate = 5.5 - power / 2;
        }

        public new void remove()
        {
            bBs.Add(new Explosion(pos, Vector2.Zero, Vector2.Zero, 20, new Sprite(RNSEB.PropSheet, PropSheet.SOURCEEXPLOSION)));
            base.remove();
        }

        public override void update(GameTime gt)
        {
            timer -= gt.ElapsedGameTime.TotalSeconds;

            if (timer <= 0 && active && shots > 0)
            {
                shots--;
                SpawnB();
                timer = shotrate - shotrate * power / 20;
            }
            if (shots == 0)
            {
                remove();
            }

            if (active && queuePoints.Count > 0)
            {
                Vector2 distance = queuePoints[0] - pos;
                Vector2 direction = distance;
                if (distance.Length() < hitrad)
                {
                    removeFirstQueue();
                }
                else
                {
                    direction.Normalize();
                    acc = .05f * direction;

                    //vel += .25f * direction;

                    Vector3 towardVel = GameEnvironment.Projection(new Vector3(distance.X, distance.Y, 0), new Vector3(vel.X, vel.Y, 0));
                    Vector2 awayVel = vel - new Vector2(towardVel.X, towardVel.Y);
                    vel -= .2f * awayVel;
                }
            }

            pic.CurrentFrame = power / 2;

            base.update(gt);
        }
    }
}
