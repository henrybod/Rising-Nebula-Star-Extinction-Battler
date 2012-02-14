using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Hook.Graphics;
using teamstairwell.Graphics.SpriteSheets;
using Microsoft.Xna.Framework.Graphics;
using Hook.Graphics.SpriteSheets;

namespace teamstairwell.Interface
{
    public class StoryOverlay : Screen
    {
        //public TextSprite TehStory;
        public StoryOverlay() : base()
        {
            MenuBack = new Sprite(RNSEB.TitleBackground, TitleBackground.TITLEBACKGROUND);
            //TehStory = new TextSprite("In the far future, your race has been at war with \"the others\" for thousands of years. \n After ravaging every habitable planet in the galaxy with the fires of war, there are no longer any planets left. \n You are the last warrior of your species. \n Although there's no real future for your people, you must fulfill your duty and finish this war. \n Defeat the last remaining member of \"the others\" and claim victory in the final war. \n The last rival awaits you.");
            //TehStory.Color = Color.White;
        }

        public void LinkButtons()
        {
            
            MenuPane MP = new MenuPane();
            Button backbutton = new Button(new Vector2(490, 320), "BACK TO MENU", ScreenManage.MainMenuScreen, back);
            backbutton.idleButton.Scale = .6f;
            MP.AddMenuItem(backbutton);
            Panes.Add(MP);
        }
    }
}
