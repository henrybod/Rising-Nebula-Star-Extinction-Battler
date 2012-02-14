using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hook.Graphics;
using Hook.Graphics.SpriteSheets;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace teamstairwell.Interface
{
    public class ScreenManage
    {
        public Stack<Screen> ScreenStack;

        public static MainMenu MainMenuScreen = new MainMenu();
        public static InGameOverlay GameScreen = new InGameOverlay();
        public static HardOverlay HardGame = new HardOverlay();
        public static StoryOverlay StoryScreen = new StoryOverlay();
        public static InstructionOverlay InstructionScreen = new InstructionOverlay();
        public static Instructiontwo InstructTwo = new Instructiontwo();
        public static ExitOut ExitScreen = new ExitOut();
        public static PlayerTwoVictory TwoVictory = new PlayerTwoVictory();
        public static BackScreen bScreen = new BackScreen();
        public static Credits Credits = new Credits();

        public Sprite MouseCursor;
        public TextSprite TehStory;
         

        bool keepgoing;
       

        public ScreenManage()
        {
            TehStory = new TextSprite(" In the far future, your race has been at war with \"notus\" for thousands \n of years. After ravaging every habitable planet in the galaxy with the \n fires of war, there are no longer any planets left. You are the last \n warrior of your species. Although there's no real future for your \n people, you must fulfill your duty and finish this war. Defeat the \n last remaining member of \"notus\" and claim victory in the final war.\n\n                       The last rival awaits you.");
            TehStory.Color = Color.White;
            TehStory.Scale = 1.5f; 
            TehStory.Origin = new Vector3(400,170,0);

            ScreenStack = new Stack<Screen>();
            MainMenuScreen.LinkButtons();
            InstructionScreen.LinkButtons();
            InstructTwo.LinkButtons();
            StoryScreen.LinkButtons();
            TwoVictory.LinkButtons();
            Credits.LinkButtons();

            keepgoing = true;

            MouseCursor = new Sprite(RNSEB.PropSheet, PropSheet.DOOMCURSOR);
            MouseCursor.Origin = Vector3.Zero;

            ScreenStack.Push(MainMenuScreen);
        }

        public void Update(GameTime gt)
        {
            
            Screen CurrentScreen = ScreenStack.Peek();
            
            if (GameScreen.Env.isOver == true || HardGame.Env.isOver == true)
            {
                keepgoing = false;
                CurrentScreen.nScreen = TwoVictory;
                ScreenStack.Push(CurrentScreen.nScreen);
                CurrentScreen.nScreen = ExitScreen;

                if(CurrentScreen is PlayerTwoVictory)
                    ((PlayerTwoVictory) CurrentScreen).Update(gt);
                

            }
            else if (CurrentScreen.nScreen == bScreen)
            {
                ScreenStack.Pop();
            }
            else if (CurrentScreen.nScreen != null)
            {
                ScreenStack.Push(CurrentScreen.nScreen);
                CurrentScreen.nScreen = null;
            }

            CurrentScreen = ScreenStack.Peek();

            if (CurrentScreen is MainMenu)
            {
                ((MainMenu)CurrentScreen).Update(gt);  
            }
            if (CurrentScreen is Credits)
            {
                ((Credits)CurrentScreen).Update(gt); 
            }
            else if (CurrentScreen is InGameOverlay)
            {
                ((InGameOverlay)CurrentScreen).Update(gt);
            }
            else if (CurrentScreen is HardOverlay)
            {
                ((HardOverlay)CurrentScreen).Update(gt);
            }
            else if (CurrentScreen is StoryOverlay)
            {
                TehStory.Update(gt);
                ((StoryOverlay)CurrentScreen).Update(gt);
            }
            else if (CurrentScreen is InstructionOverlay)
            {
                ((InstructionOverlay)CurrentScreen).Update(gt);
            }
            else if (CurrentScreen is Instructiontwo)
            {
                ((Instructiontwo)CurrentScreen).Update(gt);
            }
            else if (CurrentScreen is ExitOut)
            {
                Environment.Exit(0);
            }
            else
            {

                CurrentScreen.Update(gt);
            }

            MouseCursor.Position = new Vector3((Mouse.GetState().X - RNSEB.RESOLUTION.X / 2), (Mouse.GetState().Y - RNSEB.RESOLUTION.Y / 2), 0);
            MouseCursor.Update(gt);
        }

        public void Draw(SpriteBatch sb)
        {
            Screen CurrentScreen = ScreenStack.Peek();
            if (CurrentScreen is Credits)
            {
                ((Credits)CurrentScreen).Draw(sb);
            }
            if (CurrentScreen is PlayerTwoVictory)
            {
                ((PlayerTwoVictory)CurrentScreen).Draw(sb);
            }
            
            else if (CurrentScreen is InGameOverlay)
            {
                ((InGameOverlay)CurrentScreen).Draw(sb);
            }
            else if (CurrentScreen is HardOverlay)
            {
                ((HardOverlay)CurrentScreen).Draw(sb);
            }

            else if (CurrentScreen is StoryOverlay)
            {
                ((StoryOverlay)CurrentScreen).Draw(sb);
                 TehStory.Draw(sb);
            }
            else if (CurrentScreen is InstructionOverlay)
            {
                ((InstructionOverlay)CurrentScreen).Draw(sb);
            }
            else if (CurrentScreen is Instructiontwo)
            {
                ((Instructiontwo)CurrentScreen).Draw(sb);
            }
            else
            {
                CurrentScreen.Draw(sb);
            }
            MouseCursor.Draw(sb);
        }

    }
}
