using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using teamstairwell.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell {

    public class HenryBoss : HenrySpawner {

        private HenryHealthBar healthBar;
        private List<HenrySpawnerBay> spawnerBays = new List<HenrySpawnerBay>();

        public HenryBoss(ContentManager cm, HenryBattlefield b, float mass, Vector2 initPos, Vector2 initVel, float damping)
            : base(cm, b, 1000, mass, initPos, initVel, damping) {
            spawnerType = "Boss";
            this.LoadContent("BossIdle", true); //initally idle
            this.CenterOrigin();
            this.Animate = true;
            this.healthBar = new HenryHealthBar(cm, this);
            this.HitRadius = 65;
            EnginePower = 30.0f;
            spawnerBays.Add(new HenrySpawnerBay(this, 0.2f, "VerticalPlasmaWall", 100));

        }

        public new void Update(GameTime gt) {

            if (!Dead) {

                LoadContent("BossIdle", true);

                //calculate force direction
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

                //activate spawner bays!
                if (RNSEB.Input.GetKey("BossFire1") || Automated)
                    foreach (HenrySpawnerBay bay in spawnerBays)
                        bay.Fire();

                //update all spawner bays
                foreach (HenrySpawnerBay bay in spawnerBays)
                    bay.Update(gt);
            
            } else if (!Animate)
                RNSEB.CurrentScreen = "PlayerVictory";

            base.Update(gt);

        }

        public new void Draw(SpriteBatch sb) {
            foreach(HenrySpawnerBay bay in spawnerBays)
                bay.Draw(sb);
            base.Draw(sb);
            healthBar.Draw(sb);
        }

        public void AddSpawnerBay(HenrySpawner s) {
            //todo
        }

        public override void Damage(int amount){
            if (Health <= 0)
                Dead = true;

            if (Dead){
                LoadContent("BossDeath", false, 1.0f); //dieeeee!
                RNSEB.Audio.PlayEffect("BossDeath");
            } else {
                Health -= amount;
                LoadContent("BossHit", false, 6);
                RNSEB.Audio.PlayEffect("BossHit");
            }
        }
    }
}
