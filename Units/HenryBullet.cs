using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using teamstairwell.Graphics;

namespace teamstairwell {
    [Serializable]
    public class HenryBullet : HenryMass {

        private int damage;
        private HenryWeapon WhereIWasShotFrom;
        public bool Spent = false; //has bullet impacted something?
        public bool Immortal = false; //such a bullet continues through multiple targets (IonBeam and Shockwave)
        public float Dps = 0.0f; //used for IonBeam and Shockwave
        public float Knockback = 0.0f; //used for Shockwave
        public float Lifetime = 100.0f; //time in seconds before bullet auto-expires (necessary for QuantumMines)
        private float aliveTime = 0.0f; //time bullet has existed

        public HenryBullet(string spriteName, HenryWeapon whereIWasShotFrom, int damage, Vector2 initialPosition, float rotation, float velocity, bool manageHitRadius)
            : base(whereIWasShotFrom.Ship.cm, 1, initialPosition, /*whereIWasShotFrom.Ship.Velocity + */new Vector2((float)Math.Cos(rotation - (float)Math.PI / 2), (float)Math.Sin(rotation - (float)Math.PI / 2)) * velocity, 0.0f, true) {
            this.Position = initialPosition;
            this.Rotation = rotation;
            this.damage = damage;
            this.ManageHitRadius = manageHitRadius;
            this.LoadContent(spriteName, true);
            this.Animate = true;
            this.CenterOrigin();
            this.WhereIWasShotFrom = whereIWasShotFrom;
        }

        public new void Update(GameTime gt){
            if(Position.X < -200 //stop updating bullet if off screen
                || Position.X > RNSEB.RESOLUTION.X + 200
                || Position.Y < -200
                || Position.Y > RNSEB.RESOLUTION.Y + 200)
                Spent = true;

            aliveTime += (float)gt.ElapsedGameTime.TotalSeconds;
            if (aliveTime >= Lifetime) //stop updating bullet if it has died of old age
                Spent = true;

            foreach (HenrySpawner s in WhereIWasShotFrom.Ship.Battlefield.ships) {
                if (WhereIWasShotFrom.Ship.spawnerType == "Boss" || WhereIWasShotFrom.Ship.spawnerType == "Spawner") {
                    if (s.spawnerType == "Player" && Collision(s) && !s.Dead && !s.Invulnerable) {
                        if (Immortal) {
                            s.Damage(Dps * (float)gt.ElapsedGameTime.TotalSeconds);
                            s.Velocity += Knockback * Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
                        } else {
                            s.Damage(damage);
                            Spent = true;
                        }
                    }
                } else if (WhereIWasShotFrom.Ship.spawnerType == "Player") {
                    if ((s.spawnerType == "Boss" || s.spawnerType == "Spawner") && Collision(s) && !s.Dead && !s.Invulnerable) {
                        if (Immortal) {
                            s.Damage(Dps * (float)gt.ElapsedGameTime.TotalSeconds);
                            s.Velocity += Knockback * Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
                        } else {
                            s.Damage(damage);
                            Spent = true;
                        }
                    }
                }
            }
            base.Update(gt);
        }
    }
}
