using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using teamstairwell.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Weapons;
using teamstairwell.Graphics;

namespace teamstairwell {
    [Serializable]
  public class HenryBoss : HenrySpawner {

        //private List<HenrySpawnerBay> spawnerBays = new List<HenrySpawnerBay>();
        public List<HenryWeapon> LaunchBays = new List<HenryWeapon>();
        public List<HenryWeapon> ExtraWeapons = new List<HenryWeapon>();
        public bool StaticField = false, Regeneration = false;
        private HenrySprite staticFieldSprite;
        public float DamageReceivedMultiplier = 1.0f;
        public int SelectedLaunchBay = 1;

        public HenryBoss(ContentManager cm, HenryBattlefield b, float mass, Vector2 initPos, Vector2 initVel, float damping)
            : base(cm, b, 1000, mass, initPos, initVel, damping) {
            spawnerType = "Boss";
            this.LoadContent("BossIdle", true); //initally idle
            this.CenterOrigin();
            this.Animate = true;
            this.HitRadius = 65;
            EnginePower = 30.0f;
            staticFieldSprite = new HenrySprite(cm);
            staticFieldSprite.LoadContent("BossStaticField", true);
            staticFieldSprite.CenterOrigin();
            staticFieldSprite.Scale = 0.6f;

            //starter weapon
            LaunchBays.Add(new LaunchBay(this, 0.6f, "PeaShooter", 100));
        }

        public new void Update(GameTime gt) {

            if (!Dead) {

                LoadContent("BossIdle", true);

                //get movement
                if(Automated) {
                    Vector2 forceDirection = (Battlefield.Zihao.Position - this.Position);
                    forceDirection.Normalize();
                    acceleration = forceDirection * EnginePower;
                } else {
                    Vector2 forceDirection = new Vector2(0, 0);
                    if (RNSEB.Input.GetKey("BossUp"))
                        forceDirection.Y = -1;
                    if (RNSEB.Input.GetKey("BossDown"))
                        forceDirection.Y = 1;
                    if (RNSEB.Input.GetKey("BossLeft"))
                        forceDirection.X = -1;
                    if (RNSEB.Input.GetKey("BossRight"))
                        forceDirection.X = 1;
                    if(forceDirection.Length() > 0)
                        forceDirection.Normalize();
                    acceleration = forceDirection * EnginePower;
                }
                //fire weapons!
                if (RNSEB.Input.GetKey("BossFire2") || Automated) {
                    foreach (HenryWeapon w in ExtraWeapons)
                        w.Fire();
                }

                //get weapon selection
                SelectedLaunchBay += RNSEB.Input.GetMouseWheelDelta();
                if (SelectedLaunchBay > LaunchBays.Count) SelectedLaunchBay -= LaunchBays.Count;
                else if (SelectedLaunchBay < 1) SelectedLaunchBay = LaunchBays.Count + SelectedLaunchBay;
                for (int i = 1; i <= LaunchBays.Count; i++)
                    if (RNSEB.Input.GetKey("WeaponSelect" + i.ToString()))
                       SelectedLaunchBay = i;

                //activate spawner bays!
                if (RNSEB.Input.GetKey("BossFire1") || Automated)
                    LaunchBays[SelectedLaunchBay-1].Fire();
                if (Automated)
                    SelectedLaunchBay++;

                //update weapons
                foreach (HenryWeapon w in ExtraWeapons)
                    w.Update(gt);

                //update all spawner bays
                foreach (LaunchBay bay in LaunchBays)
                    bay.Update(gt);
            
            } else if (!Animate)
                RNSEB.CurrentScreen = "PlayerVictory";

            //regeneration upgrade
            if (Regeneration)
                Health += (float)gt.ElapsedGameTime.TotalSeconds * 8; //health per second

            base.Update(gt);

        }

        public new void Draw(SpriteBatch sb) {
            if (StaticField) {
                staticFieldSprite.Position = Position;
                staticFieldSprite.Draw(sb);
            }
            foreach (HenryWeapon w in ExtraWeapons)
                w.Draw(sb);
            foreach (LaunchBay bay in LaunchBays)
                bay.Draw(sb);
            base.Draw(sb);
        }

        public override void Damage(float amount){
            Health -= amount * DamageReceivedMultiplier;

            if (Health <= 0)
                Dead = true;

            if (Dead){
                LoadContent("BossDeath", false, 1.0f); //dieeeee!
                RNSEB.Audio.PlayEffect("BossDeath");
            } else {
                LoadContent("BossHit", false, 6);
                RNSEB.Audio.PlayEffect("BossHit");
            }
        }
    }
}
