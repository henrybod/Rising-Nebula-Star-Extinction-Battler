using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using teamstairwell.Graphics.SpriteSheets;
using Hook.Graphics;
using Microsoft.Xna.Framework.Media;

namespace teamstairwell.Interface
{
    public class MainMenu : Screen
    {

        public MainMenu() : base()
        {
            MediaPlayer.Play(Game1.Music["MenuMusic"]);
            MediaPlayer.Volume = 1.5f;
            MediaPlayer.IsRepeating = true;
            MenuBack = new Sprite(Game1.TitleBackground, TitleBackground.TITLEBACKGROUND);
            titleSheet = new Sprite(Game1.TitleSheet, TitleSheet.TITLESHEET);
            Creditword = new TextSprite("Credits \n Ship!");
        }
        public void LinkButtons()
        {
            MenuPane MP = new MenuPane();
            MP.AddMenuItem(new Button(new Vector2(0, 180), "CREDITS", ScreenManage.Credits, pship));

            Button Buttonh = (new Button(new Vector2(0, 300), "PLAY HOPELESS", ScreenManage.HardGame, hopeless));
            Buttonh.idleButton.Scale = .6f;
            MP.AddMenuItem(Buttonh);
            
            Button Buttong = (new Button(new Vector2(-200, 100), "PLAY THE GAME", ScreenManage.GameScreen, play));
            Buttong.idleButton.Scale = .6f;
            MP.AddMenuItem(Buttong);

            Button Buttoni = (new Button(new Vector2(-200, 269), "HOW TO PLAY", ScreenManage.InstructionScreen, instructions));
            Buttoni.idleButton.Scale = .6f;
            MP.AddMenuItem(Buttoni);

            Button Buttonst = (new Button(new Vector2(200, 100), "THE STORY", ScreenManage.StoryScreen, story));
            Buttonst.idleButton.Scale = .6f;
            MP.AddMenuItem(Buttonst);

            Button Buttone = (new Button(new Vector2(200, 269), "EXIT", ScreenManage.ExitScreen, exit));
            Buttone.idleButton.Scale = .6f;
            MP.AddMenuItem(Buttone);
            Panes.Add(MP);
        }


    }
}
