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

        protected ContentManager cm;
        public float MovementSpeed;
        private float shield, shieldRegenRate;
        private int shieldMax;
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
        private List<HenryWeapon> installedWeapons = new List<HenryWeapon>();

        public HenryPlayer(ContentManager cm) : base(cm) {
            this.LoadContent(cm, "PlayerIdle"); //initally idle
            this.CenterOrigin();
            this.ShieldMax = 2; //just a random starting value (upgrades will increment this)
            this.Shield = ShieldMax;
            this.ShieldRegenRate = 0.3f; //this many shield hitpoints regen per second (1 bullet takes 1 point)
            this.MovementSpeed = 300.0f; //in km/s (lol)
            this.Animate = true;
            this.installedWeapons.Add(new HenryWeapon(this, "BulletLaser", 1, 20, 600));
        }

        public new void Update(GameTime gt){
            //update t3h shieldbar
            //shieldbar.shieldVal += gt.ElapsedGameTime.TotalSeconds * shieldRegenRate;

            //check input
            //movement
            float delta = this.MovementSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
            if((HenryInput.Up || HenryInput.Down) && (HenryInput.Right || HenryInput.Left))
                delta /= (float)Math.Sqrt(2);
            if(HenryInput.Up) this.Position.Y -= delta;
            if(HenryInput.Down) this.Position.Y += delta;
            if(HenryInput.Left) this.Position.X -= delta;
            if(HenryInput.Right) this.Position.X += delta;

            //Calculate ship rotation from mouse cursor position (props to ryan)
            Rotation = (float)(Math.Atan2(Position.Y - HenryInput.MouseY, Position.X - HenryInput.MouseX) - Math.PI/2);

            //fire weapons!
            if(HenryInput.M1){
                foreach(HenryWeapon w in installedWeapons)
                    w.Fire();
            }
            
            //update all weapons
            foreach(HenryWeapon w in installedWeapons)
                w.Update(gt);

            base.Update(gt);
        }

        public new void Draw(SpriteBatch sb){
            foreach (HenryWeapon w in installedWeapons)
                w.Draw(sb);
            base.Draw(sb);
        }
        
        public void AddWeapon(HenryWeapon w){
            installedWeapons.Add(w);
        }
    }
}
