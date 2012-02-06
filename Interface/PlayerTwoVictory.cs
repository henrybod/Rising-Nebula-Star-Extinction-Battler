using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hook.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using teamstairwell.Graphics.SpriteSheets;

namespace teamstairwell.Interface
{
    public class PlayerTwoVictory : Screen
    {
        TextSprite PTVictory = new TextSprite(" You are the victor. As you see your opponent engufled in flames \n and turn into dust, you are now assured that your species was \n superior. Though there is no one left to celebrate with, you have \n proven yourself worthy of the privilege of being the last remaining \n                      life form in the galaxy.");
        TextSprite ChallengeAgain = new TextSprite("NO MORE CHALLENGES!");
        public PlayerTwoVictory()
            : base()
        {
            PTVictory.Color = Color.White;
            ChallengeAgain.Color = Color.White;
            ChallengeAgain.Scale = 1.5f;
            PTVictory.Scale = 1.5f;
            PTVictory.Origin = new Vector3(370, 150, 0);
            ChallengeAgain.Origin = new Vector3(100, -10, 0);
            MenuBack = new Sprite(Game1.TitleBackground, TitleBackground.TITLEBACKGROUND);
        }

        public void Update(GameTime gt)
        {
            PTVictory.Update(gt);
            ChallengeAgain.Update(gt);
            base.Update(gt);
        }

        public void Draw(SpriteBatch sb)
        {
            
            base.Draw(sb);
            PTVictory.Draw(sb);
            ChallengeAgain.Draw(sb);
        }

        public void LinkButtons()
        {
            MenuPane MP = new MenuPane();

            Button Buttono = (new Button(new Vector2(0, 300), "THANK YOU FOR PLAYING!", ScreenManage.ExitScreen,exit));
            Buttono.idleButton.Scale = .6f;
            MP.AddMenuItem(Buttono);

            Panes.Add(MP);
        }

    }
}
