using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using teamstairwell.Graphics;

namespace teamstairwell {

    class HenrySpawner : HenrySprite {

        public ContentManager cm;
        public HenryBattlefield Battlefield;
        public bool Dead = false;
        private float fireRateMultiplier = 1.0f, movementSpeed = 0.0f, hitRadius = 0.0f;
        public float FireRateMultiplier {
            get { return fireRateMultiplier; }
            set { fireRateMultiplier = (value <= 0) ? 1.0f : value; }
        }
        public float MovementSpeed {
            get { return movementSpeed; }
            set { movementSpeed = (value <= 0) ? 0.0f : value; }
        }
        public float HitRadius {
            get { return hitRadius; }
            set { hitRadius = (value <= 0) ? 0.0f : value; }
        }


        public HenrySpawner(ContentManager cm, HenryBattlefield b){
            this.Battlefield = b;
            this.cm = cm;
        }

    }
}
