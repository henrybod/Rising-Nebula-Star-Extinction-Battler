using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using teamstairwell;

namespace Hook.Graphics
{
    public class Sprite
    {
        public string Name;
        public SpriteSheet SpriteSheet;
        public SpriteEffects SpriteEffect;
        public Vector3 Position, Origin, Rotation;
        public int SpriteType, FrameCount, CurrentFrame, FrameWidth, FrameHeight, HeightMarker;
        public float Timer, Interval, FrameRate, Scale, BackgroundFactor;
        public Rectangle SourceRect;

        public Sprite(string FileName)
        {
            Name = FileName;
            LoadSpriteData(FileName);
        }

        public Sprite(SpriteSheet ss, int SpriteType)
        {
            SpriteSheet = ss;
            this.SpriteType = SpriteType;

            SetSpriteDefaults();
        }

        public void Update(GameTime gt)
        {
            //Check game time to determine if it is time to change the animation frame.
            Timer += (float)gt.ElapsedGameTime.TotalMilliseconds;

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
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(SpriteSheet.SheetImage, new Vector2(Position.X, Position.Y) + Game1.RESOLUTION / 2, SourceRect, Color.White, Rotation.Z, new Vector2(Origin.X, Origin.Y), Scale, SpriteEffect, 0);
        }

        public void Draw(SpriteBatch sb, Camera Cam)
        {
            sb.Draw(SpriteSheet.SheetImage, new Vector2((Position - Cam.Position).X, (Position - Cam.Position).Y) * Cam.Zoom * BackgroundFactor + Game1.RESOLUTION / 2, SourceRect, Color.White, Rotation.Z, new Vector2(Origin.X, Origin.Y), Scale * Cam.Zoom, SpriteEffect, 0);
        }

        public void Draw(SpriteBatch sb, float angle)
        {
            sb.Draw(SpriteSheet.SheetImage, new Vector2(Position.X, Position.Y) + Game1.RESOLUTION / 2, SourceRect, Color.White, angle, new Vector2(Origin.X, Origin.Y), Scale, SpriteEffect, 0);
        }

        public void SetType(int SpriteType)
        {
            this.SpriteType = SpriteType;
            CurrentFrame = 0;
        }

        public void SetFlip(bool IsFlipped)
        {
            if(IsFlipped)
                SpriteEffect = SpriteEffects.FlipHorizontally;
            else
                SpriteEffect = SpriteEffects.None;
        }

        /// <summary>
        /// Load sprite variables from designated text file.
        /// Frame height, width, count
        /// Origin offset, framerate, 
        /// </summary>
        public void LoadSpriteData(string FileName)
        {
            //StorageContainer Container = Device.OpenContainer("Props");

            // Get the path of the save game.
            //FileName = Path.Combine(Container.Path, FileName);
            FileName = "Sprites/" + FileName;
            
            if (File.Exists(FileName))
            {
                List<string> Lines = new List<string>();
                StreamReader SR = new StreamReader(FileName);

                string CurrentLine = SR.ReadLine();
                while (CurrentLine != null)
                {
                    Lines.Add(CurrentLine);
                    CurrentLine = SR.ReadLine();
                }

                //SpriteSheet.SheetImage = Game1.Content.Load<Texture2D>(FileName);

                for (int s = 0; s < Lines.Count; s++)
                {
                    if (Lines[s].StartsWith("SpriteSheet = "))
                    {
                        string Sheet = Lines[s].Substring("SpriteSheet = ".Length);

                        switch (Sheet)
                        {
                            case "EffectSheet": SpriteSheet = Game1.EffectSheet; break;
                            case "EnvironmentSheet": SpriteSheet = Game1.EnvironmentSheet; break;
                            case "PanelSheet": SpriteSheet = Game1.PanelSheet; break;
                            case "ParticlePlayerSheet": SpriteSheet = Game1.ParticlePlayerSheet; break;
                            case "PlayerSheet": SpriteSheet = Game1.PlayerSheet; break;
                            case "PrimitiveSheet": SpriteSheet = Game1.PrimitiveSheet; break;
                            case "PropSheet": SpriteSheet = Game1.PropSheet; break;
                        }
                    }
                    else if (Lines[s].StartsWith("SpriteType = "))
                    {
                        SpriteType = int.Parse(Lines[s].Substring("SpriteType = ".Length));
                        SetSpriteDefaults();
                    }
                    else if (Lines[s].StartsWith("FrameRate = "))
                    {
                        FrameRate = int.Parse(Lines[s].Substring("FrameRate = ".Length));
                    }
                    else if (Lines[s].StartsWith("OriginOffset = "))
                    {
                        Vector3 Offset = new Vector3();
                        int XLoc, YLoc, ZLoc;
                        XLoc = Lines[s].IndexOf('X');
                        YLoc = Lines[s].IndexOf('Y');
                        ZLoc = Lines[s].IndexOf('Z');
                        Offset.X = float.Parse(Lines[s].Substring(XLoc + 2, YLoc - XLoc - 3));
                        Offset.Y = float.Parse(Lines[s].Substring(YLoc + 2, ZLoc - YLoc - 3));
                        Offset.Z = float.Parse(Lines[s].Substring(ZLoc + 2, Lines[s].Length - ZLoc - 3));
                        Origin = Offset;
                    }
                }
            }
            else
            {
                LoadSpriteData("NullSprite.txt");
                Name = "NullSprite.txt";
            }
        }

        public void SetSpriteDefaults()
        {
            CurrentFrame = 0;
            Timer = 0;
            FrameRate = 5;
            Interval = 1000 / FrameRate;
            Scale = 1;
            BackgroundFactor = 1;
            Rotation = Vector3.Zero;
            SpriteEffect = SpriteEffects.None;
            Position = Vector3.Zero;

            HeightMarker = 0;
            for (int n = 0; n < SpriteType; n++)
            {
                HeightMarker += SpriteSheet.GetFrameHeight(n);
            }
            //HeightMarker++;

            FrameWidth = SpriteSheet.GetFrameWidth(SpriteType);
            FrameHeight = SpriteSheet.GetFrameHeight(SpriteType);
            FrameCount = SpriteSheet.GetFrameCount(SpriteType);

            SourceRect = new Rectangle(CurrentFrame * FrameWidth, HeightMarker, FrameWidth, FrameHeight);
            Origin = new Vector3(SourceRect.Width / 2, SourceRect.Height / 2, 0);
        }
    }
}
