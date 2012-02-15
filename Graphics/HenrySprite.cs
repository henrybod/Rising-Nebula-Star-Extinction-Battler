using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell.Graphics.SpriteSheets;

namespace teamstairwell.Graphics {

    public class HenrySprite {

        public Vector2 Position = new Vector2(0, 0);
        public Vector2 Origin = new Vector2(0, 0);
        protected Texture2D Texture;
        public Vector2 Size = new Vector2(0, 0);
        private float scale = 1.0f;
        public float Rotation = 0.0f;
        public bool Animate = false;
        protected int frame = 0, totalFrames;
        private Rectangle viewRect = new Rectangle(0, 0, 0, 0);

        public float Scale {
            get {
                return scale;
            }
            set {
                scale = value;
                Size.X = (int)(viewRect.Width * scale);
                Size.Y = (int)(viewRect.Height * scale);
            }
        }

        public void CenterOrigin(){
            Origin = new Vector2(viewRect.Width/2.0f, viewRect.Height/2.0f);
        }

        public void LoadContent(ContentManager cm, string spriteName) {
            SpriteInfo s = (SpriteInfo)RNSEB.HenrySprites.All[spriteName];
            Texture = cm.Load<Texture2D>(s.Filename);
            Size.X = (int)(s.Width * scale);
            Size.Y = (int)(s.Height * scale);
            viewRect.Width = s.Width;
            viewRect.Height = s.Height;
            viewRect.Y = s.Y;
            totalFrames = s.FrameCount;
        }

        public void Draw(SpriteBatch sprites) {
            sprites.Draw(Texture, Position, viewRect, Color.White,
                Rotation, Origin, Scale, SpriteEffects.None, 0);
        }

        public void Update(GameTime gt) {
            if(Animate) frame = (frame + 1) % totalFrames; //auto change frames
            viewRect.X = viewRect.Width * frame; //update viewRect to match frame
        }
    }
}
