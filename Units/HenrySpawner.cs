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

   public class HenrySpawner : HenryMass {

        public int HealthMax = 10;
        public float Health, EnginePower;
        protected float healthOld;
        public HenryBattlefield Battlefield;
        public bool Dead = false, Automated = false, FiringFocused = false, FiringDiffuse = false;
        private float fireRateMultiplier = 1.0f;
        public Vector2 Velocity = Vector2.Zero;
        public float FireRateMultiplier {
            get { return fireRateMultiplier; }
            set { fireRateMultiplier = (value <= 0) ? 1.0f : value; }
        }
        public HenryWeapon focusedWeapon, diffuseWeapon;


        public HenrySpawner(ContentManager cm, HenryBattlefield b, float mass, Vector2 initPos, Vector2 initVel, float damping)
            : base(cm, mass, initPos, initVel, damping, false) {
            this.Battlefield = b;
        }

        public new void Draw(SpriteBatch sb) {
            if (focusedWeapon != null) focusedWeapon.Draw(sb);
            if (diffuseWeapon != null) diffuseWeapon.Draw(sb);
            if (!Dead || Animate) base.Draw(sb);
        }

        public new void Update(GameTime gt){
            //spawners may not leave the screen
            

            if (Automated) { //I'm a mindless minion of notus!
                //temporary crude ai for testing purposes (face player and fire)
                Rotation = (float)(Math.Atan2(Position.Y - Battlefield.Zihao.Position.Y,
                                              Position.X - Battlefield.Zihao.Position.X) - Math.PI / 2);
                FiringFocused = true;
            }

            //update all weapons
            if (focusedWeapon != null) focusedWeapon.Update(gt);
            if (diffuseWeapon != null) diffuseWeapon.Update(gt);

            //update mass
            if(!Dead || Animate) base.Update(gt);
        }

        public void Damage(int amount) {
            if (Health <= 0)
                Dead = true;

            if (Dead){
                velocity = Vector2.Zero;
                LoadContent("PlayerDeath", false, 1.0f); //dieeeee!
                RNSEB.Audio.PlayEffect("SmallExplosion");
            } else {
                Health -= amount;
            }
        }
    }
}