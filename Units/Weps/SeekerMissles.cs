using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {

    public class SeekerMissles : HenryWeapon {

        private const float rotationalVelocity = (float)Math.PI/2.0f;

        public SeekerMissles(HenrySpawner ship)
            : base(ship, 1) {
            //hmm
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!

            //add new bullets
            bullets.Add(new HenryBullet("BulletSeeker", this, 2, Ship.Position, Ship.Rotation, 50, true));
            bullets.Add(new HenryBullet("BulletSeeker", this, 2, Ship.Position, Ship.Rotation + (float)Math.PI/2, 50, true));
            bullets.Add(new HenryBullet("BulletSeeker", this, 2, Ship.Position, Ship.Rotation + (float)Math.PI, 50, true));
            bullets.Add(new HenryBullet("BulletSeeker", this, 2, Ship.Position, Ship.Rotation + 3*(float)Math.PI/2, 50, true));
            
            //play appropriate sound effect
            RNSEB.Audio.PlayEffect("BulletLaser"); //todo: find appropriate sound effect
        }

        public override void Update(GameTime gt){
            //update each bullet i've spawned
            foreach(HenryBullet b in bullets){
                //step 1: find a target (closest spawner)
                float smallestDistance = 100000000; //just some large distance to make the first comparison with
                Vector2 targetPosition = new Vector2();
                foreach(HenrySpawner s in RNSEB.TheBattlefield.ships){
                    if(s.Dead || s.spawnerType == "Player") continue;
                    float distance = 0;
                    Vector2.Distance(ref b.Position, ref s.Position, out distance);
                    if(distance < smallestDistance){
                        smallestDistance = distance;
                        targetPosition = s.Position;
                    }
                }

                //step 2: rotate towards that target
                //if i'm pointing right at the target, do nothing
                //if i'm point left of the target, rotate right
                if (b.Rotation < (float)(Math.Atan2(targetPosition.Y - b.Position.Y, targetPosition.X - b.Position.X) + MathHelper.PiOver2))
                    b.Rotation += rotationalVelocity * (float)gt.ElapsedGameTime.TotalSeconds;
                else if (b.Rotation > (float)(Math.Atan2(targetPosition.Y - b.Position.Y, targetPosition.X - b.Position.X) + MathHelper.PiOver2))
                    b.Rotation -= rotationalVelocity * (float)gt.ElapsedGameTime.TotalSeconds;
                
                
                //b.Rotation = (float)(Math.Atan2(targetPosition.Y - b.Position.Y, targetPosition.X - b.Position.X) + MathHelper.PiOver2);
                //b.Velocity *= Vector2.Normalize(targetPosition - b.Position);
                b.Velocity.X = (float)Math.Cos(b.Rotation + Math.PI / 2) * -200;
                b.Velocity.Y = (float)Math.Sin(b.Rotation + Math.PI / 2) * -200;
            }
            base.Update(gt);
        }

    }
}
