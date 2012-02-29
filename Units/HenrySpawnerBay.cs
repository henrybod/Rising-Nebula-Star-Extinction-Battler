using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace teamstairwell {

    class HenrySpawnerBay {
        //this class holds information on a given bay, e.g. what spawners it launches
        //is "held" by the boss
        public HenrySpawner Ship;
        private string spawnerSprite, bulletSprite;
        int damage;
        float rateOfFire, bulletVelocity, timeSinceLastFired = 0;
        public List<HenrySpawner> spawners = new List<HenrySpawner>();

        public HenrySpawnerBay(HenrySpawner ship, string spawnerSprite, string bulletSprite, int damage, float rateOfFire, float bulletVelocity){
            this.Ship = ship;
            this.spawnerSprite = spawnerSprite;
            this.bulletSprite = bulletSprite;
            this.damage = damage;
            this.rateOfFire = rateOfFire;
            this.bulletVelocity = bulletVelocity;
        }

        public void Fire(){
            if(timeSinceLastFired >= 1/(rateOfFire * Ship.FireRateMultiplier)){
                HenrySpawner s = new HenrySpawner(Ship.cm, Ship.Battlefield, 100, Ship.Position, Ship.Velocity, 0.5f);
                s.LoadContent(spawnerSprite, true);
                s.CenterOrigin();
                s.Position = Ship.Position;
                s.Oscillate = true;
                s.Automated = true;
                s.focusedWeapon = new HenryWeapon(s, bulletSprite,"Impact", damage, rateOfFire, bulletVelocity);
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
