using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {
    [Serializable()]
    public class BasicLaser : HenryWeapon {

        public BasicLaser(HenrySpawner ship) : base(ship, 20) {
            IconName = "BasicLaserIcon";
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            RNSEB.Audio.PlayEffect("Laser");
            bullets.Add(new HenryBullet("BulletLaser", this, 1, Ship.Position, Ship.Rotation, 800, true));
        }
    }
}
