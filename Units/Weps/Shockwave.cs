using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {

    public class Shockwave : HenryWeapon {

        public Shockwave(HenrySpawner ship) : base(ship, 1.0f) {
            IconName = "ShockwaveIcon";
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            RNSEB.Audio.PlayEffect("BulletLaser");

            //damage everything in a line in front of ship
            //get direction vector
            Vector2 direction = Vector2.Normalize(RNSEB.Input.GetCursor() - Ship.Position);
            //set bullet
            HenryBullet b = new HenryBullet("BulletShockwave", this, 0, Ship.Position, Ship.Rotation, 400, true);
            b.Immortal = true;
            b.Dps = 75;
            b.Knockback = 0.5f;
            bullets.Add(b);
        }

        public override void Update(GameTime gt) {
            foreach (HenryBullet b in bullets) {
                b.Scale += 2.0f * (float)gt.ElapsedGameTime.TotalSeconds;
            }
            base.Update(gt);
        }
    }
}
