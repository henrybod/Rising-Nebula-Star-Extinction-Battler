using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {
    [Serializable()]
    public class QuadLaser : HenryWeapon {

        public QuadLaser(HenrySpawner ship) : base(ship, 10) {
            IconName = "QuadLaserIcon";
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            float spread = (float)Math.PI/32;
            bullets.Add(new HenryBullet("BulletLaser", this, 1, Ship.Position, Ship.Rotation + spread/2, 800, true));
            bullets.Add(new HenryBullet("BulletLaser", this, 1, Ship.Position, Ship.Rotation + 3*spread/2, 800, true));
            bullets.Add(new HenryBullet("BulletLaser", this, 1, Ship.Position, Ship.Rotation - spread/2, 800, true));
            bullets.Add(new HenryBullet("BulletLaser", this, 1, Ship.Position, Ship.Rotation - 3*spread/2, 800, true));
            
            RNSEB.Audio.PlayEffect("Laser");
        }
    }
}
