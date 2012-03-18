using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using teamstairwell.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell {
    [Serializable]
  public  class HenryBoss : HenrySpawner {

        new public int HealthMax = 100;
        private HenryHealthBar healthBar;
        private List<HenrySpawnerBay> spawnerBays = new List<HenrySpawnerBay>();

        public HenryBoss(ContentManager cm, HenryBattlefield b, float mass, Vector2 initPos, Vector2 initVel, float damping)
            : base(cm, b, mass, initPos, initVel, damping) {
            this.LoadContent("BossIdle", true); //initally idle
            this.CenterOrigin();
            this.Animate = true;
            this.healthBar = new HenryHealthBar(cm, this);
            this.HitRadius = 70;
            EnginePower = 30.0f;
            Health = healthOld = HealthMax;
            spawnerBays.Add(new HenrySpawnerBay(this, "MarbleSpawner", "Marble", 1, 0.5f, 100));
        }

        public new void Update(GameTime gt) {

            if (!Dead) {
                if(healthOld > Health){
                    //i've been damaged!
                    LoadContent("BossHit", false, 6);
                    RNSEB.Audio.PlayEffect("BossHit");
                } else {
                    LoadContent("BossIdle", true);
                }
                //move according to input
                /*
                float delta = this.MovementSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
                if ((RNSEB.Input.GetKey("BossUp") || RNSEB.Input.GetKey("BossDown")) && (RNSEB.Input.GetKey("BossRight") || RNSEB.Input.GetKey("BossLeft")))
                    delta /= (float)Math.Sqrt(2);
                if (RNSEB.Input.GetKey("BossUp") && Position.Y - delta >= 0)
                    this.Position.Y -= delta;
                if (RNSEB.Input.GetKey("BossDown") && Position.Y + delta <= RNSEB.RESOLUTION.Y)
                    this.Position.Y += delta;
                if (RNSEB.Input.GetKey("BossLeft") && Position.X - delta >= 0)
                    this.Position.X -= delta;
                if (RNSEB.Input.GetKey("BossRight") && Position.X + delta <= RNSEB.RESOLUTION.X)
                    this.Position.X += delta;
                */
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
                if(forceDirection.Length() > 0) forceDirection.Normalize();
                acceleration = forceDirection * EnginePower;

                //activate spawner bays!
                if (RNSEB.Input.GetKey("BossFire1"))
                    foreach (HenrySpawnerBay bay in spawnerBays)
                        bay.Fire();

                //update all spawner bays
                foreach (HenrySpawnerBay bay in spawnerBays)
                    bay.Update(gt);
            
                healthOld = Health;
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

        new public void Damage(int amount){
            if (Health <= 0)
                Dead = true;

            if (Dead){
                LoadContent("BossDeath", false, 1.0f); //dieeeee!
                RNSEB.Audio.PlayEffect("BossDeath");
            } else {
                Health -= amount;
            }
        }
    }
}
