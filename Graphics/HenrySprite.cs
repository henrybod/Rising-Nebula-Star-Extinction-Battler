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
        public bool Animate = false, Oscillate = false;
        private bool reverseFrames = false;
        private int frame = 0, totalFrames;
        private Rectangle viewRect = new Rectangle(0, 0, 0, 0);
        private float frameSpeed = 20.0f; //sprite frames to display per second
        private float timeSinceLastFrameChange = 0.0f;

        public float Scale {
            get { return scale; }
            set {
                scale = value;
                Size.X = (int)(viewRect.Width * scale);
                Size.Y = (int)(viewRect.Height * scale);
            }
        }

        public float FrameSpeed {
            get { return frameSpeed; }
            set { if(value > 0) frameSpeed = value; }
        }

        protected int Frame {
            get { return frame; }
            set { if (value >= 0) frame = value; }
        }

        protected int TotalFrames {
            get { return totalFrames; }
            set { if (value > 0) frameSpeed = value; }
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
            if (Animate) { //automatically change frames
                timeSinceLastFrameChange += (float)gt.ElapsedGameTime.TotalSeconds;
                if(timeSinceLastFrameChange >= 1 / frameSpeed){
                    timeSinceLastFrameChange = 0;
                    if(Oscillate){
                        if(!reverseFrames && frame >= totalFrames - 1)
                            reverseFrames = true;
                        else if(reverseFrames && frame <= 0)
                            reverseFrames = false;
                        if (reverseFrames)
                            frame--;
                        else
                            frame++;
                    }else
                        frame = (frame + 1) % totalFrames;
                }
            }
            viewRect.X = viewRect.Width * frame; //update viewRect to match frame (this is outside the loop so frame may be updated manually by a child obj)
        }
    }
}