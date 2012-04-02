using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell.Weapons {

    public class LaunchBay : HenryWeapon {

        private List<HenrySpawner> spawners = new List<HenrySpawner>();
        private float spawnerMaxVelocity/*currently unused*/;
        private string spawnerType;
        SpawnerTemplate template = new SpawnerTemplate();

        public LaunchBay(HenryBoss ship, float launchRate, string spawnerType, float spawnerMaxVelocity) : base(ship, launchRate) {

            this.spawnerType = spawnerType;
            this.spawnerMaxVelocity = spawnerMaxVelocity;

            template.health = 10;
            template.mass = 100;
            template.damping = 0.0f;
            if (spawnerType == "PeaShooter") {
                template.health = 2;
                template.mass = 20;
            } else if(spawnerType == "Hulk") {
                template.health = 50;
                template.mass = 1000;
                template.damping = 0.5f;
            }
            
            //create a template spawner
            switch (spawnerType) {
                case "PeaShooter":
                    IconName = "PeaShooterIcon";
                    template.spriteName = "BulletRainbow";
                    template.facesTarget = true;
                    break;
                case "PhotonTorpedo":
                    IconName = "PhotonTorpedoIcon";
                    template.spriteName = "Line";
                    template.facesTarget = true;
                    break;
                case "PlasmaTorpedo":
                    IconName = "PlasmaTorpedoIcon";
                    template.spriteName = "Skull";
                    template.facesTarget = true;
                    break;
                case "RingOfFire":
                    IconName = "RingOfFireIcon";
                    template.spriteName = "Radial";
                    break;
                case "HorizontalPlasmaWall":
                    IconName = "HorizontalPlasmaWallIcon";
                    template.spriteName = "HorizontalSpawner";
                    break;
                case "VerticalPlasmaWall":
                    IconName = "VerticalPlasmaWallIcon";
                    template.spriteName = "HorizontalSpawner";
                    break;
                case "Hulk":
                    IconName = "HulkIcon";
                    template.spriteName = "HexSpawner";
                    break;
                //more spawners here!!!
                default:
                    break;
            }
        }
        
        public override void SpawnBullets() {
            //logic for spawning spawners here!

            /*temporary mechanic determining spawner launch speed and direction
            Random r = new Random(IconName.Length);
            int rx = r.Next(200);
            int ry = r.Next(200);
            if(r.Next(2) > 0) rx *= -1;
            if(r.Next(2) > 0) ry *= -1;
            Vector2 initVel = new Vector2(rx, ry); //TODO: calc where the spawner should head to
            */

            //better launch mechanic
            Vector2 initVel = spawnerMaxVelocity * Vector2.Normalize(RNSEB.Input.GetCursor() - Ship.Position);

            //create t3h spawner
            HenrySpawner s = new HenrySpawner(Ship.cm, Ship.Battlefield, template.health, template.mass, Ship.Position, initVel, template.damping);
            switch(spawnerType) {
                case "PeaShooter":
                    s.FocusedWeapon = new PeaShooter(s);
                    s.Scale = 1.25f;
                    break;
                case "PhotonTorpedo":
                    s.FocusedWeapon = new PhotonTorpedo(s);
                    break;
                case "PlasmaTorpedo":
                    s.FocusedWeapon = new PlasmaTorpedo(s);
                    break;
                case "RingOfFire":
                    s.FocusedWeapon = new RingOfFire(s);
                    break;
                case "HorizontalPlasmaWall":
                    s.FocusedWeapon = new PlasmaWall(s, (float)Math.PI / 2);
                    s.Rotation = (float)Math.PI / 2;
                    break;
                case "VerticalPlasmaWall":
                    s.FocusedWeapon = new PlasmaWall(s, 0);
                    break;
            }

            s.FacesTarget = template.facesTarget;
            s.ManageHitRadius = true;
            s.LoadContent(template.spriteName, true);
            s.CenterOrigin();
            s.Oscillate = true;
            s.Automated = true;
            s.FocusedWeapon.timeSinceLastFired = 1.0f;
            spawners.Add(s);
            Ship.Battlefield.ships.Add(s);
            RNSEB.Audio.PlayEffect("LaunchSpawner");
            timeSinceLastFired = 0;
            //phew!



        }

        public override void Update(GameTime gt) {
            foreach (HenrySpawner s in spawners)
                s.Update(gt);
            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb) {
            foreach (HenrySpawner s in spawners)
                s.Draw(sb);
        }

        public struct SpawnerTemplate {
            public string spriteName;
            public int health;
            public float mass, damping;
            public bool facesTarget;
        }
    }
}
