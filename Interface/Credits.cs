using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hook.Graphics;
using teamstairwell.Graphics.SpriteSheets;
using Microsoft.Xna.Framework;

namespace teamstairwell.Interface
{
    public class Credits : Screen
    {
        public Credits() : base()
        {
            MenuBack = new Sprite(Game1.TitleBackground, TitleBackground.TITLEBACKGROUND);
            Creditpic = new Sprite(Game1.creditp, teamstairwell.Graphics.SpriteSheets.Credits.CREDIT);
        }

        public void LinkButtons()
        {
            MenuPane MP = new MenuPane();
            Button Buttonb = (new Button(new Vector2(0, 350), "BACK TO MAIN MENU", ScreenManage.MainMenuScreen, back));
            Buttonb.idleButton.Scale = .6f;
            MP.AddMenuItem(Buttonb);
            Panes.Add(MP); 
        }
    }
}
