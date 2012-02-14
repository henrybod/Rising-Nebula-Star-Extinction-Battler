using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Hook.Graphics;
using teamstairwell.Graphics.SpriteSheets;

namespace teamstairwell.Interface
{
    public class InstructionOverlay : Screen
    {
        public InstructionOverlay() : base()
        {
            MenuBack = new Sprite(RNSEB.TitleBackground, TitleBackground.TITLEBACKGROUND);
            infosheetOne = new Sprite(RNSEB.InfoOne, InfoOne.INFOONE);
        }

        public void LinkButtons()
        {
            MenuPane MP = new MenuPane();
            Button Buttonb = (new Button(new Vector2(-500, 250), "BACK TO MAIN MENU", ScreenManage.MainMenuScreen, back));
            Buttonb.idleButton.Scale = .6f;
            MP.AddMenuItem(Buttonb);
            Button Buttono = (new Button(new Vector2(300, 250), "TO PAGE 2", ScreenManage.InstructTwo, pageTwo));
            Buttono.idleButton.Scale = .6f;
            MP.AddMenuItem(Buttono); 
            Panes.Add(MP); 
        }
    }
}
