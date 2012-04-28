using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {

    public class QuantumMines : HenryWeapon {

        public QuantumMines(HenrySpawner ship) : base(ship, 1) {
            IconName = "QuantumMinesIcon";
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            RNSEB.Audio.PlayEffect("BulletLaser");
            bullets.Add(new HenryBullet("BulletBlueFuzz", this, 1, Ship.Position, Ship.Rotation, 0, true));
        }
    }
}
