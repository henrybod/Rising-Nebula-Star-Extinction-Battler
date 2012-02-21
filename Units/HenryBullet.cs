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
        public bool Spent = false; //has bullet impacted something or left the screen?
        private string soundEffect;

        public HenryBullet(string spriteName, string soundName, HenryWeapon whereIWasShotFrom, long id, int damage, Vector2 initialPosition, float rotation, float velocity){
            this.Position = initialPosition;
            this.Rotation = rotation;
            this.velocity = velocity;
            this.damage = damage;
            this.LoadContent(whereIWasShotFrom.Ship.cm, spriteName);
            this.Animate = true;
            this.CenterOrigin();
            this.WhereIWasShotFrom = whereIWasShotFrom;
            this.IDNumber = id;
            this.soundEffect = soundName;
        }

        public new void Update(GameTime gt){
            if(Position.X < -50 //stop updating bullet if off screen
                || Position.X > RNSEB.RESOLUTION.X + 50
                || Position.Y < -50
                || Position.Y > RNSEB.RESOLUTION.Y + 50)
                Spent = true;
            if(WhereIWasShotFrom.Ship.GetType().ToString() == "teamstairwell.HenryPlayer"
                && InHitRadius(WhereIWasShotFrom.Ship.Battlefield.Notus.Position, WhereIWasShotFrom.Ship.Battlefield.Notus.HitRadius)){
                WhereIWasShotFrom.Ship.Battlefield.Notus.Health -= damage;
                RNSEB.Audio.Play("BossDamage");
                Spent = true;
            } else if(WhereIWasShotFrom.Ship.GetType().ToString() == "teamstairwell.HenrySpawner"
                && InHitRadius(WhereIWasShotFrom.Ship.Battlefield.Zihou.Position, WhereIWasShotFrom.Ship.Battlefield.Zihou.HitRadius)){
                WhereIWasShotFrom.Ship.Battlefield.Zihou.Shield -= damage;
                Spent = true;
            }
            Position.X += (float)Math.Cos(Rotation-(float)Math.PI/2) * velocity * (float)gt.ElapsedGameTime.TotalSeconds;
            Position.Y += (float)Math.Sin(Rotation-(float)Math.PI/2) * velocity * (float)gt.ElapsedGameTime.TotalSeconds;
            base.Update(gt);
        }

        private bool InHitRadius(Vector2 center, float radius){
            if(Math.Pow(this.Position.X-center.X, 2) + Math.Pow(this.Position.Y-center.Y, 2) < Math.Pow(radius, 2))
                return true;
            else
                return false;
        }
    }
}
