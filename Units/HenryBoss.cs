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
        public int HealthMax = 10000;
        public float Health = 10000;
        private HenryHealthBar healthBar;
        private List<HenrySpawnerBay> spawnerBays = new List<HenrySpawnerBay>();

        public HenryBoss(ContentManager cm, HenryBattlefield b) : base(cm, b){
            this.LoadContent(cm, "BossIdle"); //initally idle
            this.CenterOrigin();
            this.MovementSpeed = 30.0f; //in km/s (lol)
            this.Animate = true;
            this.healthBar = new HenryHealthBar(cm, this);
            this.HitRadius = 70;
        }

        public new void Update(GameTime gt) {
            //move according to input
            float delta = this.MovementSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
            if ((HenryInput.Up || HenryInput.Down) && (HenryInput.Right || HenryInput.Left))
                delta /= (float)Math.Sqrt(2);
            if (HenryInput.UpBoss && Position.Y - delta >= 0)
                this.Position.Y -= delta;
            if (HenryInput.DownBoss && Position.Y + delta <= RNSEB.RESOLUTION.Y)
                this.Position.Y += delta;
            if (HenryInput.LeftBoss && Position.X - delta >= 0)
                this.Position.X -= delta;
            if (HenryInput.RightBoss && Position.X + delta <= RNSEB.RESOLUTION.X)
                this.Position.X += delta;

            //spawn spawners
            if (HenryInput.M1) {
                //todo
            }

            //update all spawners
            //todo

            base.Update(gt);
        }

        public new void Draw(SpriteBatch sb) {
            //todo: draw spawners
            foreach(HenrySpawnerBay bay in spawnerBays)
                bay.Draw(sb);
            base.Draw(sb);
            healthBar.Draw(sb);
        }

        public void AddSpawnerBay(HenrySpawner s) {
            //todo
        }
    }
}
