using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {

    public class RingOfFire : HenryWeapon {

        public RingOfFire(HenrySpawner ship)
            : base(ship, 0.5f) {
            //hmm
        }
        
        public override void SpawnBullets() {
            //spawn a ring of 8 bullets shooting outward
            for(float theta = 0.0f; theta < 2*Math.PI - (float)Math.PI/4.0f; theta += (float)Math.PI/4.0f)
                bullets.Add(new HenryBullet("BulletFireball", this, 1, Ship.Position, Ship.Rotation + theta, 50, true));
            
            //and play an appropriate sound effect
            RNSEB.Audio.PlayEffect("BulletTorpedo"); //todo: find a more uniquely suited effect
        }
    }
}
