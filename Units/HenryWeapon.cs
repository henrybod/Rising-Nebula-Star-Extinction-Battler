using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Interface;
using teamstairwell.Graphics;

namespace teamstairwell {
    [Serializable()]
    public class HenryWeapon {
        //this class holds information on a given weapon, i.e. what bullet, what dmg, ...
        //is "held" by the player

        public HenrySpawner Ship;
        public float rateOfFire, bulletVelocity;
        public float timeSinceLastFired;
        public List<HenryBullet> bullets = new List<HenryBullet>();
        public List<HenrySprite> otherEffects = new List<HenrySprite>();
        public string IconName = "EmptyIcon";
        public virtual bool Hot {
            get { return (bullets.Count > 0); }
        }

        public HenryWeapon(HenrySpawner ship, float rateOfFire) {
            this.Ship = ship;
            this.rateOfFire = rateOfFire;
            this.timeSinceLastFired = 1 / rateOfFire;
        }

        public virtual void SpawnBullets() {
            //a plain old HenryWeapon does nothing when fired
            //look to the children for a brighter tomorrow
        }
        
        public virtual void Fire() {
            //wait for weapon to reload, then spawn bullets
            if (!Ship.Dead && timeSinceLastFired >= 1 / (rateOfFire * Ship.FireRateMultiplier)) {
                timeSinceLastFired = 0;
                SpawnBullets();
            }
        }

        public virtual void Update(GameTime gt) {
            //accumulate time since the weapon was last fired
            timeSinceLastFired += (float)gt.ElapsedGameTime.TotalSeconds;

            //update all my bullets
            Rectangle screenArea = new Rectangle(-200, -200, (int)RNSEB.RESOLUTION.X+400, (int)RNSEB.RESOLUTION.Y+400);
            for (int i = 0; i < bullets.Count; i++) {
                bullets[i].Update(gt);
                if (bullets[i].Spent || !screenArea.Contains(new Point((int)bullets[i].Position.X, (int)bullets[i].Position.Y))) {
                    bullets.RemoveAt(i);
                    i--;
                }
            }

            //update any other stuff
            foreach (HenrySprite s in otherEffects)
                s.Update(gt);

            //check for offscreen bullets and delete them
        }

        public virtual void Draw(SpriteBatch sb) {
            //draw all my bullets
            foreach (HenryBullet b in bullets)
                if(!b.Spent) b.Draw(sb);

            //draw any other stuff
            foreach (HenrySprite s in otherEffects)
                s.Draw(sb);
        }
    }
}