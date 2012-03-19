﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Interface;
using teamstairwell.Weapons;

namespace teamstairwell {

   public class HenryPlayer : HenrySpawner {

        private float shield, shieldRegenRate, shieldDownTime, shieldDownTimeCounter = 0, shieldReturnCapacity;
        private float invulnerabilityAfterDamageLength, invulnerabilityAfterDamageCounter = 0;
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

        public HenryPlayer(ContentManager cm, HenryBattlefield b, float mass, Vector2 initPos, Vector2 initVel, float damping)
            : base(cm, b, 1, mass, initPos, initVel, damping) {
            spawnerType = "Player";
            LoadContent("PlayerIdle", true); //initally idle
            CenterOrigin();
            ShieldMax = 2; //just a random starting value (upgrades will increment this)
            Shield = 2; //starting shield charge
            shieldDownTime = 8.0f; //how many seconds should the shield stay offline when brought to 0?
            shieldReturnCapacity = 0.2f; //what percentage of the shield should be instantly restored when shield comes back online?
            ShieldRegenRate = 0.2f; //this many shield hitpoints regen per second (1 bullet takes 1 point)
            EnginePower = 800.0f; //accerlation magnitude of the engines
            HitRadius = 20; //what hit size is the player?
            invulnerabilityAfterDamageLength = 1.0f; //how much time (in seconds) should the player be invulnerable after a hit?
            shieldBar = new HenryShieldBar(cm, this);
            FocusedWeapon = new DroneLauncher(this);
        }

        public new void Update(GameTime gt){

            if (!Dead) {
                //play anim
                LoadContent("PlayerIdle", true);

                //update invulnerability from shield hit
                if(Invulnerable) invulnerabilityAfterDamageCounter += (float)gt.ElapsedGameTime.TotalSeconds;
                if(invulnerabilityAfterDamageCounter > invulnerabilityAfterDamageLength){
                    invulnerabilityAfterDamageCounter = 0;
                    Invulnerable = false;
                }

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
                    forceDirection.Normalize();
                acceleration = forceDirection * EnginePower;

                //calculate ship rotation from mouse cursor position (props to ryan)
                Rotation = (float)(Math.Atan2(Position.Y - RNSEB.Input.GetCursor().Y, Position.X - RNSEB.Input.GetCursor().X) - Math.PI / 2);

                //fire weapons!
                firingFocused = RNSEB.Input.GetKey("PlayerFire1");
                firingDiffuse = RNSEB.Input.GetKey("PlayerFire2");

            } else if (!Animate)
                RNSEB.CurrentScreen = "BossVictory";

            base.Update(gt);
        }

        public new void Draw(SpriteBatch sb){
            base.Draw(sb);
        }

        public override void Damage(int amount){
            if (Invulnerable)
                return; //no damage if invulnerbale

            if (!shieldIsUp)
                Dead = true; //if no shield and be damaged equal deddify j00!

            if (Dead) {
                Velocity = Vector2.Zero;
                acceleration = Vector2.Zero;
                LoadContent("PlayerDeath", false, 1.0f); //u b ded!
                RNSEB.Audio.PlayEffect("PlayerDeath"); //u sounds ded 2!
            } else {
                //not dead yet
                Shield -= amount;
                Invulnerable = true;

                //shield logic
                if (Shield <= 0) { //oh noes! teh shield iz down!
                    shieldIsUp = false; //this initiates logic in the update method
                    //RNSEB.Audio.Play("PlayerShieldDown!!!"); <-- todo: find sound effect
                } else { //shield absorbs damage
                    LoadContent("PlayerHit", false, 3);
                    RNSEB.Audio.PlayEffect("PlayerHit");
                }
            }
        }

        public void AddUpgrade(RNSEB.HenryUpgrade upgrade){
            switch(upgrade){
                case RNSEB.HenryUpgrade.PlayerSuperLaser:
                    FocusedWeapon = new GoldLaser(this);
                    break;

                //todo: put every player upgrade here!
            }
        }
    }
}
