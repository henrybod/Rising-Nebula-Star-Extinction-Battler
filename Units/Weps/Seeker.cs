using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace teamstairwell.Units.Weps {
    class Seeker : HenryBullet {

        public Seeker(string spriteName, HenryWeapon whereIWasShotFrom, int damage, Vector2 initialPosition, float rotation, float velocity, bool manageHitRadius)
            : base(spriteName, whereIWasShotFrom, damage, initialPosition, rotation, velocity, manageHitRadius){

        }

        public new void Update(GameTime gt){

        }
    }
}
