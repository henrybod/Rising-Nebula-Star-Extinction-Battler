using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;

namespace teamstairwell.Interface
{
    public class Button : MenuItem
    {
        public Sprite idleButton, hoverButton, pressButton; 
        public TextSprite buttonText;
        
        
        public Screen nextScreen; 

        public Button(Vector2 Position, string text, Screen nScreen, Sprite buttonFrame) :base()  
        {
            buttonText = new TextSprite(text);
            this.nextScreen = nScreen;
            this.Position = Position;
            
            buttonText.Color = Color.Transparent;
            idleButton = buttonFrame;
            Dimension = new Vector2(idleButton.FrameWidth, idleButton.FrameHeight);
            hoverButton = buttonFrame;
            hoverButton.Scale = 1.3f;
            pressButton = buttonFrame;
            Sprite = idleButton; 
        }

        public new void Update(GameTime gt)
        {
            base.Update(gt);
            buttonText.Update(gt);
        }

        public new void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            buttonText.Draw(sb); 
        }


    }
}
