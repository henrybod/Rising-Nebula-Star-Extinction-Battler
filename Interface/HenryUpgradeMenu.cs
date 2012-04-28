﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics;

namespace teamstairwell.Interface {

    class HenryUpgradeMenu : HenryMenu {
        
        //this is a special kind of menu with very special buttons and other mechanics necessary for
        //the user to select upgrades every "level" in a spiffy manner

        private Dictionary<string, HenryUpgradeButton> upgradeButtons = new Dictionary<string, HenryUpgradeButton>();
        private bool bossMode;
        private bool mayUpgrade = true;
        
        public HenryUpgradeMenu(bool bossMode) : base(RNSEB.cm) {
            this.bossMode = bossMode;

            AddText(0.5f, 0.1f, RNSEB.TitleFont, Color.White, "Upgrades");
            AddButton(0.9f, 0.9f, "Done", new RNSEB.OnClick(() => {
                mayUpgrade = true;
                Buttons["Done"].Enabled = false;
                RNSEB.CurrentScreen = "Battlefield";
            }));
            Buttons["Done"].Enabled = false;

            //set up player upgrade menu
            if(!bossMode) {
                //add all the buttons
                AddUpgradeButton("BasicLaser", 1, 1, RNSEB.HenryUpgrade.PlayerPulsePhaser, "Your basic starter weapon");
                AddUpgradeButton("PulsePhaser", 1, 2, RNSEB.HenryUpgrade.PlayerPulsePhaser, "Twice the damage");
                AddUpgradeButton("QuadLaser", 1, 3, RNSEB.HenryUpgrade.PlayerQuadLaser, "Four laser overkill");
                AddUpgradeButton("IonBeam", 1, 4, RNSEB.HenryUpgrade.PlayerIonBeam, "High damage; Passes through multiple targets");
                AddUpgradeButton("Shockwave", 1, 5, RNSEB.HenryUpgrade.PlayerShockwave, "Damage and repel enemies in a wide arc");

                AddUpgradeButton("TwinRockets", 2, 1, RNSEB.HenryUpgrade.PlayerTwinRockets, "A parabola of destruction");
                AddUpgradeButton("Seekers", 2, 2, RNSEB.HenryUpgrade.PlayerSeekers, "Four missles seek closest target");
                AddUpgradeButton("SpiralRockets", 2, 3, RNSEB.HenryUpgrade.PlayerSpiralRockets, "Rocket fountain");
                AddUpgradeButton("EnergyBomb", 2, 4, RNSEB.HenryUpgrade.PlayerEnergyBomb, "Area-wide damage");
                AddUpgradeButton("Drones", 2, 5, RNSEB.HenryUpgrade.PlayerDrones, "Up to four mini-starfighters protect you and fire outwards");

                AddUpgradeButton("ShieldCapacity1", 3, 1, RNSEB.HenryUpgrade.PlayerShieldCapacity1, "Increase shield capacity by +2 (4)");
                AddUpgradeButton("ShieldRecharge1", 3, 2, RNSEB.HenryUpgrade.PlayerShieldRecharge1, "Increase shield recharge rate by +25% (125%)");
                AddUpgradeButton("ShieldCapacity2", 3, 3, RNSEB.HenryUpgrade.PlayerShieldCapacity2, "Increase shield capacity by +4 (8)");
                AddUpgradeButton("ShieldRecharge2", 3, 4, RNSEB.HenryUpgrade.PlayerShieldRecharge2, "Increase shield recharge rate by +25% (125%)");
                AddUpgradeButton("ShieldRecovery", 3, 5, RNSEB.HenryUpgrade.PlayerShieldRecovery, "Decrease downtime for shield by 66%");

                //set starter weapon as obtained
                upgradeButtons["BasicLaser"].Status = HenryUpgradeButton.UpgradeStatus.Obtained;

                //set up button dependencies
                upgradeButtons["PulsePhaser"].Requirement = "BasicLaser";
                upgradeButtons["QuadLaser"].Requirement = "PulsePhaser";
                upgradeButtons["IonBeam"].Requirement = "QuadLaser";
                upgradeButtons["Shockwave"].Requirement = "IonBeam";

                upgradeButtons["TwinRockets"].Requirement = "BasicLaser";
                upgradeButtons["Seekers"].Requirement = "TwinRockets";
                upgradeButtons["SpiralRockets"].Requirement = "Seekers";
                upgradeButtons["EnergyBomb"].Requirement = "SpiralRockets";
                upgradeButtons["Drones"].Requirement = "EnergyBomb";

                upgradeButtons["ShieldCapacity1"].Requirement = "BasicLaser";
                upgradeButtons["ShieldRecharge1"].Requirement = "ShieldCapacity1";
                upgradeButtons["ShieldCapacity2"].Requirement = "ShieldRecharge1";
                upgradeButtons["ShieldRecharge2"].Requirement = "ShieldCapacity2";
                upgradeButtons["ShieldRecovery"].Requirement = "ShieldRecharge2";
            }

            //set up boss upgrade menu
            else {
                //add all the buttons
                AddUpgradeButton("PeaShooter", 1, 1, RNSEB.HenryUpgrade.BossPhotonTorpedo, "Low health; Medium attack and speed");
                AddUpgradeButton("PhotonTorpedo", 1, 2, RNSEB.HenryUpgrade.BossPhotonTorpedo, "Slow fire rate, but fast projectile");
                AddUpgradeButton("PlasmaTorpedo", 1, 3, RNSEB.HenryUpgrade.BossPlasmaTorpedo, "Large projectile");
                AddUpgradeButton("QuantumMines", 1, 4, RNSEB.HenryUpgrade.BossQuantumMines, "Stationary projectile");
                AddUpgradeButton("Replicator", 1, 5, RNSEB.HenryUpgrade.BossReplicator, "No weapon; Clones itself exponentially");

                AddUpgradeButton("RingOfFire", 2, 1, RNSEB.HenryUpgrade.BossRingOfFire, "Lots of small, slow projectiles");
                AddUpgradeButton("HorizontalPlasmaWall", 2, 2, RNSEB.HenryUpgrade.BossHorizontalPlasmaWall, "Fires a line of projectiles; Hard to avoid");
                AddUpgradeButton("VerticalPlasmaWall", 2, 3, RNSEB.HenryUpgrade.BossVerticalPlasmaWall, "Fires a line of projectiles; Hard to avoid");
                AddUpgradeButton("Hulk", 2, 4, RNSEB.HenryUpgrade.BossHulk, "No weapon; Hard to destroy");
                AddUpgradeButton("Magneto", 2, 5, RNSEB.HenryUpgrade.BossMagneto, "No weapon; Accelerates towards target");

                AddUpgradeButton("StaticField", 3, 1, RNSEB.HenryUpgrade.BossStaticField, "Shoot lightning out your arse [right mouse]");
                AddUpgradeButton("Kevlar", 3, 2, RNSEB.HenryUpgrade.BossKevlar, "Reduce incoming damage by 25%");
                AddUpgradeButton("NanoregenerativeSubsystems", 3, 3, RNSEB.HenryUpgrade.BossNanoregenerativeSubsystems, "Slowly regenerate some health");
                AddUpgradeButton("TritaniumBulkheads", 3, 4, RNSEB.HenryUpgrade.BossTritaniumBulkheads, "Reduce damage a futher 25%");
                AddUpgradeButton("AutoLasers", 3, 5, RNSEB.HenryUpgrade.BossAutoTurrets, "Keep the enemy on the run! [right mouse]");

                //set started weapon as obtained
                upgradeButtons["PeaShooter"].Status = HenryUpgradeButton.UpgradeStatus.Obtained;

                //set up button dependencies
                upgradeButtons["PhotonTorpedo"].Requirement = "PeaShooter";
                upgradeButtons["PlasmaTorpedo"].Requirement = "PhotonTorpedo";
                upgradeButtons["QuantumMines"].Requirement = "PlasmaTorpedo";
                upgradeButtons["Replicator"].Requirement = "QuantumMines";

                upgradeButtons["RingOfFire"].Requirement = "PeaShooter";
                upgradeButtons["HorizontalPlasmaWall"].Requirement = "RingOfFire";
                upgradeButtons["VerticalPlasmaWall"].Requirement = "HorizontalPlasmaWall";
                upgradeButtons["Hulk"].Requirement = "VerticalPlasmaWall";
                upgradeButtons["Magneto"].Requirement = "Hulk";

                upgradeButtons["StaticField"].Requirement = "PeaShooter";
                upgradeButtons["Kevlar"].Requirement = "StaticField";
                upgradeButtons["NanoregenerativeSubsystems"].Requirement = "Kevlar";
                upgradeButtons["TritaniumBulkheads"].Requirement = "NanoregenerativeSubsystems";
                upgradeButtons["AutoLasers"].Requirement = "TritaniumBulkheads";

            }
        }

