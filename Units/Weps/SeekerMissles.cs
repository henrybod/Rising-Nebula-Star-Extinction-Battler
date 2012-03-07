using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {

    public class SeekerMissles : HenryWeapon {

        public SeekerMissles(HenrySpawner ship)
            : base(ship, 1) {
            //hmm
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!

            

            //add new bullets
            bullets.Add(new HenryBullet("BulletSeeker", this, 2, Ship.Position, Ship.Rotation, 150, true));
            bullets.Add(new HenryBullet("BulletSeeker", this, 2, Ship.Position, Ship.Rotation + (float)Math.PI/2, 150, true));
            bullets.Add(new HenryBullet("BulletSeeker", this, 2, Ship.Position, Ship.Rotation + (float)Math.PI, 150, true));
            bullets.Add(new HenryBullet("BulletSeeker", this, 2, Ship.Position, Ship.Rotation + 3*(float)Math.PI/2, 150, true));
            
            //play appropriate sound effect
            RNSEB.Audio.PlayEffect("BulletLaser"); //todo: find appropriate sound effect
        }

        //rot = (float)(Math.Atan2(target.Y - pos.Y, target.X - pos.X) + Math.PI / 2);
        //vel.X = (float)Math.Cos(rot + Math.PI / 2) * -5;
        //vel.Y = (float)Math.Sin(rot + Math.PI / 2) * -5;

        public new void Update(GameTime gt){
            Console.WriteLine("Hi!");
            base.Update(gt);
        }

    }
}
