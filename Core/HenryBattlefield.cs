using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;
using teamstairwell.Interface;
using teamstairwell.Interface.HUD;
using teamstairwell.Weapons;

namespace teamstairwell {
    
    public class HenryBattlefield : HenryScreen {

        private string music = "Level1Music";
        public bool BossMode;
        public List<HenryBullet> bullets = new List<HenryBullet>();
        public List<HenrySpawner> ships = new List<HenrySpawner>();
        public HenryPlayer Zihao;
        public HenryBoss Notus;
        public int LevelNumber = 1;
        public ProgressBar NotusHealthBar, ZihaoShieldBar;
        public WeaponBar NotusWepBar, ZihaoWepBar;

        public HenryBattlefield(bool mode) {
            BossMode = mode;
            SetBackground("BattlefieldBackground");
            SpinBackground = true;
        }

        public void LoadContent() {
            //load the default stuff
            Zihao = new HenryPlayer(RNSEB.cm, this, 20, new Vector2(600, 600), new Vector2(0, 0), 0.9999999999999f);
            Notus = new HenryBoss(RNSEB.cm, this, 100, new Vector2(200, 300), new Vector2(0, 0), 0.8f);
            ships.Add(Notus);
            ships.Add(Zihao);
            ZihaoWepBar = new WeaponBar();
            NotusWepBar = new WeaponBar();
            if (BossMode) {
                Zihao.Automated = true;
                Zihao.Invulnerable = true;
                ZihaoShieldBar = new ProgressBar(RNSEB.cm, new Rectangle(0, 0, (int)RNSEB.RESOLUTION.X, 20), "ShieldTick");
                NotusHealthBar = new ProgressBar(RNSEB.cm, new Rectangle(0, (int)RNSEB.RESOLUTION.Y - 20, (int)RNSEB.RESOLUTION.X, 20), "HealthTick");
                ZihaoWepBar.Position = new Vector2(RNSEB.RESOLUTION.X - 96, 6+22);
                NotusWepBar.Position = new Vector2(6, RNSEB.RESOLUTION.Y - (45+6+22));
                NotusWepBar.SelectorVisible = true;
            } else {
                Notus.Automated = true;
                ZihaoShieldBar = new ProgressBar(RNSEB.cm, new Rectangle(0, (int)RNSEB.RESOLUTION.Y - 20, (int)RNSEB.RESOLUTION.X, 20), "ShieldTick");
                NotusHealthBar = new ProgressBar(RNSEB.cm, new Rectangle(0, 0, (int)RNSEB.RESOLUTION.X, 20), "HealthTick");
                ZihaoWepBar.Position = new Vector2(RNSEB.RESOLUTION.X - 96, RNSEB.RESOLUTION.Y - 70);
                NotusWepBar.Position = new Vector2(6, 12+22);
            }

            ZihaoShieldBar.TickRotation = (float)Math.PI / 16.0f;
            ZihaoShieldBar.QuantityMax = Zihao.ShieldMax;
            ZihaoShieldBar.ShowValue = true;
            ZihaoShieldBar.ShowBorder = true;
            ZihaoShieldBar.ValueAccuracy = 1;
            NotusHealthBar.QuantityMax = Notus.HealthMax;
            NotusHealthBar.ShowValue = true;
            NotusHealthBar.ShowBorder = true;
            ZihaoWepBar.AddWeapon(Zihao.FocusedWeapon);
            NotusWepBar.AddWeapon(Notus.LaunchBays[0]);
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);
            Notus.Draw(sb);
            Zihao.Draw(sb);
            NotusHealthBar.Draw(sb);
            ZihaoShieldBar.Draw(sb);
            NotusWepBar.Draw(sb);
            ZihaoWepBar.Draw(sb);
        }

