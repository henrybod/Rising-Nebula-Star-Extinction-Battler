using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using teamstairwell.Graphics;

namespace teamstairwell {

    class HenryBullet : HenryMass {

        private int damage;
        public HenryWeapon WhereIWasShotFrom;
        public bool Spent = false; //has bullet impacted something or left the screen?

        public HenryBullet(string spriteName, HenryWeapon whereIWasShotFrom, int damage, Vector2 initialPosition, float rotation, float velocity, bool manageHitRadius)
            : base(whereIWasShotFrom.Ship.cm, 1, initialPosition, new Vector2((float)Math.Cos(rotation - (float)Math.PI / 2), (float)Math.Sin(rotation - (float)Math.PI / 2)) * velocity, 0.0f, true) {
            this.Position = initialPosition;
            this.Rotation = rotation;
            this.damage = damage;
            this.ManageHitRadius = manageHitRadius;
            this.LoadContent(spriteName, true);
            this.Animate = true;
            this.CenterOrigin();
            this.WhereIWasShotFrom = whereIWasShotFrom;
        }

        public new void Update(GameTime gt){
            if(Position.X < -50 //stop updating bullet if off screen
                || Position.X > RNSEB.RESOLUTION.X + 50
                || Position.Y < -50
                || Position.Y > RNSEB.RESOLUTION.Y + 50)
                Spent = true;
            
            if(WhereIWasShotFrom.Ship.spawnerType == "Player") { //if I was shot from the player, I should damage spawners and boss
                foreach(HenrySpawner s in WhereIWasShotFrom.Ship.Battlefield.spawners)
                    if(!s.Dead && Collision(s)){
                        if (s.spawnerType == "Boss")
                            ((HenryBoss)s).Damage(damage);
                        else
                            s.Damage(damage);
                        Spent = true;
                    }

            } else if((WhereIWasShotFrom.Ship.spawnerType == "Boss" //if I was shot from the boss or a spawner, I should damage player
                || WhereIWasShotFrom.Ship.spawnerType == "Spawner")
                && !WhereIWasShotFrom.Ship.Battlefield.Zihao.Dead
                && Collision(WhereIWasShotFrom.Ship.Battlefield.Zihao)) {
                    WhereIWasShotFrom.Ship.Battlefield.Zihao.Damage(damage);
                    Spent = true;
            }

            base.Update(gt);
        }
    }
}
