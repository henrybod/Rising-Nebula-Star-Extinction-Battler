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

        public ContentManager cm;
        public Vector2 Position = new Vector2(0, 0);
        public Vector2 Origin = new Vector2(0, 0);
        protected Texture2D Texture;
        public Vector2 Size = new Vector2(0, 0);
        private float scale = 1.0f, hitRadius;
        public float Rotation = 0.0f;
        public bool Animate = false, Oscillate = false, Loop = false, ManageHitRadius = false;
        private bool reverseFrames = false;
        private int frame = 0, totalFrames;
        protected Rectangle viewRect = new Rectangle(0, 0, 0, 0);
        private float frameSpeed = 20.0f; //sprite frames to display per second
        private float timeSinceLastFrameChange = 0.0f;
        private string currentSprite = "";
        public float Scale {
            get { return scale; }
            set {
                scale = value;
                Size.X = (int)(viewRect.Width * scale);
                Size.Y = (int)(viewRect.Height * scale);
                if(ManageHitRadius) HitRadius = Size.Length() / 2;
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
        public float HitRadius {
            get { return hitRadius; }
            set { hitRadius = (value <= 0) ? 0.0f : value; }
        }




        public HenrySprite(ContentManager cm){
            this.cm = cm;
        }

        public void CenterOrigin(){
            Origin = new Vector2(viewRect.Width/2.0f, viewRect.Height/2.0f);
        }

        public void LoadContent(string spriteName, bool loop, float frameSpeed = 20.0f) {
            if((spriteName == currentSprite) || (!Loop && Animate)) return;
            SpriteInfo s = RNSEB.HenrySprites.All[spriteName];
            Texture = cm.Load<Texture2D>(s.Filename);
            currentSprite = spriteName;
            Size.X = (int)(s.Width * scale);
            Size.Y = (int)(s.Height * scale);
            if(ManageHitRadius) HitRadius = Size.Length() / 2;
            viewRect.Width = s.Width;
            viewRect.Height = s.Height;
            viewRect.Y = s.Y;
            totalFrames = s.FrameCount;
            Loop = loop;
            FrameSpeed = frameSpeed;
        }

        public void Draw(SpriteBatch sprites) {
            sprites.Draw(Texture, Position, viewRect, Color.White,
                Rotation, Origin, Scale, SpriteEffects.None, 0);
        }
        
        public void Update(GameTime gt) {
            //anything else?//
            if (Loop || Frame < TotalFrames - 1)
                Animate = true;
            else
                Animate = false;
                
            //moar lojik hear

            if(Animate) ChangeFrame(gt); //automatically change frames
        }

        private void ChangeFrame(GameTime gt) {
            timeSinceLastFrameChange += (float)gt.ElapsedGameTime.TotalSeconds;

            if(timeSinceLastFrameChange >= 1 / frameSpeed){
                timeSinceLastFrameChange = 0;
                if(Oscillate) {
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
                    
                viewRect.X = viewRect.Width * frame; //update viewRect to match frame (this is outside the loop so frame may be updated manually by a child obj)
            }
        }

        public bool Collision(HenrySprite that){
            if (Math.Sqrt(Math.Pow(that.Position.X - this.Position.X, 2) + Math.Pow(that.Position.Y - this.Position.Y, 2)) < this.HitRadius + that.HitRadius)
                return true;
            else
                return false;
        }
    }
}