﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Interface;

namespace teamstairwell {

   public class HenryWeapon {
        //this class holds information on a given weapon, i.e. what bullet, what dmg, ...
        //is "held" by the player

        public HenrySpawner Ship;
        private string bulletSprite, bulletSound;
        int damage;
        float rateOfFire, bulletVelocity;
        public float timeSinceLastFired = 100;
        public List<HenryBullet> bullets = new List<HenryBullet>();

        public HenryWeapon(HenrySpawner ship, float rateOfFire) {
            this.Ship = ship;
            this.rateOfFire = rateOfFire;
        }

        public virtual void SpawnBullets() {}

        public void Fire() {

            if (!Ship.Dead && timeSinceLastFired >= 1 / (rateOfFire * Ship.FireRateMultiplier)) {
                SpawnBullets();
                timeSinceLastFired = 0;
            }
        }

        public virtual void Update(GameTime gt) {
            //fire weapon
            timeSinceLastFired += (float)gt.ElapsedGameTime.TotalSeconds;

            //update all my bullets
            foreach (HenryBullet b in bullets)
                if(!b.Spent) b.Update(gt);
        }

        public virtual void Draw(SpriteBatch sb){
            //draw all my bullets
            foreach (HenryBullet b in bullets)
                if(!b.Spent) b.Draw(sb);
        }
    }
}