using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {

    public class TwinRockets : HenryWeapon {

        public TwinRockets(HenrySpawner ship) : base(ship, 5) {
            IconName = "TwinRocketsIcon";
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            RNSEB.Audio.PlayEffect("BulletMissle");
            //rocket #1
            HenryBullet b = new HenryBullet("BulletMissle", this, 2, Ship.Position, Ship.Rotation, 100, true);
            b.acceleration = 400 * new Vector2((float)Math.Cos(Ship.Rotation-Math.PI/2.0f), (float)Math.Sin(Ship.Rotation-Math.PI/2.0f));
            b.Velocity = 100 * new Vector2((float)Math.Cos(Ship.Rotation), (float)Math.Sin(Ship.Rotation));
            bullets.Add(b);

            //rocket #2
            b = new HenryBullet("BulletMissle", this, 2, Ship.Position, Ship.Rotation, 100, true);
            b.acceleration = 400 * new Vector2((float)Math.Cos(Ship.Rotation-Math.PI/2.0f), (float)Math.Sin(Ship.Rotation-Math.PI/2.0f));
            b.Velocity = 100 * new Vector2((float)Math.Cos(Ship.Rotation+Math.PI), (float)Math.Sin(Ship.Rotation+Math.PI));
            bullets.Add(b);

        }
    }
}
