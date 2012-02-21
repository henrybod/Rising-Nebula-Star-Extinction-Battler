using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell {

    class HenrySpawnerBay {
        //this class holds information on a given weapon, i.e. what bullet, what dmg, ...
        //is "held" by the boss
        public HenrySpawner Ship;
        private string spawnerSprite;
        int damage;
        float rateOfFire, bulletVelocity, timeSinceLastFired = 0;
        public List<HenrySpawner> spawners = new List<HenrySpawner>();
        long bulletCount = 0;

        public HenrySpawnerBay(HenrySpawner ship, string spawnerSprite, int damage, float rateOfFire, float bulletVelocity){
            this.Ship = ship;
            this.spawnerSprite = spawnerSprite;
            this.damage = damage;
            this.rateOfFire = rateOfFire;
            this.bulletVelocity = bulletVelocity;
        }

        public void Fire(){
            if(timeSinceLastFired >= 1/(rateOfFire * Ship.FireRateMultiplier)){
                HenrySpawner s = new HenrySpawner(Ship.cm, Ship.Battlefield);
                s.CenterOrigin();
                s.Animate = true;
                spawners.Add(s);
                timeSinceLastFired = 0;
            }
        }

        public void Update(GameTime gt){
            timeSinceLastFired += (float)gt.ElapsedGameTime.TotalSeconds;
            foreach (HenrySpawner s in spawners)
                if(!s.Dead) s.Update(gt);
        }

        public void Draw(SpriteBatch sb){
            foreach (HenrySpawner s in spawners)
                if(!s.Dead) s.Draw(sb);
        }
    }
}
