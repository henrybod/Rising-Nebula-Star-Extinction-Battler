using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {

    public class SpiralRockets : HenryWeapon {

        float rotation, rotation2, time = 0;

        public SpiralRockets(HenrySpawner ship)
            : base(ship, 20) {
            //hmm
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            time += 0.1f;
            rotation = (float)Math.PI / 16.0f * (float)Math.Sin(time);
            rotation2 = (float)Math.PI / 16.0f * (float)Math.Sin(time+Math.PI);
            
 
            bullets.Add(new HenryBullet("BulletMissle", this, 1, Ship.Position, Ship.Rotation + rotation, 250, true));
            bullets.Add(new HenryBullet("BulletMissle", this, 1, Ship.Position, Ship.Rotation + rotation2, 250, true));
            RNSEB.Audio.PlayEffect("BulletLaser");
        }
    }
}
