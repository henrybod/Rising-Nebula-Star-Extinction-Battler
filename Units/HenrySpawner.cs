using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;
using teamstairwell.Interface;

namespace teamstairwell {
    [Serializable()]
    public class HenrySpawner : HenryMass {

        public string spawnerType = "Spawner";
        private int healthMax;
        public int HealthMax {
            get { return healthMax; }
            set { healthMax = value;
                  Health = healthMax; }}
        private float health;
        public float Health {
            get { return health; }
            set {
                if (value <= HealthMax && value >= 0)
                    health = value;
                else if (value < 0)
                    health = 0;
            }
        }
        public float EnginePower;
        public HenryBattlefield Battlefield;
        public bool Dead = false, Automated = false, Invulnerable = false, FacesTarget = false, CollidedThisFrame = false, Magnetic = false;
        public bool firingFocused = false, firingDiffuse = false;
        private float fireRateMultiplier = 1.0f;
        public float FireRateMultiplier {
            get { return fireRateMultiplier; }
            set { fireRateMultiplier = (value <= 0) ? 1.0f : value; }
        }
        private float speedMultiplier = 1.0f;
        public float SpeedMultiplier {
            get { return speedMultiplier; }
            set { speedMultiplier = (value <= 0) ? 1.0f : value; }
        }
        public HenryWeapon FocusedWeapon, DiffuseWeapon;



        public HenrySpawner(ContentManager cm, HenryBattlefield b, int health, float mass, Vector2 initPos, Vector2 initVel, float damping)
            : base(cm, mass, initPos, initVel, damping, false) {
            this.Battlefield = b;
            HealthMax = health;
            Health = (float)health;
            FocusedWeapon = new HenryWeapon(this, 20);
            DiffuseWeapon = new HenryWeapon(this, 20);
        }

        public new void Draw(SpriteBatch sb) {
            FocusedWeapon.Draw(sb);
            DiffuseWeapon.Draw(sb);
            if (!Dead || Animate) base.Draw(sb);
        }

        public new void Update(GameTime gt){

            //note: spawners may not leave the screen

            if (!Dead && Automated) { //I'm a mindless minion of notus!
                if(FacesTarget)
                    Rotation = (float)(Math.Atan2(Position.Y - Battlefield.Zihao.Position.Y,
                                       Position.X - Battlefield.Zihao.Position.X) - Math.PI / 2);
                if (Magnetic)
                    acceleration = 2*(Battlefield.Zihao.Position - this.Position);
                
                firingFocused = true;
                firingDiffuse = true;
            }

            //update weapons
            FocusedWeapon.Update(gt);
            DiffuseWeapon.Update(gt);

            //fire weapons
            if (firingFocused)
                FocusedWeapon.Fire();
            if (firingDiffuse)
                DiffuseWeapon.Fire();

            //update physics
            if(!Dead || Animate) base.Update(gt);
        }

        public virtual void Damage(float amount) {
            Health -= amount;

            if (Health <= 0)
                Dead = true;

            if (Dead){
                Velocity = Vector2.Zero;
                acceleration = Vector2.Zero;
                LoadContent("Explosion", false, 3.0f); //dieeeee!
                CenterOrigin();
                RNSEB.Audio.PlayEffect("ExplosionSmall");
            }
        }

        public void Overcharge(float bonusFireRate, float bonusSpeed) {
            FireRateMultiplier = 1 + bonusFireRate;
            Velocity *= 1 + bonusSpeed;
            SpeedMultiplier = 1 + bonusSpeed;
        }
    }
}