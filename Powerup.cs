using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Hook.Graphics;

namespace teamstairwell
{
    class Powerup : Bullet
    {
        internal string type;

        public Powerup(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, Sprite sprite, string ty)
            : base(position, velocity, acceleration, radius, sprite)
        {
            type = ty;
        }

        public void getUp(Player player, GameTime gt)
        {
            if (type == "bomb")
            {
                player.bomb++;
            }
            else if (type == "life")
            {
                player.live++;
            }
            else if (type == "gun")
            {
                player.fury = true;
                player.furyTime = gt.TotalGameTime;
            }
            else if (type == "surge")
            {
                player.surge = true;
            }
            else if (type == "rotTurretL")
            {
                player.rotTurretL = true;
            }
            else if (type == "angleGun")
            {
                player.angleGun = true;
            }
            else if (type == "angleRocket")
            {
                player.angleRocket = true;
            }
            else if (type == "rotTurretR")
            {
                player.rotTurretR = true;
            }
        }
    }
}
