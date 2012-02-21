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
        private HenryShieldBar shieldBar;

        public HenryPlayer(ContentManager cm, HenryBattlefield b) : base(cm, b) {
            this.LoadContent(cm, "PlayerIdle"); //initally idle
            this.CenterOrigin();
            this.ShieldMax = 2; //just a random starting value (upgrades will increment this)
            this.Shield = 0; //starting shield charge (set to 0 just for demonstration of auto recharge)
            this.ShieldRegenRate = 0.3f; //this many shield hitpoints regen per second (1 bullet takes 1 point)
            this.MovementSpeed = 300.0f; //in km/s (lol)
            this.HitRadius = 20;
            this.Animate = true;
            this.installedWeapons.Add(new HenryWeapon(this, "BulletLaser", "BulletLaser", 1, 20, 600));
            shieldBar = new HenryShieldBar(cm, this);
        }

        public new void Update(GameTime gt){
            //update shield
            this.Shield += (float)gt.ElapsedGameTime.TotalSeconds * ShieldRegenRate;

            //check input
            //movement
            float delta = this.MovementSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
            if((HenryInput.Up || HenryInput.Down) && (HenryInput.Right || HenryInput.Left))
                delta /= (float)Math.Sqrt(2);
            if(HenryInput.Up && Position.Y - delta >= 0) this.Position.Y -= delta;
            if(HenryInput.Down && Position.Y + delta <= RNSEB.RESOLUTION.Y) this.Position.Y += delta;
            if(HenryInput.Left && Position.X - delta >= 0) this.Position.X -= delta;
            if(HenryInput.Right && Position.X + delta <= RNSEB.RESOLUTION.X) this.Position.X += delta;

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
            shieldBar.Draw(sb);
        }
        
        public void AddWeapon(HenryWeapon w){
            installedWeapons.Add(w);
        }
    }
}
