using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {

    class SpiralRockets : HenryWeapon {

        float rotation = 0;

        public SpiralRockets(HenrySpawner ship)
            : base(ship, 20) {
            //hmm
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            rotation -= (float)Math.PI/15;

            //for (float theta = 0.0f; theta < 2 * Math.PI; theta += (float)Math.PI / 1.5f)
                if(rotation < 0 && rotation > -Math.PI) bullets.Add(new HenryBullet("BulletMissle", this, 1, Ship.Position, Ship.Rotation+rotation, 200, true));

            if (rotation < -2 * Math.PI)
                rotation = 0;

            RNSEB.Audio.PlayEffect("BulletLaser");
        }
    }
}
