using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Interface;

namespace teamstairwell {

    class HenryPlayer : HenrySpawner {

        public int UpgradePoints = 0;
        private float shield, shieldRegenRate, shieldDownTime, shieldDownTimeCounter = 0, shieldReturnCapacity, damping;
        private int shieldMax;
        private HenryShieldBar shieldBar;
        private bool shieldIsUp = true;
        public float Shield {
            get{ return shield; }
            set {
                if (value < 0)
                    shield = 0;
                else if (value > ShieldMax)
                    shield = ShieldMax;
                else
                    shield = value;
            }
        }
        public int ShieldMax {
            get { return shieldMax; }
            set { shieldMax = (value < 0)?0:value; }
        }
        public float ShieldRegenRate {
            get{ return shieldRegenRate; }
            set { shieldRegenRate = (value < 0)?0:value; }
        }

        public HenryPlayer(ContentManager cm, HenryBattlefield b) : base(cm, b) {
            LoadContent("PlayerIdle", true); //initally idle
            CenterOrigin();
            ShieldMax = 2; //just a random starting value (upgrades will increment this)
            Shield = 2; //starting shield charge
            shieldDownTime = 10.0f; //how many seconds should the shield stay offline when brought to 0?
            shieldReturnCapacity = 0.2f; //what percentage of the shield should be instantly restored when shield comes back online?
            ShieldRegenRate = 0.3f; //this many shield hitpoints regen per second (1 bullet takes 1 point)
            MovementSpeed = 300.0f; //in km/s (lol)
            damping = 0.98f; //the damping coefficient (how quickly should the player decelerate when not pressing any keys?)
            HitRadius = 20;
            AddWeapon(new HenryWeapon(this, "BulletLaser", "BulletLaser", 1, 20, 600));
            shieldBar = new HenryShieldBar(cm, this);
        }

        public new void Update(GameTime gt){
            if(!Dead){
                //play anim
                LoadContent("PlayerIdle", true);
                
                //update shield
                if (shieldIsUp)
                    Shield += (float)gt.ElapsedGameTime.TotalSeconds * ShieldRegenRate;
                else {
                    shieldDownTimeCounter += (float)gt.ElapsedGameTime.TotalSeconds;
                    if(shieldDownTimeCounter >= shieldDownTime){
                        Shield = shieldReturnCapacity * ShieldMax;
                        shieldIsUp = true;
                        //RNSEB.Audio.Play("PlayerShieldUp"); <-- todo: find sound effect
                        shieldDownTimeCounter = 0;
                    }
                }

                //movement
                /*
                float delta = this.MovementSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
                if ((RNSEB.Input.GetKey("PlayerUp") || RNSEB.Input.GetKey("PlayerDown"))
                    && (RNSEB.Input.GetKey("PlayerRight") || RNSEB.Input.GetKey("PlayerLeft")))
                    delta /= (float)Math.Sqrt(2);
                if (RNSEB.Input.GetKey("PlayerUp") && Position.Y - delta >= 0)
                    this.Position.Y -= delta;
                if (RNSEB.Input.GetKey("PlayerDown") && Position.Y + delta <= RNSEB.RESOLUTION.Y)
                    this.Position.Y += delta;
                if (RNSEB.Input.GetKey("PlayerLeft") && Position.X - delta >= 0)
                    this.Position.X -= delta;
                if (RNSEB.Input.GetKey("PlayerRight") && Position.X + delta <= RNSEB.RESOLUTION.X)
                    this.Position.X += delta;
                */
                Vector2 accelDir = Vector2.Zero, accel = Vector2.Zero;
                float time = (float)gt.ElapsedGameTime.TotalSeconds;
                if (RNSEB.Input.GetKey("PlayerUp"))
                    accelDir.Y = -1;
                if (RNSEB.Input.GetKey("PlayerDown"))
                    accelDir.Y = 1;
                if (RNSEB.Input.GetKey("PlayerLeft"))
                    accelDir.X = -1;
                if (RNSEB.Input.GetKey("PlayerRight"))
                    accelDir.X = 1;

                //physics!!!
                Velocity += 1000 * accelDir * time;
                Velocity *= (1 - damping * time); //damping
                Position += Velocity * time;

                //Calculate ship rotation from mouse cursor position (props to ryan)
                Rotation = (float)(Math.Atan2(Position.Y - RNSEB.Input.GetCursor().Y, Position.X - RNSEB.Input.GetCursor().X) - Math.PI/2);

                //fire weapons!
                if (RNSEB.Input.GetKey("PlayerFire1"))
                    foreach (HenryWeapon w in installedWeapons)
                        w.Fire();
            }

            base.Update(gt);
        }

        public new void Draw(SpriteBatch sb){
            base.Draw(sb);
            shieldBar.Draw(sb);
        }

        public new void Damage(int amount){

            if (!shieldIsUp)
                Dead = true;

            if (Dead) {
                LoadContent("PlayerDeath", false, 1.0f); //you be ded!
                RNSEB.Audio.Play("PlayerDeath"); // you soun ded 2!
                //todo: end the game w/ notus victory
            } else {
                //not dead yet
                Shield -= amount;
                if (Shield == 0) {
                    shieldIsUp = false;
                    //RNSEB.Audio.Play("PlayerShieldDown"); <-- todo: find sound effect
                } else {
                    LoadContent("PlayerHit", false, 3);
                    RNSEB.Audio.Play("PlayerHit");
                }
            }
        }
    }
}
