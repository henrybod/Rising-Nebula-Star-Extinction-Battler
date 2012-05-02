using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Weapons {
    [Serializable()]
    public class PlasmaWall : HenryWeapon {

        float rotation; //should be 0 or pi/2 for horizontal or vertical walls of plasma

        public PlasmaWall(HenrySpawner ship, float rotation)
            : base(ship, 0.2f) {
            this.rotation = rotation;
        }
        
        public override void SpawnBullets() {

            Vector2 delta = new Vector2();
            delta.X = 15 * (float)Math.Cos(rotation);
            delta.Y = 15 * (float)Math.Sin(rotation);

            //spawn two lines of 8 bullets
            for (int i = -4; i <= 4; i++)
                bullets.Add(new HenryBullet("BulletPlasma", this, 1, Ship.Position + i * delta, rotation, 50, true));

            for (int i = -4; i <= 4; i++)
                bullets.Add(new HenryBullet("BulletPlasma", this, 1, Ship.Position + i * delta, (float)Math.PI + rotation, 50, true));


            //and play an appropriate sound effect
            RNSEB.Audio.PlayEffect("PlasmaTorpedo");
        }
    }
}
