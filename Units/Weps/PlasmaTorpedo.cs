using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {
    [Serializable()]
    public class PlasmaTorpedo : HenryWeapon {

        public PlasmaTorpedo(HenrySpawner ship)
            : base(ship, 0.5f) {
            //hmm
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            RNSEB.Audio.PlayEffect("PlasmaTorpedo");
            bullets.Add(new HenryBullet("Marble", this, 1, Ship.Position, Ship.Rotation, 200, true));
        }
    }
}
