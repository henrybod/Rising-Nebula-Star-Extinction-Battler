using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {

    class GoldLaser : HenryWeapon {

        public GoldLaser(HenrySpawner ship)
            : base(ship, 20) {
            //hmm
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            //this weapon fires two gold thingies side by side
            //we need to calculate the new starting position of the bullets (+/- delta)
            //set 10 pixels to the left/right
            Vector2 delta = new Vector2();
            delta.X = 10 * (float)Math.Cos(Ship.Rotation);
            delta.Y = 10 * (float)Math.Sin(Ship.Rotation);

            RNSEB.Audio.PlayEffect("BulletLaser");
            bullets.Add(new HenryBullet("BulletGoldLaser", this, 1, Ship.Position + delta, Ship.Rotation, 1000, true));
            bullets.Add(new HenryBullet("BulletGoldLaser", this, 1, Ship.Position - delta, Ship.Rotation, 1000, true));
        }
    }
}