        public void AddUpgradeButton(string name, int column, int row, RNSEB.HenryUpgrade up, string description) {
            if(column < 0 || column > 3 || row < 0 || row > 5) return;
            HenryUpgradeButton b = new HenryUpgradeButton(name, column, row, up, description);
            upgradeButtons.Add(name, b);
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);
            foreach(HenryUpgradeButton u in upgradeButtons.Values)
                u.Draw(sb);
        }

        public override void Update(GameTime gt){
            base.Update(gt);
            foreach (HenryUpgradeButton u in upgradeButtons.Values) {
                //set upgrade status
                if (u.Status != HenryUpgradeButton.UpgradeStatus.Obtained) {
                    if (upgradeButtons[u.Requirement].Status == HenryUpgradeButton.UpgradeStatus.Obtained)
                        u.Status = HenryUpgradeButton.UpgradeStatus.Obtainable;
                    else
                        u.Status = HenryUpgradeButton.UpgradeStatus.Unobtainable;
                }
                //make upgrades
                if (mayUpgrade && !(u.Status == HenryUpgradeButton.UpgradeStatus.Obtained)
                        && u.GetButtonState() == HenryButton.ButtonState.JustClicked) {
                    u.Status = HenryUpgradeButton.UpgradeStatus.Obtained;
                    RNSEB.TheBattlefield.GrantUpgrade(u.Upgrade);
                    mayUpgrade = false;
                    Buttons["Done"].Enabled = true;
                }

                u.Update(gt);
            }
        }
    }
}
