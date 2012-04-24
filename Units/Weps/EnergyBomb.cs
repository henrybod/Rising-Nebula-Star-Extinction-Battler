using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {

    public class EnergyBomb : HenryWeapon {

        public EnergyBomb(HenrySpawner ship) : base(ship, 0.2f) {
            IconName = "EnergyBombIcon";
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            RNSEB.Audio.PlayEffect("ExplosionMedium");

            //draw explosion
            HenryBullet b = new HenryBullet("EnergyBomb", this, 0, Ship.Position, 0, 0, false);
            b.Loop = false;
            b.HitRadius = 0;
            bullets.Add(b);

            //draw more 'splosions
            for (float theta = 0.0f; theta < 2 * Math.PI - (float)Math.PI / 32.0f; theta += (float)Math.PI /32.0f) {
                b = new HenryBullet("Explosion", this, 0, Ship.Position, Ship.Rotation + theta, 2000, false);
                b.Color = Color.Cyan;
                b.HitRadius = 0;
                bullets.Add(b);
            }

            foreach (HenrySpawner s in RNSEB.TheBattlefield.ships) {
                if (!(s is HenryPlayer)) {
                    s.Damage(9);
                }
            }
        }
    }
}