        public override void Update(GameTime gt) {
            base.Update(gt);

            //link to upgrade, pause menus
            if (RNSEB.Input.GetKey("Pause"))
                RNSEB.CurrentScreen = "PauseMenu";
            //choose level based on boss's health
            if (1 - Notus.Health/(float)Notus.HealthMax > LevelNumber/10.0f) {
                LevelNumber++; //take it to the next level, yo
                music = "Level" + LevelNumber.ToString() + "Music"; //dynamically generate name of music
                RNSEB.CurrentScreen = BossMode ? "BossUpgradeMenu" : "PlayerUpgradeMenu"; //let the user upgrade
            }

            //update our players
            Zihao.CollidedThisFrame = false;
            Notus.CollidedThisFrame = false;
            Zihao.Update(gt);
            Notus.Update(gt);
            CheckCollisions();

            //update interface elements
            ZihaoShieldBar.Quantity = Zihao.Shield;
            NotusHealthBar.Quantity = Notus.Health;
            ZihaoShieldBar.QuantityMax = Zihao.ShieldMax;
            NotusHealthBar.QuantityMax = Notus.HealthMax;
            ZihaoWepBar.Update(gt);
            NotusWepBar.SelectWeapon(Notus.SelectedLaunchBay);
            NotusWepBar.Update(gt);
            
            RNSEB.Audio.PlayMusic(music); //this can be called every update because of logic in PlayMusic()
        }

