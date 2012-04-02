using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {

    public class SpiralRockets : HenryWeapon {

        float rotation = 0;

        public SpiralRockets(HenrySpawner ship) : base(ship, 20) {
            IconName = "SpiralRocketsIcon";
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!

            //rocket #1
            HenryBullet b = new HenryBullet("BulletMissle", this, 2, Ship.Position, rotation, 100, true);
            b.acceleration = 400 * new Vector2((float)Math.Cos(rotation-Math.PI/2), (float)Math.Sin(rotation-Math.PI/2));
            bullets.Add(b);

            //rocket #2
            b = new HenryBullet("BulletMissle", this, 2, Ship.Position, rotation+(float)Math.PI, 100, true);
            b.acceleration = 400 * new Vector2((float)Math.Cos(rotation+Math.PI / 2), (float)Math.Sin(rotation+Math.PI / 2));
            bullets.Add(b);

            RNSEB.Audio.PlayEffect("BulletMissle");
        }

        public override void Update(GameTime gt) {
            rotation -= (float)(Math.PI * gt.ElapsedGameTime.TotalSeconds);
            if (Math.Abs(rotation) >= 2*Math.PI) rotation = 0;
            base.Update(gt);
        }
    }
}
