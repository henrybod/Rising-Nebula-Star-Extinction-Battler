using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using teamstairwell;

namespace Hook.Graphics
{
    public class TextSprite
    {
        public string Text;
        public SpriteFont Font;
        public Vector3 Position, Origin, Rotation;
        public SpriteEffects SpriteEffect;
        public Color Color;
        //public int SpriteType, FrameCount, CurrentFrame, FrameWidth, FrameHeight, HeightMarker;
        //public float Timer, Interval, FrameRate, BackgroundFactor;
        public float Scale;
        //public Rectangle SourceRect;

        public TextSprite(string Text)
        {
            /*CurrentFrame = 0;
            Timer = 0;
            FrameRate = 5;
            Interval = 1000 / FrameRate;
            BackgroundFactor = 1;*/
            Scale = 1;
            SpriteEffect = SpriteEffects.None;
            Position = Rotation = Vector3.Zero;
            this.Text = Text;
            Font = RNSEB.GameFont;
            Color = Color.Black;
            Origin = new Vector3(Font.MeasureString(Text).X / 2, Font.MeasureString(Text).Y / 2, 0);

            /*HeightMarker = 0;
            for(int n = 0; n < SpriteType; n++)
            {
                HeightMarker += SpriteSheet.GetFrameHeight(n);
            }*/
            //HeightMarker++;

            /*FrameWidth = SpriteSheet.GetFrameWidth(SpriteType);
            FrameHeight = SpriteSheet.GetFrameHeight(SpriteType);
            FrameCount = SpriteSheet.GetFrameCount(SpriteType);*/
        }

        public void Update(GameTime gt)
        {
            //Check game time to determine if it is time to change the animation frame.
            /*Timer += (float)gt.ElapsedGameTime.TotalMilliseconds;

            if(FrameRate > 0)
            {
                Interval = 1000 / FrameRate;
                if(Timer > Interval)
                {
                    CurrentFrame++;
                    if(CurrentFrame >= FrameCount)
                    {
                        CurrentFrame = 0;
                    }
                    Timer = 0f;
                }
            }

            //Select the current animation frame to use.
            SourceRect = new Rectangle(CurrentFrame * FrameWidth, HeightMarker, FrameWidth, FrameHeight);
            Origin = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2);*/
        }
        
        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(Font, Text, new Vector2(Position.X + RNSEB.RESOLUTION.X / 2, Position.Y + RNSEB.RESOLUTION.Y / 2), Color, Rotation.Z, new Vector2(Origin.X, Origin.Y), Scale, SpriteEffect, 0);
        }

        public void SetFlip(bool IsFlipped)
        {
            if(IsFlipped)
                SpriteEffect = SpriteEffects.FlipHorizontally;
            else
                SpriteEffect = SpriteEffects.None;
        }
    }
}
