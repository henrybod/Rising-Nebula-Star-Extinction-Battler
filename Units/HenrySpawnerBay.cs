using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using teamstairwell.Weapons;

namespace teamstairwell {

    public class HenrySpawnerBay {
        //this class holds information on a given bay, e.g. what spawners it launches
        //is "held" by the boss
        public HenrySpawner Ship;
        private string spawnerType;
        private float timeSinceLastFired = 100, spawnerVelocity = 0, launchRate = 1;
        public List<HenrySpawner> spawners = new List<HenrySpawner>();

        public HenrySpawnerBay(HenrySpawner ship, float launchRate, string spawnerType, float spawnerVelocity){
            this.Ship = ship;
            this.spawnerType = spawnerType;
            this.spawnerVelocity = spawnerVelocity;
            this.launchRate = launchRate;
        }

        public void Fire(){
            if(timeSinceLastFired >= 1/(launchRate * Ship.FireRateMultiplier)) {
                //calc new spawner's initial velocity
                Random r = new Random();
                int rx = r.Next(400);
                int ry = r.Next(400);
                if(r.Next(2) > 0) rx *= -1;
                if(r.Next(2) > 0) ry *= -1;
                Vector2 initVel = new Vector2(rx, ry); //TODO: calc where the spawner should head to

                //create
                HenrySpawner s = new HenrySpawner(Ship.cm, Ship.Battlefield, 10, 100, Ship.Position, initVel, 0.0f);

                HenryWeapon weapon;
                string sprite;
                switch(spawnerType) {
                    case "MarbleShooter":
                        sprite = "MarbleSpawner";
                        weapon = new MarbleShooter(s);
                        break;
                    case "RingOfFire":
                        sprite = "Radial";
                        weapon = new RingOfFire(s);
                        break;
                    case "HorizontalPlasmaWall":
                        sprite = "HorizontalSpawner";
                        weapon = new PlasmaWall(s, 0);
                        break;
                    case "VerticalPlasmaWall":
                        sprite = "HorizontalSpawner";
                        s.Rotation = (float)Math.PI/2;
                        weapon = new PlasmaWall(s, (float)Math.PI/2.0f);
                        break;
                    //more spawners here!!!
                    default:
                        sprite = "None";
                        weapon = new HenryWeapon(s, 1.0f); //empty weapon
                        break;
                }
                s.FocusedWeapon = weapon;
                s.ManageHitRadius = true;
                s.LoadContent(sprite, true);
                s.CenterOrigin();
                s.Position = Ship.Position;
                s.Oscillate = true;
                s.Automated = true;
                s.FocusedWeapon.timeSinceLastFired = 1.0f;
                spawners.Add(s);
                Ship.Battlefield.spawners.Add(s);
                RNSEB.Audio.PlayEffect("LaunchSpawner");
                timeSinceLastFired = 0;
            }
        }

        public void Update(GameTime gt){
            timeSinceLastFired += (float)gt.ElapsedGameTime.TotalSeconds;
            foreach (HenrySpawner s in spawners)
                s.Update(gt);
        }

        public void Draw(SpriteBatch sb){
            foreach (HenrySpawner s in spawners)
                s.Draw(sb);
        }
    }
}
