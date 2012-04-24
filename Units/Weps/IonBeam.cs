using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {

    public class IonBeam : HenryWeapon {

        public IonBeam(HenrySpawner ship) : base(ship, 1.0f) {
            IconName = "IonBeamIcon";
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            RNSEB.Audio.PlayEffect("BulletLaser");
            
            //damage everything in a line in front of ship
            //get direction vector
            Vector2 direction = Vector2.Normalize(RNSEB.Input.GetCursor() - Ship.Position);
            //find all ships to be damaged
            HenryBullet b = new HenryBullet("BulletIonBeam", this, 0, Ship.Position, Ship.Rotation, 800, true);
            b.Rotation = Ship.Rotation + (float)Math.PI / 2.0f;
            b.Dps = 200;
            b.Immortal = true;
            bullets.Add(b);
        }
    }
}
