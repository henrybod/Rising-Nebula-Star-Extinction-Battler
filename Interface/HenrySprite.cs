using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace teamstairwell.Interface {

    public class HenrySprite {

        public Vector2 Position = new Vector2(0, 0);
        public Vector2 Origin = new Vector2(0, 0);
        protected Texture2D Texture;
        public Vector2 Size = new Vector2(0, 0);
        private float scale = 1.0f;
        public float Rotation = 0.0f;
        private string filename;

        public float Scale {
            get {
                return scale;
            }
            set {
                scale = value;
                Size.X = (int)(Texture.Width * scale);
                Size.Y = (int)(Texture.Height * scale);
            }
        }

        public void CenterOrigin(){
            Origin = new Vector2(Texture.Width/2.0f, Texture.Height/2.0f);
        }

        public void LoadContent(ContentManager cm, string filename) {
            Texture = cm.Load<Texture2D>(filename);
            this.filename = filename;
            Size.X = (int)(Texture.Width * scale);
            Size.Y = (int)(Texture.Height * scale);
        }

        public void Draw(SpriteBatch sprites) {
            sprites.Draw(Texture, Position,
                new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White,
                Rotation, Origin, Scale, SpriteEffects.None, 0);
        }

        public void Update(GameTime gt, Vector2 speed, Vector2 direction) {
            Position += direction * speed * (float)gt.ElapsedGameTime.TotalSeconds;
        }


    }
}
