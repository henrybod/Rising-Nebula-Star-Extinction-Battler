﻿using System;
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
        private float shield, shieldRegenRate, shieldDownTime, shieldDownTimeCounter = 0, shieldReturnCapacity;
        private int shieldMax;
        private HenryShieldBar shieldBar;
        private bool shieldIsUp = true;
        public List<Drone> Drones;
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

        public HenryPlayer(ContentManager cm, HenryBattlefield b, float mass, Vector2 initPos, Vector2 initVel, float damping)
            : base(cm, b, mass, initPos, initVel, damping) {
            LoadContent("PlayerIdle", true); //initally idle
            CenterOrigin();
            ShieldMax = 2; //just a random starting value (upgrades will increment this)
            Shield = 2; //starting shield charge
            shieldDownTime = 8.0f; //how many seconds should the shield stay offline when brought to 0?
            shieldReturnCapacity = 0.2f; //what percentage of the shield should be instantly restored when shield comes back online?
            ShieldRegenRate = 0.2f; //this many shield hitpoints regen per second (1 bullet takes 1 point)
            EnginePower = 1000.0f; //in km/s (lol)
            HitRadius = 20;
            focusedWeapon = new HenryWeapon(this, "BulletLaser", "BulletLaser", 1, 20, 1000);
            shieldBar = new HenryShieldBar(cm, this);
            Drones = new List<Drone>();
            Drones.Add(new Drone(cm, Battlefield, 80, new Vector2(Position.X + 40, Position.Y), new Vector2(0, 0), 0.9999999999999f, this, Drone.droneType.SideBySide));
        }

        public new void Update(GameTime gt){

            if (!Dead) {
                //play anim
                LoadContent("PlayerIdle", true);

                //update shield
                if (shieldIsUp)
                    Shield += (float)gt.ElapsedGameTime.TotalSeconds * ShieldRegenRate;
                else {
                    shieldDownTimeCounter += (float)gt.ElapsedGameTime.TotalSeconds;
                    if (shieldDownTimeCounter >= shieldDownTime) {
                        Shield = shieldReturnCapacity * ShieldMax;
                        shieldIsUp = true;
                        //RNSEB.Audio.Play("PlayerShieldUp"); <-- todo: find sound effect
                        shieldDownTimeCounter = 0;
                    }
                }

                //movement (anachronous)
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

                //calculate acceleration (the rest is handled in Mass class)
                Vector2 forceDirection = new Vector2(0, 0);
                if (RNSEB.Input.GetKey("PlayerUp"))
                    forceDirection.Y = -1;
                if (RNSEB.Input.GetKey("PlayerDown"))
                    forceDirection.Y = 1;
                if (RNSEB.Input.GetKey("PlayerLeft"))
                    forceDirection.X = -1;
                if (RNSEB.Input.GetKey("PlayerRight"))
                    forceDirection.X = 1;
                if (forceDirection.Length() > 0)
                    forceDirection.Normalize(); //hmm
                acceleration = forceDirection * EnginePower;

                //calculate ship rotation from mouse cursor position (props to ryan)
                Rotation = (float)(Math.Atan2(Position.Y - RNSEB.Input.GetCursor().Y, Position.X - RNSEB.Input.GetCursor().X) - Math.PI / 2);

                //fire weapons!
                FiringFocused = RNSEB.Input.GetKey("PlayerFire1");
                FiringDiffuse = RNSEB.Input.GetKey("PlayerFire2");

            } else if (!Animate)
                RNSEB.CurrentScreen = "BossVictory";

            base.Update(gt);
        }

        public new void Draw(SpriteBatch sb){
            base.Draw(sb);
            shieldBar.Draw(sb);
            for (int i = 0; i < Drones.Count; i++)
            {
                Drones[i].Draw(sb);
            }

        }

        public new void Damage(int amount){

            if (!shieldIsUp)
                Dead = true; //if no shield and be damage equal deddify j00!

            if (Dead) {
                LoadContent("PlayerDeath", false, 1.0f); //u b ded!
                RNSEB.Audio.PlayEffect("PlayerDeath"); //u sounds ded 2!
            } else {
                //not dead yet
                Shield -= amount;
                if (Shield == 0) {
                    shieldIsUp = false;
                    //RNSEB.Audio.Play("PlayerShieldDown"); <-- todo: find sound effect
                } else {
                    LoadContent("PlayerHit", false, 3);
                    RNSEB.Audio.PlayEffect("PlayerHit");
                }
            }
        }

        public void AddUpgrade(RNSEB.HenryUpgrade upgrade){
            switch(upgrade){
                case RNSEB.HenryUpgrade.PlayerSuperLaser:
                    focusedWeapon = new HenryWeapon(this, "BulletGoldLaser", "BulletLaser", 1, 30, 600);
                    break;
                case RNSEB.HenryUpgrade.PlayerGunnerDrones:
                    Drones.Add(new Drone(cm, Battlefield, 80, new Vector2(Position.X + 40, 0), new Vector2(0, 0),  0.9999999999999f, this, Drone.droneType.SideBySide));
                    break;

                //every player upgrade here!
            }
        }
    }
}
