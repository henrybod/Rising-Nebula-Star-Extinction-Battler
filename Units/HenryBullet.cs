using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using teamstairwell.Graphics;

namespace teamstairwell {

    class HenryBullet : HenrySprite {

        private float velocity;
        private int damage;
        public HenryWeapon WhereIWasShotFrom;
        private long IDNumber;

        public HenryBullet(string spriteName, HenryWeapon whereIWasShotFrom, long id, int damage, Vector2 initialPosition, float rotation, float velocity){
            this.Position = initialPosition;
            this.Rotation = rotation;
            this.velocity = velocity;
            this.damage = damage;
            this.LoadContent(whereIWasShotFrom.Ship.cm, spriteName);
            this.Animate = true;
            this.CenterOrigin();
            this.WhereIWasShotFrom = whereIWasShotFrom;
            this.IDNumber = id;
        }

        public new void Update(GameTime gt){
            /*if(Position.X < -50
                || Position.X > RNSEB.RESOLUTION.X + 50
                || Position.Y < -50
                || Position.Y > RNSEB.RESOLUTION.Y + 50)
                WhereIWasShotFrom.bullets.Remove(IDNumber);*/
            Position.X += (float)Math.Cos(Rotation-(float)Math.PI/2) * velocity * (float)gt.ElapsedGameTime.TotalSeconds;
            Position.Y += (float)Math.Sin(Rotation-(float)Math.PI/2) * velocity * (float)gt.ElapsedGameTime.TotalSeconds;
            base.Update(gt);
        }
    }
}
