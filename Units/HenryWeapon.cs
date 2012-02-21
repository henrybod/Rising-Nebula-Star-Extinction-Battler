using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Interface;

namespace teamstairwell {

    class HenryWeapon {
        //this class holds information on a given weapon, i.e. what bullet, what dmg, ...
        //is "held" by the player
        public HenrySpawner Ship;
        private string bulletSprite, bulletSound;
        int damage;
        float rateOfFire, bulletVelocity, timeSinceLastFired = 0;
        public Dictionary<long, HenryBullet> bullets = new Dictionary<long, HenryBullet>();
        long bulletCount = 0;

        public HenryWeapon(HenrySpawner ship, string bulletSprite, string bulletSound, int damage, float rateOfFire, float bulletVelocity){
            this.Ship = ship;
            this.bulletSprite = bulletSprite;
            this.damage = damage;
            this.rateOfFire = rateOfFire;
            this.bulletVelocity = bulletVelocity;
            this.bulletSound = bulletSound;
        }

        public void Fire(){
            if(timeSinceLastFired >= 1/(rateOfFire * Ship.FireRateMultiplier)){
                HenryBullet b = new HenryBullet(bulletSprite, bulletSound, this, bulletCount, 1, Ship.Position, Ship.Rotation, bulletVelocity);
                b.CenterOrigin();
                b.Animate = true;
                bullets.Add(bulletCount++, b);
                timeSinceLastFired = 0;
            }
        }

        public void Update(GameTime gt){
            timeSinceLastFired += (float)gt.ElapsedGameTime.TotalSeconds;
            foreach (HenryBullet b in bullets.Values)
                if(!b.Spent) b.Update(gt);
        }

        public void Draw(SpriteBatch sb){
            foreach (HenryBullet b in bullets.Values)
                if(!b.Spent) b.Draw(sb);
        }
    }
}
