﻿using System;
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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace teamstairwell {
    
    [Serializable()]
    public class HenryBattlefield : HenryScreen, ISerializable {

        private string music = "Level1Music";
        public bool BossMode;
        public bool MultiMode;
        public List<HenrySpawner> ships = new List<HenrySpawner>();
        public HenryPlayer Zihao;
        public HenryBoss Notus;
        public int LevelNumber = 1;
        public ProgressBar NotusHealthBar, ZihaoShieldBar;
        public WeaponBar NotusWepBar, ZihaoWepBar;
        List<RNSEB.HenryUpgrade> AIUpgrades = new List<RNSEB.HenryUpgrade>(); //list of upgrades the AI player has obtained

        public HenryBattlefield(bool mode) {
            BossMode = mode;
            MultiMode = false;
            SetBackground("BattlefieldBackground");
            SpinBackground = true;
            //RNSEB.Win.ClientSizeChanged += new EventHandler<EventArgs>(RecalibrateHUD);
        }

        public HenryBattlefield(bool mode, bool multi)
        {
            BossMode = mode;
            MultiMode = multi;
            SetBackground("BattlefieldBackground");
            SpinBackground = true;
            //RNSEB.Win.ClientSizeChanged += new EventHandler<EventArgs>(RecalibrateHUD);
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
                if (!MultiMode)
                {
                    Zihao.Automated = true;
                }
                ZihaoShieldBar = new ProgressBar(RNSEB.cm, new Rectangle(0, 0, (int)RNSEB.RESOLUTION.X, 20), "ShieldTick");
                NotusHealthBar = new ProgressBar(RNSEB.cm, new Rectangle(0, (int)RNSEB.RESOLUTION.Y - 20, (int)RNSEB.RESOLUTION.X, 20), "HealthTick");
                ZihaoWepBar.Position = new Vector2(RNSEB.RESOLUTION.X - 96, 6+22);
                NotusWepBar.Position = new Vector2(6, RNSEB.RESOLUTION.Y - (45+6+22));
                NotusWepBar.SelectorVisible = true;
            } else {
                if (!MultiMode)
                {
                    Notus.Automated = true;
                }
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
                RNSEB.Audio.PlayEffect("LevelUp");

                //upgrade the AI (if single player)
                if(Zihao.Automated) ChooseAIUpgrade(false);
                if(Notus.Automated) ChooseAIUpgrade(true);
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

        public void ChooseAIUpgrade(bool bossMode) {
            Random rand = new Random();
            int category = rand.Next(3) + 1; //choose a focused, diffuse, or defensive upgrade

            if (bossMode) { //boss upgrades
                if (category == 1) {
                    if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.BossPhotonTorpedo)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.BossPhotonTorpedo);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.BossPhotonTorpedo);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.BossPlasmaTorpedo)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.BossPlasmaTorpedo);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.BossPlasmaTorpedo);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.BossQuantumMines)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.BossQuantumMines);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.BossQuantumMines);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.BossReplicator)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.BossReplicator);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.BossReplicator);
                    } else
                        ChooseAIUpgrade(bossMode); //tried to upgrade maxed category ... try again.
                } else if (category == 2) {
                    if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.BossRingOfFire)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.BossRingOfFire);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.BossRingOfFire);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.BossHorizontalPlasmaWall)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.BossHorizontalPlasmaWall);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.BossHorizontalPlasmaWall);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.BossVerticalPlasmaWall)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.BossVerticalPlasmaWall);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.BossVerticalPlasmaWall);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.BossHulk)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.BossHulk);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.BossHulk);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.BossMagneto)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.BossMagneto);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.BossMagneto);
                    } else
                        ChooseAIUpgrade(bossMode); //tried to upgrade maxed category ... try again.
                } else if (category == 3) {
                    if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.BossStaticField)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.BossStaticField);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.BossStaticField);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.BossKevlar)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.BossKevlar);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.BossKevlar);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.BossNanoregenerativeSubsystems)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.BossNanoregenerativeSubsystems);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.BossNanoregenerativeSubsystems);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.BossTritaniumBulkheads)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.BossTritaniumBulkheads);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.BossTritaniumBulkheads);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.BossAutoTurrets)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.BossAutoTurrets);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.BossAutoTurrets);
                    } else
                        ChooseAIUpgrade(bossMode); //tried to upgrade maxed category ... try again.
                }

            } else { //zihao upgrades

                if (category == 1) {
                    if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.PlayerPulsePhaser)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.PlayerPulsePhaser);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.PlayerPulsePhaser);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.PlayerQuadLaser)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.PlayerQuadLaser);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.PlayerQuadLaser);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.PlayerIonBeam)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.PlayerIonBeam);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.PlayerIonBeam);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.PlayerShockwave)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.PlayerShockwave);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.PlayerShockwave);
                    } else
                        ChooseAIUpgrade(bossMode); //tried to upgrade maxed category ... try again.
                } else if (category == 2) {
                    if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.PlayerTwinRockets)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.PlayerTwinRockets);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.PlayerTwinRockets);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.PlayerSeekers)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.PlayerSeekers);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.PlayerSeekers);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.PlayerSpiralRockets)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.PlayerSpiralRockets);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.PlayerSpiralRockets);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.PlayerEnergyBomb)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.PlayerEnergyBomb);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.PlayerEnergyBomb);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.PlayerDrones)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.PlayerDrones);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.PlayerDrones);
                    } else
                        ChooseAIUpgrade(bossMode); //tried to upgrade maxed category ... try again.
                } else if (category == 3) {
                    if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.PlayerShieldCapacity1)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.PlayerShieldCapacity1);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.PlayerShieldCapacity1);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.PlayerShieldRecharge1)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.PlayerShieldRecharge1);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.PlayerShieldRecharge1);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.PlayerShieldCapacity2)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.PlayerShieldCapacity2);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.PlayerShieldCapacity2);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.PlayerShieldRecharge2)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.PlayerShieldRecharge2);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.PlayerShieldRecharge2);
                    } else if (!AIUpgrades.Contains(RNSEB.HenryUpgrade.PlayerShieldRecovery)) {
                        GrantUpgrade(RNSEB.HenryUpgrade.PlayerShieldRecovery);
                        AIUpgrades.Add(RNSEB.HenryUpgrade.PlayerShieldRecovery);
                    } else
                        ChooseAIUpgrade(bossMode); //tried to upgrade maxed category ... try again.
                }
            }

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
                case RNSEB.HenryUpgrade.PlayerShockwave:
                    Zihao.FocusedWeapon = new Shockwave(Zihao);
                    ZihaoWepBar.ChangeWeapon(1, Zihao.FocusedWeapon);
                    break;

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
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.3f, "PhotonTorpedo", 400));
                    NotusWepBar.AddWeapon(Notus.LaunchBays[Notus.LaunchBays.Count-1]);
                    break;
                case RNSEB.HenryUpgrade.BossPlasmaTorpedo:
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.2f, "PlasmaTorpedo", 200));
                    NotusWepBar.AddWeapon(Notus.LaunchBays[Notus.LaunchBays.Count-1]);
                    break;
                case RNSEB.HenryUpgrade.BossQuantumMines:
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.2f, "QuantumMines", 300));
                    NotusWepBar.AddWeapon(Notus.LaunchBays[Notus.LaunchBays.Count - 1]);
                    break;
                case RNSEB.HenryUpgrade.BossReplicator:
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.1f, "Replicator", 250));
                    NotusWepBar.AddWeapon(Notus.LaunchBays[Notus.LaunchBays.Count - 1]);
                    break;

                //notus diffuse spawners
                case RNSEB.HenryUpgrade.BossRingOfFire:
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.2f, "RingOfFire", 300));
                    NotusWepBar.AddWeapon(Notus.LaunchBays[Notus.LaunchBays.Count - 1]);
                    break;
                case RNSEB.HenryUpgrade.BossHorizontalPlasmaWall:
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.2f, "HorizontalPlasmaWall", 250));
                    NotusWepBar.AddWeapon(Notus.LaunchBays[Notus.LaunchBays.Count - 1]);
                    break;
                case RNSEB.HenryUpgrade.BossVerticalPlasmaWall:
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.2f, "VerticalPlasmaWall", 250));
                    NotusWepBar.AddWeapon(Notus.LaunchBays[Notus.LaunchBays.Count - 1]);
                    break;
                case RNSEB.HenryUpgrade.BossHulk:
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.1f, "Hulk", 200));
                    NotusWepBar.AddWeapon(Notus.LaunchBays[Notus.LaunchBays.Count - 1]);
                    break;
                case RNSEB.HenryUpgrade.BossMagneto:
                    Notus.LaunchBays.Add(new LaunchBay(Notus, 0.2f, "Magneto", 300));
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
                    Notus.ExtraWeapons.Add(new AutoLaser(Notus));
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
                        //|| (This.spawnerType == "Spawner" && That.spawnerType == "Spawner")
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

                        This.Damage(1);
                        That.Damage(1);

                        This.CollidedThisFrame = true;
                        That.CollidedThisFrame = true;
                    }
                }
            }
        }
        public void RecalibrateHUD(object sender, EventArgs e) {
            GameWindow window = (GameWindow)sender;
            Rectangle windowArea = window.ClientBounds;
            //unfinished
        }















        // Here lies the code of Parker Leech, noble warrior, protector of all.
        // He fought valiantly 'til the last, though in the end the Git was too mighty a foe.

        protected HenryBattlefield(SerializationInfo info, StreamingContext context) {
            BossMode = info.GetBoolean("BossMode");
            ships = (List<HenrySpawner>)info.GetValue("ships", typeof(List<HenrySpawner>));
            background = (HenrySprite)info.GetValue("background", typeof(HenrySprite));
            Zihao = (HenryPlayer)info.GetValue("Zihao", typeof(HenryPlayer));
            Notus = (HenryBoss)info.GetValue("Notus", typeof(HenryBoss));
            LevelNumber = (int)info.GetValue("LevelNumber", typeof(int));
            NotusHealthBar = (ProgressBar)info.GetValue("NotusHealthBar", typeof(ProgressBar));
            ZihaoShieldBar = (ProgressBar)info.GetValue("ZihaoShieldBar", typeof(ProgressBar));
            NotusWepBar = (WeaponBar)info.GetValue("NotusWepBar", typeof(WeaponBar));//has sprites
            ZihaoWepBar = (WeaponBar)info.GetValue("ZihaoWepBar", typeof(WeaponBar));//has sprites
            background.ReloadContent();
            NotusHealthBar.text.ReloadContent("Button");
            ZihaoShieldBar.text.ReloadContent("Button");


            //Console.WriteLine("ships not null");
            foreach (HenrySpawner hs in ships)//is this part of the problem
            {
                // Console.WriteLine("bout to reload ship");
                hs.ReloadContent();//the problem is here // don't think th cm really is doing anything, probably remove it later
                if (hs is HenryPlayer) {
                    foreach (HenryBullet b in ((HenryPlayer)hs).FocusedWeapon.bullets)
                        b.ReloadContent();
                    foreach (HenryBullet b in ((HenryPlayer)hs).DiffuseWeapon.bullets)
                        b.ReloadContent();
                } else if (hs is HenryBoss) {
                    foreach (LaunchBay lb in ((HenryBoss)hs).LaunchBays)
                        foreach (HenrySpawner sh in lb.spawners)
                            foreach (HenryBullet B in sh.FocusedWeapon.bullets)
                                B.ReloadContent();
                    foreach (HenryWeapon hw in ((HenryBoss)hs).ExtraWeapons)
                        foreach (HenryBullet B in hw.bullets)
                            B.ReloadContent();
                } else
                    foreach (HenryBullet B in hs.FocusedWeapon.bullets)
                        B.ReloadContent();
            }
            Zihao.ReloadContent();
            ZihaoWepBar.selectorSquare.ReloadContent();
            NotusWepBar.selectorSquare.ReloadContent();
            foreach (WeaponIndicator wi in NotusWepBar.Weapons) {
                wi.icon.ReloadContent();
                wi.chargeBar.tick.ReloadContent();
                wi.chargeBar.borderPixel.ReloadContent();
                wi.text.ReloadContent("Button");
            }
            foreach (WeaponIndicator wi in ZihaoWepBar.Weapons) {
                wi.icon.ReloadContent();
                wi.chargeBar.tick.ReloadContent();
                wi.chargeBar.borderPixel.ReloadContent();
                wi.text.ReloadContent("Button");
            }

            ZihaoShieldBar.tick.ReloadContent();
            ZihaoShieldBar.borderPixel.ReloadContent();
            NotusHealthBar.tick.ReloadContent();
            NotusHealthBar.borderPixel.ReloadContent();
            Notus.ReloadContent();
            Notus.staticFieldSprite.ReloadContent();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {

            info.AddValue("BossMode", BossMode);
            info.AddValue("ships", ships);
            info.AddValue("background", background);
            info.AddValue("Zihao", Zihao);
            info.AddValue("Notus", Notus);
            info.AddValue("LevelNumber", LevelNumber);
            info.AddValue("NotusHealthBar", NotusHealthBar);
            info.AddValue("ZihaoShieldBar", ZihaoShieldBar);
            info.AddValue("NotusWepBar", NotusWepBar);
            info.AddValue("ZihaoWepBar", ZihaoWepBar);


        }
    }
}
