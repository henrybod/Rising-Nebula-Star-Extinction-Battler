using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell
{
    class Drone : HenrySpawner
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
        //Entity master;              //Reference to controlling unit
        HenrySpawner master;
        droneType type;

        public Drone(ContentManager cm, HenryBattlefield b, float mass, Vector2 initPos, Vector2 initVel, float damping, HenrySpawner master, droneType type)
            : base(cm, b, mass, initPos, initVel, damping)
        {
            LoadContent("PlayerIdle", true);
            this.type = type;
            this.master = master;
            dBs = new List<Bullet>();
            CenterOrigin();
        }
        /*
        public Drone(Vector2 position, Vector2 velocity, Vector2 acceleration, float radius, droneType type, Entity master, Sprite sprite)
            : base(position, velocity, acceleration, radius)
        {
            this.type = type;
            this.master = master;
            dBs = new List<Bullet>();
            pic = sprite;

            //normalSprite = sprite;
           //normalSprite = new Sprite(RNSEB.PlayerSheet, PlayerSheet.PNORMAL);
        }*/

        public new void Update(GameTime time)
        {
            LoadContent("PlayerIdle", true);
            /*if (type == droneType.SideBySide)
            {
                rot = master.rot;
                pos.X = master.pos.X + (float)Math.Cos(rot) * 50;
                pos.Y = master.pos.Y + (float)Math.Sin(rot) * 50;
            }
            if (type == droneType.Orbital)
            {
                rot -= (float)(time.ElapsedGameTime.Milliseconds) / 1000;
                pos.X = master.pos.X + (float)Math.Cos(rot) * 50;
                pos.Y = master.pos.Y + (float)Math.Sin(rot) * 50;
            }
            */
            base.Update(time);
        }

        public new void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
