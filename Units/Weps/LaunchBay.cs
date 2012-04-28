using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell.Weapons {

    public class LaunchBay : HenryWeapon {

        private List<HenrySpawner> spawners = new List<HenrySpawner>();
        private float spawnerMaxVelocity; //currently unused
        private string spawnerType;
        SpawnerTemplate template = new SpawnerTemplate();
        public float chargingTimeMax = 0.0f;
        private float chargingTime = 0.0f;
        private bool charging = false;
        private float chargePercent = 0.0f;
        public override bool Hot {
            get {
                return (spawners.Count > 0);
            }
        }

        public LaunchBay(HenrySpawner ship, float launchRate, string spawnerType, float spawnerMaxVelocity) : base(ship, launchRate) {

            this.spawnerType = spawnerType;
            this.spawnerMaxVelocity = spawnerMaxVelocity;

            template.health = 10;
            template.mass = 10;
            template.damping = 0.0f;
            if (spawnerType == "PeaShooter") {
                template.health = 2;
                template.mass = 4;
            } else if(spawnerType == "Hulk") {
                template.health = 50;
                template.mass = 50;
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
                case "QuantumMines":
                    IconName = "QuantumMinesIcon";
                    template.spriteName = "QuantumSpawner";
                    break;
                case "Replicator":
                    IconName = "ReplicatorIcon";
                    template.spriteName = "Replicator";
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
                case "Magneto":
                    IconName = "MagnetoIcon";
                    template.spriteName = "Magneto";
                    break;
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

            //better launch mechanic, I guess...
            Vector2 initVel;
            if(Ship is HenryBoss) initVel = spawnerMaxVelocity * Vector2.Normalize(RNSEB.Input.GetCursor() - Ship.Position);
            else { //(Ship is a Replicator-type Spawner
                float speed = Ship.Velocity.Length();
                double angle = Math.Atan2(Ship.Velocity.Y, Ship.Velocity.X);
                Ship.Velocity = speed * new Vector2((float)Math.Cos(angle+Math.PI/4.0f), (float)Math.Sin(angle+Math.PI/4.0f));
                initVel = speed * new Vector2((float)Math.Cos(angle-Math.PI/4.0f), (float)Math.Sin(angle-Math.PI/4.0f));
                
            }

            //create t3h spawner
            HenrySpawner s = new HenrySpawner(Ship.cm, Ship.Battlefield, template.health, template.mass, Ship.Position, initVel, template.damping);

            s.FacesTarget = template.facesTarget;
            s.ManageHitRadius = true;
            s.LoadContent(template.spriteName, true);
            s.CenterOrigin();
            s.Oscillate = true;
            s.Automated = true;

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
                case "QuantumMines":
                    s.FocusedWeapon = new QuantumMines(s);
                    break;
                case "Replicator":
                    s.FocusedWeapon = new LaunchBay(s, 0.3f, "Replicator", 100);
                    s.Oscillate = false;
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
                case "Magneto":
                    s.Magnetic = true;
                    break;
                    
            }

            s.FocusedWeapon.timeSinceLastFired = 0.0f;
            spawners.Add(s);
            Ship.Battlefield.ships.Add(s);
            RNSEB.Audio.PlayEffect("LaunchSpawner");
            timeSinceLastFired = 0;
            //phew!



        }

        public override void Update(GameTime gt) {
            if (charging) chargingTime += (float)gt.ElapsedGameTime.TotalSeconds;
            else if (chargingTime >= chargingTimeMax) ;
            charging = false;

            //remove dead spawners
            for (int i = 0; i < spawners.Count; i++) {
                if (spawners[i].Dead && !spawners[i].Animate && !spawners[i].FocusedWeapon.Hot) {
                    spawners.RemoveAt(i);
                    i--;
                }
            }
            //reset collisions
            foreach (HenrySpawner s in spawners)
                s.CollidedThisFrame = false;
            //update
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

        public override void Fire() {
            charging = true;
            base.Fire();
        }
    }
}
