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

    class HenrySpawner : HenrySprite {

        public HenryBattlefield Battlefield;
        public bool Dead = false, Automated = false;
        private float fireRateMultiplier = 1.0f, movementSpeed = 0.0f;
        private Vector2 velocity = Vector2.Zero;
        public float FireRateMultiplier {
            get { return fireRateMultiplier; }
            set { fireRateMultiplier = (value <= 0) ? 1.0f : value; }
        }
        public float MovementSpeed {
            get { return movementSpeed; }
            set { movementSpeed = (value <= 0) ? 0.0f : value; }
        }
        public Vector2 Velocity {
            get { return velocity; }
            set {
                if (value.Length() <= MovementSpeed)
                    velocity = value;
            }
        }
        protected List<HenryWeapon> installedWeapons = new List<HenryWeapon>();


        public HenrySpawner(ContentManager cm, HenryBattlefield b) : base(cm) {
            this.Battlefield = b;
        }

        public new void Draw(SpriteBatch sb) {
            foreach (HenryWeapon w in installedWeapons)
                w.Draw(sb);
            base.Draw(sb);
        }

        public new void Update(GameTime gt){
            if (Automated){ //I'm a mindless minion of notus!
                //temporary crude ai for testing purposes
                Position.Y += (float)gt.ElapsedGameTime.TotalSeconds * MovementSpeed;
                Rotation = (float)(Math.Atan2(Position.Y - Battlefield.Zihou.Position.Y,
                                              Position.X - Battlefield.Zihou.Position.X) - Math.PI / 2);
                foreach (HenryWeapon w in installedWeapons)
                    w.Fire();
            }

            //update all weapons
            foreach (HenryWeapon w in installedWeapons)
                w.Update(gt);

            //update sprite
            base.Update(gt);
        }

        public void Damage(int amount){
            //check if dead
            //play noise
            //play anim
            //end game

            //else
            //play noise
            //play anim
        }

        public void AddWeapon(HenryWeapon w) {
            installedWeapons.Add(w);
        }
    }
}