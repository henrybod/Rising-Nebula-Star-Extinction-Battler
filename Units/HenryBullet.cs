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
        public bool Spent = false; //has bullet impacted something or left the screen?
        private string soundEffect;

        public HenryBullet(string spriteName, string soundName, HenryWeapon whereIWasShotFrom, int damage, Vector2 initialPosition, float rotation, float velocity, bool manageHitRadius)
            : base(whereIWasShotFrom.Ship.cm) {
            this.Position = initialPosition;
            this.Rotation = rotation;
            this.velocity = velocity;
            this.damage = damage;
            this.ManageHitRadius = manageHitRadius;
            this.LoadContent(spriteName, true);
            this.Animate = true;
            this.CenterOrigin();
            this.WhereIWasShotFrom = whereIWasShotFrom;
            this.soundEffect = soundName;
        }

        public new void Update(GameTime gt){
            if(Position.X < -50 //stop updating bullet if off screen
                || Position.X > RNSEB.RESOLUTION.X + 50
                || Position.Y < -50
                || Position.Y > RNSEB.RESOLUTION.Y + 50)
                Spent = true;
            if(WhereIWasShotFrom.Ship.GetType().ToString() == "teamstairwell.HenryPlayer"
                && !WhereIWasShotFrom.Ship.Battlefield.Notus.Dead
                && Collision(WhereIWasShotFrom.Ship.Battlefield.Notus)) {
                    WhereIWasShotFrom.Ship.Battlefield.Notus.Damage(damage);
                    Spent = true;
            } else if(WhereIWasShotFrom.Ship.GetType().ToString() == "teamstairwell.HenrySpawner"
                && !WhereIWasShotFrom.Ship.Battlefield.Zihao.Dead
                && Collision(WhereIWasShotFrom.Ship.Battlefield.Zihao)) {
                    WhereIWasShotFrom.Ship.Battlefield.Zihao.Damage(damage);
                    Spent = true;
            }

            Position.X += (float)Math.Cos(Rotation - (float)Math.PI / 2) * velocity * (float)gt.ElapsedGameTime.TotalSeconds;
            Position.Y += (float)Math.Sin(Rotation - (float)Math.PI / 2) * velocity * (float)gt.ElapsedGameTime.TotalSeconds;
            base.Update(gt);
        }
    }
}
