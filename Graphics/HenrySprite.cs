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

        //data members
        public ContentManager cm;
        public Color Color = Color.White;
        public Vector2 Position = new Vector2(0, 0);
        public Vector2 Origin = new Vector2(0, 0);
        protected Texture2D Texture;
        public Vector2 Size = new Vector2(0, 0);
        private float scale = 1.0f, hitRadius;
        public float Rotation = 0.0f;
        public bool Animate = false, Oscillate = false, Loop = false, ManageHitRadius = false, Visible = true;
        private bool reverseFrames = false;
        private int frame = 0, totalFrames;
        protected Rectangle viewRect = new Rectangle(0, 0, 0, 0);
        private float frameSpeed = 20.0f; //sprite frames to display per second
        private float timeSinceLastFrameChange = 0.0f;
        private string currentSprite = "";
        public float Scale {
            get { return scale; }
            set {
                if(value > 0) scale = value;
                Size.X = (viewRect.Width * scale);
                Size.Y = (viewRect.Height * scale);
                if(ManageHitRadius) HitRadius = CalcHitRadius();
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



        //functions
        public HenrySprite(ContentManager cm) {
            this.cm = cm;
            RNSEB.Win.ClientSizeChanged += new EventHandler<EventArgs>(ResolutionChange);
        }

        private void ResolutionChange(object sender, EventArgs e) {
            GameWindow window = (GameWindow)sender;
            Position *= (new Vector2(window.ClientBounds.Width, window.ClientBounds.Height)) / RNSEB.RESOLUTION;
            Scale *= window.ClientBounds.Width / RNSEB.RESOLUTION.X;
        }

        public void CenterOrigin() {
            Origin = new Vector2(viewRect.Width/2.0f, viewRect.Height/2.0f);
        }

        public void LoadContent(string spriteName, bool loop, float frameSpeed = 20.0f) {
            if((spriteName == currentSprite) || (!Loop && Animate)) return;
            SpriteInfo s = RNSEB.HenrySprites.All[spriteName];
            Texture = cm.Load<Texture2D>(s.Filename);
            currentSprite = spriteName;
            Size.X = (int)(s.Width * scale);
            Size.Y = (int)(s.Height * scale);
            if(ManageHitRadius) HitRadius = CalcHitRadius();
            viewRect.Width = s.Width;
            viewRect.Height = s.Height;
            viewRect.Y = s.Y;
            totalFrames = s.FrameCount;
            Loop = loop;
            FrameSpeed = frameSpeed;
            Frame = 0; //start new animation on first frame (doesn't seem to be working properly)
        }

        public void Draw(SpriteBatch sb) {
            if (Visible)
                sb.Draw(Texture, Position, viewRect, Color,
                    Rotation, Origin, Scale, SpriteEffects.None, 0);
        }
        
        public void Update(GameTime gt) {
            if (Loop || Frame < TotalFrames - 1)
                Animate = true;
            else
                Animate = false;

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
                    
                viewRect.X = viewRect.Width * frame; //update viewRect to match frame
            }
        }

        public bool Collision(HenrySprite that){
            if (Math.Sqrt(Math.Pow(that.Position.X - this.Position.X, 2) + Math.Pow(that.Position.Y - this.Position.Y, 2)) < this.HitRadius + that.HitRadius)
                return true;
            else
                return false;
        }

        private float CalcHitRadius(){
            if (Size.X >= Size.Y)
                return Size.Y / 2.0f;
            else
                return Size.X / 2.0f;
        }
    }
}