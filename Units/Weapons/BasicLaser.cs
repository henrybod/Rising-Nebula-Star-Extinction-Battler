using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell {
    class BasicLaser : HenryWeapon {

        public BasicLaser(HenrySpawner ship) : base(ship, "BulletLaser", "BulletLaser", 1, 20, 1000) {
            //hmm
        }

        public override void BulletLogic() {
            //logic for spawning bullets here!
            //function reference will be passed to bullets
        }
    }
}
