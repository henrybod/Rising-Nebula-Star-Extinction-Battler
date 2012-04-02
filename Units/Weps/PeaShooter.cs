using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {

    public class PeaShooter : HenryWeapon {

        public PeaShooter(HenrySpawner ship) : base(ship, 1) {
            IconName = "PeaShooterIcon";
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            RNSEB.Audio.PlayEffect("BulletPea");
            bullets.Add(new HenryBullet("BulletPea", this, 1, Ship.Position, Ship.Rotation, 300, true));
        }
    }
}
