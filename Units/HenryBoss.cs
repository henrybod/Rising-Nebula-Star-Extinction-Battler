using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using teamstairwell.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell {
    class HenryBoss : HenrySpawner {

        public int HealthMax = 10;
        public float Health;
        private float healthOld;
        private HenryHealthBar healthBar;
        private List<HenrySpawnerBay> spawnerBays = new List<HenrySpawnerBay>();

        public HenryBoss(ContentManager cm, HenryBattlefield b) : base(cm, b){
            this.LoadContent("BossIdle", true); //initally idle
            this.CenterOrigin();
            this.MovementSpeed = 30.0f; //in km/s (lol)
            this.Animate = true;
            this.healthBar = new HenryHealthBar(cm, this);
            this.HitRadius = 70;
            Health = healthOld = HealthMax;
            spawnerBays.Add(new HenrySpawnerBay(this, "MarbleSpawner", "Marble", 1, 0.5f, 100));
        }

        public new void Update(GameTime gt) {
            if(!Dead){
                if(healthOld > Health){
                    //i've been damaged!
                    LoadContent("BossHit", false, 6);
                    RNSEB.Audio.Play("BossHit");
                } else {
                    LoadContent("BossIdle", true);
                }
                //move according to input
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

                //activate spawner bays!
                if (RNSEB.Input.GetKey("BossFire1"))
                    foreach (HenrySpawnerBay bay in spawnerBays)
                        bay.Fire();

                //update all spawner bays
                foreach (HenrySpawnerBay bay in spawnerBays)
                    bay.Update(gt);
            
                healthOld = Health;

            }

            base.Update(gt);
            
            if (Dead && !Animate) //wait to finish death animation
                RNSEB.CurrentScreen = "PlayerVictory";
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

        public new void Damage(int amount){
            if (Health <= 0)
                Dead = true;

            if (Dead){
                LoadContent("BossDeath", false, 1.0f); //dieeeee!
                RNSEB.Audio.Play("BossDeath");
            } else {
                Health -= amount;
            }
        }
    }
}
