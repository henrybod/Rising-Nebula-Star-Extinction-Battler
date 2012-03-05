using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell
{
    public class Drone : Entity
    {

        //Specifies the movement pattern of the drone
        public enum droneType
        {
            Fixed,
            Orbital,
            SideBySide
        }

        public int health;

        List<Bullet> dBs;           //Drone bullet list
        Entity master;              //Reference to controlling unit
        droneType type;

        Sprite normalSprite;

        public Drone(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, droneType type, Entity master)
            : base(position, velocity, acceleration, radius)
        {
            this.type = type;
            this.master = master;
            dBs = new List<Bullet>();

            normalSprite = new Sprite(RNSEB.PlayerSheet, PlayerSheet.PNORMAL);
        }

        public override void update(GameTime time)
        {
            if (type == droneType.SideBySide)
            {
                rot = master.rot;
                pos.X = master.pos.X + (float)Math.Cos(rot) * 50;
                pos.Y = master.pos.Y + (float)Math.Sin(rot) * 50;
            }
            if (type == droneType.Orbital)
            {
                rot += (float)(time.ElapsedGameTime.Milliseconds) / 1000;
                pos.X = master.pos.X + (float)Math.Cos(rot) * 50;
                pos.Y = master.pos.Y + (float)Math.Sin(rot) * 50;
            }

            base.update(time);
        }
    }
}
