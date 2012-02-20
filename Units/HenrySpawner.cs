using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using teamstairwell.Graphics;

namespace teamstairwell {
    class HenrySpawner : HenrySprite {
        public ContentManager cm;
        private float fireRateMultiplier = 1.0f;
        public float FireRateMultiplier {
            get {
                return fireRateMultiplier;
            }
            set {
                fireRateMultiplier = (value <= 0) ? 1.0f : value;
            }
        }
        public HenrySpawner(ContentManager cm){
            this.cm = cm;
        }
    }
}
