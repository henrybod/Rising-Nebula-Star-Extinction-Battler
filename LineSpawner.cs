using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;

namespace teamstairwell
{
    class LineSpawner : BulletSpawner
    {
        int numB;
        public LineSpawner(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, float rotation, float rotationVelocity, List<Bullet> bossBullets, Boolean retract, int bulletNumber, int numberofshots, double rateoffire)
            : base(position, velocity, acceleration, radius, rotation, rotationVelocity, bossBullets, retract, numberofshots, rateoffire)
        {
            numB = bulletNumber;

            normalSprite = new Sprite(Game1.PropSheet, PropSheet.HORIZON);
            selectSprite = new Sprite(Game1.PropSheet, PropSheet.HORIZONAURA);
            pic = normalSprite;
        }

        public override void SpawnB()
        {
            int lineLength = 70 + 20 * power;
            Vector2 dir = new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot));
            Vector2 offset = lineLength / numB * dir;
            Vector2 bAcc = Vector2.Zero;
            if (doRetract)
            {
                for (int i = 1; i <= numB; i++)
                {
                    Vector2 a = new Vector2(-dir.Y, dir.X) * (float)Math.Pow(-1, i);
                    Vector2 b = new Vector2(-dir.Y, dir.X) * (float)Math.Pow(-1, i + 1);
                    Bullet bu, bh;
                    if(Game1.GUMBALLMODE)
                    {
                        bu = new Bullet(pos + offset * i, a * 2, a * -.01f, 7.5f, new Sprite(Game1.PropSheet, PropSheet.GUM1));
                        bh = new Bullet(pos - offset * i, b * 2, b * -.01f, 7.5f, new Sprite(Game1.PropSheet, PropSheet.GUM1));
                    }
                    else
                    {
                        bu = new Bullet(pos + offset * i, a * 2, a * -.01f, 7.5f, new Sprite(Game1.PropSheet, PropSheet.GUM0));
                        bh = new Bullet(pos - offset * i, b * 2, b * -.01f, 7.5f, new Sprite(Game1.PropSheet, PropSheet.GUM0));
                    }
                    bBs.Add(bu);
                    bBs.Add(bh);
                }
            }
            else{
                for (int i = 1; i <= numB; i++)
                {
                    Vector2 a = new Vector2(-dir.Y, dir.X) * (float)Math.Pow(-1, i);
                    Vector2 b = new Vector2(-dir.Y, dir.X) * (float)Math.Pow(-1, i + 1);
                    Bullet bu, bh;
                    if(Game1.GUMBALLMODE)
                    {
                        bu = new Bullet(pos + offset * i, Vector2.Zero, a * .05f, 7.5f, new Sprite(Game1.PropSheet, PropSheet.GUM1));
                        bh = new Bullet(pos - offset * i, Vector2.Zero, b * .05f, 7.5f, new Sprite(Game1.PropSheet, PropSheet.GUM1));
                    }
                    else
                    {
                        bu = new Bullet(pos + offset * i, Vector2.Zero, a * .05f, 7.5f, new Sprite(Game1.PropSheet, PropSheet.GUM0));
                        bh = new Bullet(pos - offset * i, Vector2.Zero, b * .05f,  7.5f, new Sprite(Game1.PropSheet, PropSheet.GUM0));
                    }
                    bBs.Add(bu);
                    bBs.Add(bh);
                }
                }
            //bBs.Add(new Bullet(pos, new Vector2(2 * (float)Math.Cos(rot), 2 * (float)Math.Sin(rot)), Vector2.Zero, 7.5f, new Sprite(Game1.PropSheet, PropSheet.GUM1)));
            
        }
    }
}
