﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Interface;
using teamstairwell.Graphics;

namespace teamstairwell {

   public class HenryWeapon {
        //this class holds information on a given weapon, i.e. what bullet, what dmg, ...
        //is "held" by the player

        public HenrySpawner Ship;
        public float rateOfFire, bulletVelocity;
        public float timeSinceLastFired;
        public List<HenryBullet> bullets = new List<HenryBullet>();
        public List<HenrySprite> otherEffects = new List<HenrySprite>();
        public string IconName = "EmptyIcon";

        public HenryWeapon(HenrySpawner ship, float rateOfFire) {
            this.Ship = ship;
            this.rateOfFire = rateOfFire;
            this.timeSinceLastFired = 1 / rateOfFire;
        }

        public virtual void SpawnBullets() {}

        public void Fire() {

            if (!Ship.Dead && timeSinceLastFired >= 1 / (rateOfFire * Ship.FireRateMultiplier)) {
                timeSinceLastFired = 0;
                SpawnBullets();
            }
        }

        public virtual void Update(GameTime gt) {
            //fire weapon
            timeSinceLastFired += (float)gt.ElapsedGameTime.TotalSeconds;

            //update all my bullets
            foreach (HenryBullet b in bullets)
                if(!b.Spent) b.Update(gt);

            //update any other stuff
            foreach (HenrySprite s in otherEffects)
                s.Update(gt);
        }

        public virtual void Draw(SpriteBatch sb){
            //draw all my bullets
            foreach (HenryBullet b in bullets)
                if(!b.Spent) b.Draw(sb);

            //draw any other stuff
            foreach (HenrySprite s in otherEffects)
                s.Draw(sb);
        }
    }
}