using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {
    [Serializable()]
    public class AutoLaser : HenryWeapon {

        int count = 0;
        int Count {
            get { return count; }
            set { count = value % 4; }
        }

        public AutoLaser(HenrySpawner ship) : base(ship, 4) {
            IconName = "BasicLaserIcon";
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            Vector2 myPosition = Ship.Position + 55 * new Vector2((float)Math.Cos(count*Math.PI/2.0f+Math.PI/4.0f), (float)Math.Sin(count*Math.PI/2.0f+Math.PI/4.0f));

            Vector2 aim = myPosition - Ship.Battlefield.Zihao.Position;
            bullets.Add(new HenryBullet("BulletLaser", this, 1, myPosition,
                (float)(Math.Atan2(aim.Y, aim.X) - Math.PI / 2),
                800, true));
            Count++;
            RNSEB.Audio.PlayEffect("Laser");
        }
    }
}
