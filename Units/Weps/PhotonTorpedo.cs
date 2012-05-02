using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {
    [Serializable()]
    public class PhotonTorpedo : HenryWeapon {

        public PhotonTorpedo(HenrySpawner ship) : base(ship, 0.2f) {
            IconName = "PhotonTorpedoIcon";
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            RNSEB.Audio.PlayEffect("PhotonTorpedo");
            bullets.Add(new HenryBullet("BulletRedFuzz", this, 1, Ship.Position, Ship.Rotation, 1000, true));
        }
    }
}
