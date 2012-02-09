using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Hook.Graphics;
using teamstairwell.Graphics.SpriteSheets;

namespace teamstairwell.Interface
{
    public class Instructiontwo : Screen
    {
        public Instructiontwo()
            : base()
        {
            MenuBack = new Sprite(RNSEB.TitleBackground, TitleBackground.TITLEBACKGROUND);
            infosheetTwo = new Sprite(RNSEB.InfoTwo, InfoTwo.INFOTWO);
        }

        public void LinkButtons()
        {
            MenuPane MP = new MenuPane();
            Button BO = (new Button(new Vector2(-500, 250), "BACK TO PAGE ONE", ScreenManage.InstructionScreen, pageOne));
            BO.idleButton.Scale=.6f;
            MP.AddMenuItem(BO);
            Button BB = (new Button(new Vector2(500, 250), "BACK TO MENU", ScreenManage.MainMenuScreen, back));
            BB.idleButton.Scale = .6f;
            MP.AddMenuItem(BB);
            Panes.Add(MP);
        }
    }
}