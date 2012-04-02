using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {

    public class AutoLaser : HenryWeapon {

        Vector2 relativePosition;

        public AutoLaser(HenrySpawner ship, Vector2 relativePosition) : base(ship, 5) {
            IconName = "BasicLaserIcon";
            this.relativePosition = relativePosition;
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            RNSEB.Audio.PlayEffect("BulletLaser");
            Vector2 myPosition = Ship.Position + relativePosition;
            Vector2 aim = myPosition - Ship.Battlefield.Zihao.Position;
            bullets.Add(new HenryBullet("BulletLaser", this, 1, myPosition,
                (float)(Math.Atan2(aim.Y, aim.X) - Math.PI / 2),
                800, true));
        }
    }
}