        public void GrantUpgrade(RNSEB.HenryUpgrade up) {
            switch(up) {
                //zihao focused weapons
                case RNSEB.HenryUpgrade.PlayerPulsePhaser:
                    Zihao.FocusedWeapon = new PulsePhaser(Zihao);
                    ZihaoWepBar.ChangeWeapon(1, Zihao.FocusedWeapon);
                    break;
                case RNSEB.HenryUpgrade.PlayerQuadLaser:
                    Zihao.FocusedWeapon = new QuadLaser(Zihao);
                    ZihaoWepBar.ChangeWeapon(1, Zihao.FocusedWeapon);
                    break;
                case RNSEB.HenryUpgrade.PlayerIonBeam:
                    Zihao.FocusedWeapon = new IonBeam(Zihao);
                    ZihaoWepBar.ChangeWeapon(1, Zihao.FocusedWeapon);
                    break;
                //one more to create!

                //zihao diffuse weapons
                case RNSEB.HenryUpgrade.PlayerTwinRockets:
                    Zihao.DiffuseWeapon = new TwinRockets(Zihao);
                    ZihaoWepBar.AddWeapon(Zihao.DiffuseWeapon);
                    break;
                case RNSEB.HenryUpgrade.PlayerSeekers:
                    Zihao.DiffuseWeapon = new SeekerMissles(Zihao);
                    ZihaoWepBar.ChangeWeapon(2, Zihao.DiffuseWeapon);
                    break;
                case RNSEB.HenryUpgrade.PlayerSpiralRockets:
                    Zihao.DiffuseWeapon = new SpiralRockets(Zihao);
                    ZihaoWepBar.ChangeWeapon(2, Zihao.DiffuseWeapon);
                    break;
                case RNSEB.HenryUpgrade.PlayerEnergyBomb:
                    Zihao.DiffuseWeapon = new EnergyBomb(Zihao);
                    ZihaoWepBar.ChangeWeapon(2, Zihao.DiffuseWeapon);
                    break;
                case RNSEB.HenryUpgrade.PlayerDrones:
                    Zihao.DiffuseWeapon = new DroneLauncher(Zihao);
                    ZihaoWepBar.ChangeWeapon(2, Zihao.DiffuseWeapon);
                    break;
                
                //zihao upgrades
                case RNSEB.HenryUpgrade.PlayerShieldCapacity1:
                    Zihao.ShieldMax += 2;
                    break;
                case RNSEB.HenryUpgrade.PlayerShieldRecharge1:
                    Zihao.ShieldRegenRate *= 1.25f;
                    break;
                case RNSEB.HenryUpgrade.PlayerShieldCapacity2:
                    Zihao.ShieldMax += 4;
                    break;
                case RNSEB.HenryUpgrade.PlayerShieldRecharge2:
                    Zihao.ShieldRegenRate *= 1.25f;
                    break;
                case RNSEB.HenryUpgrade.PlayerShieldRecovery:
                    Zihao.ShieldDownTime /= 3.0f;
                    break;


                //notus focused spawners
                case RNSEB.HenryUpgrade.BossPhotonTorpedo:
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.5f, "PhotonTorpedo", 400));
                    NotusWepBar.AddWeapon(Notus.LaunchBays[Notus.LaunchBays.Count-1]);
                    break;
                case RNSEB.HenryUpgrade.BossPlasmaTorpedo:
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.3f, "PlasmaTorpedo", 200));
                    NotusWepBar.AddWeapon(Notus.LaunchBays[Notus.LaunchBays.Count-1]);
                    break;

                //notus diffuse spawners
                case RNSEB.HenryUpgrade.BossRingOfFire:
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.3f, "RingOfFire", 300));
                    NotusWepBar.AddWeapon(Notus.LaunchBays[Notus.LaunchBays.Count - 1]);
                    break;
                case RNSEB.HenryUpgrade.BossHorizontalPlasmaWall:
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.2f, "HorizontalPlasmaWall", 300));
                    NotusWepBar.AddWeapon(Notus.LaunchBays[Notus.LaunchBays.Count - 1]);
                    break;
                case RNSEB.HenryUpgrade.BossVerticalPlasmaWall:
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.2f, "VerticalPlasmaWall", 300));
                    NotusWepBar.AddWeapon(Notus.LaunchBays[Notus.LaunchBays.Count - 1]);
                    break;
                case RNSEB.HenryUpgrade.BossHulk:
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.1f, "Hulk", 400));
                    NotusWepBar.AddWeapon(Notus.LaunchBays[Notus.LaunchBays.Count - 1]);
                    break;

                //notus upgrades
                case RNSEB.HenryUpgrade.BossStaticField:
                    Notus.ExtraWeapons.Add(new ArseLightning(Notus));
                    Notus.StaticField = true;
                    break;
                case RNSEB.HenryUpgrade.BossKevlar:
                    Notus.DamageReceivedMultiplier = 0.75f;
                    break;
                case RNSEB.HenryUpgrade.BossNanoregenerativeSubsystems:
                    Notus.Regeneration = true;
                    break;
                case RNSEB.HenryUpgrade.BossTritaniumBulkheads:
                    Notus.DamageReceivedMultiplier = 0.5f;
                    break;
                case RNSEB.HenryUpgrade.BossAutoTurrets:
                    Notus.ExtraWeapons.Add(new AutoLaser(Notus, new Vector2(-25, -25)));
                    Notus.ExtraWeapons.Add(new AutoLaser(Notus, new Vector2(-25, 25)));
                    Notus.ExtraWeapons.Add(new AutoLaser(Notus, new Vector2(25, -25)));
                    Notus.ExtraWeapons.Add(new AutoLaser(Notus, new Vector2(25, 25)));
                    break;



                default:
                    //unimplemented upgrade; do nothin'
                    break;
            }
        }

        public void CheckCollisions() {
            //check for collisions!
            foreach(HenrySpawner This in ships)
            if (!This.CollidedThisFrame && !This.Dead) {
                foreach (HenrySpawner That in ships) {
                    if (  ((This.spawnerType == "Boss" && That.spawnerType == "Player")
                        || (This.spawnerType == "Spawner" && That.spawnerType == "Player")
                        || (This.spawnerType == "Spawner" && That.spawnerType == "Spawner")
                        || (This.spawnerType == "Player" && That.spawnerType == "Spawner")
                        || (This.spawnerType == "Player" && That.spawnerType == "Boss"))
                        && !That.CollidedThisFrame
                        && !That.Dead
                        && This.Collision(That)) {

                        Vector2 v1 = This.Velocity;
                        Vector2 v2 = That.Velocity;
                        float m1 = This.mass;
                        float m2 = That.mass;

                        This.Velocity = (v1 * (m1 - m2) + 2 * m2 * v2) / (m1 + m2); //from wikipedia
                        That.Velocity = (v2 * (m2 - m1) + 2 * m1 * v1) / (m1 + m2); //ditto

                        //This.Damage(1);
                        //That.Damage(1);

                        This.CollidedThisFrame = true;
                        That.CollidedThisFrame = true;
                    }
                }
            }
        }
    }
}
