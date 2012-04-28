using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell.Weapons {

    public class DroneLauncher : HenryWeapon {

        private List<HenrySpawner> drones = new List<HenrySpawner>();
        private Vector2[] positions = new Vector2[4];
        private float rotation = 0;

        public DroneLauncher(HenrySpawner ship) : base(ship, 0.3f) {
            IconName = "DronesIcon";
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!

            if (drones.Count >= 4)
                return; //we may have at most four drones at once

            //add a new drone
            HenrySpawner drone = new HenrySpawner(Ship.cm, Ship.Battlefield, 5, 50, Ship.Position, Vector2.Zero, 0.0f);
            drone.FocusedWeapon = new BasicLaser(drone);
            drone.Automated = true;
            drone.ManageHitRadius = true;
            drone.LoadContent("Drone", true);
            drone.CenterOrigin();
            drone.Position = Ship.Position;
            drone.Oscillate = true;
            drone.spawnerType = "Player";
            drones.Add(drone);
            RNSEB.TheBattlefield.ships.Add(drone);
            RNSEB.Audio.PlayEffect("LaunchSpawner");
        }

        public override void Update(GameTime gt) {
            //remove dead drones from list
            for (int k = 0; k < drones.Count; k++) {
                if (drones[k].Dead) {
                    drones.RemoveAt(k);
                    k--;
                }
            }

            //update destination position for drones (4 spots circling around player)
            rotation -= (float)Math.PI / 2.0f * (float)gt.ElapsedGameTime.TotalSeconds;
            for(int i = 0; i < 4; i++) {
                rotation += (float)Math.PI / 2.0f;
                rotation %= 2 * (float)Math.PI;
                positions[i] = Ship.Position + 50 * Vector2.Normalize(new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)));
            }

            //assign a drone to each orbital position
            int j = 0;
            foreach (HenrySpawner s in drones) {
                s.Velocity = (positions[j] - s.Position) * 2;
                s.Rotation = (float)(Math.Atan2(Ship.Position.Y - s.Position.Y, Ship.Position.X - s.Position.X) + MathHelper.Pi);
                s.Update(gt);
                j++;
            }
            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb) {
            foreach (HenrySpawner s in drones)
                s.Draw(sb);
            base.Draw(sb);
        }
    }
}
