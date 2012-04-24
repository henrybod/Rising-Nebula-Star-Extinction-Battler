using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using teamstairwell.Graphics;

namespace teamstairwell.Weapons {

    public class ArseLightning : HenryWeapon {

        HenrySprite lightning;

        public ArseLightning(HenrySpawner ship) : base(ship, 2) {
            IconName = "StaticFieldIcon";
        }
        
        public override void SpawnBullets() {
            //logic for spawning bullets here!
            float distance = (RNSEB.TheBattlefield.Zihao.Position - RNSEB.TheBattlefield.Notus.Position).Length();
            if (distance < 160) {
                //zihao is in range! zap!
                //damage zihao
                RNSEB.TheBattlefield.Zihao.Damage(2);
                //play lightning sound effect
                RNSEB.Audio.PlayEffect("BulletLaser");
                //draw lightning
                lightning = new HenrySprite(RNSEB.cm);
                lightning.LoadContent("ArseLightning", false, 5);
                lightning.CenterOrigin();
                otherEffects.Add(lightning);
                //scale lightning
                lightning.Scale = distance / lightning.Size.X;
                //rotate lightning
                lightning.Rotation = (float)(Math.Atan2(Ship.Position.Y - Ship.Battlefield.Zihao.Position.Y,
                   Ship.Position.X - Ship.Battlefield.Zihao.Position.X) + Math.PI);
                //position lightning
                lightning.Position = Ship.Position + (lightning.Size.X/2)*new Vector2((float)Math.Cos(lightning.Rotation), (float)Math.Sin(lightning.Rotation));
                lightning.Animate = true;
                
            } else
                base.timeSinceLastFired = 5.0f; //don't fire unless zihao is in range
        }
    }
}
