using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Hook.Graphics;
using teamstairwell.Graphics.SpriteSheets;
using Microsoft.Xna.Framework.Media;
using Hook.Graphics.SpriteSheets;


namespace teamstairwell.Interface
{
    public class Screen
    {
        public const double MouseTime = .5;
        
        public Sprite MenuBack;// = new Sprite(Game1.TitleBackground, TitleBackground.TITLEBACKGROUND);
        
        public double mouseTimer;
        public int ScreenChosen;
        public Screen nScreen;
        public bool inGame;
        public Sprite titleSheet;
        public Sprite infosheetOne;
        public Sprite infosheetTwo;
        public Sprite Creditpic; 
        

        public Sprite pship = new Sprite(RNSEB.PlayerSheet, PlayerSheet.PNORMAL); 
        public Sprite play = new Sprite(RNSEB.PanelSheet, PanelSheet.NORMALMODE);
        public Sprite hopeless = new Sprite(RNSEB.PanelSheet, PanelSheet.HOPELESSMODE);
        public Sprite story = new Sprite(RNSEB.PanelSheet, PanelSheet.STORYBUTTONFRAME);
        public Sprite instructions = new Sprite(RNSEB.PanelSheet, PanelSheet.INSTRBUTTONFRAME);
        public Sprite exit = new Sprite(RNSEB.PanelSheet, PanelSheet.EXITBUTTONFRAME);
        public Sprite redbar = new Sprite(RNSEB.PanelSheet, PanelSheet.REDBAR);
        public Sprite back = new Sprite(RNSEB.PanelSheet, PanelSheet.BACKBUTTONFRAME);
        public Sprite pageOne = new Sprite(RNSEB.PanelSheet, PanelSheet.PAGEONE);
        public Sprite pageTwo = new Sprite(RNSEB.PanelSheet, PanelSheet.PAGETWO);

        public TextSprite Creditword;
            

        public List<MenuPane> Panes; 
        
        public Screen()
        {
            nScreen = null;
            mouseTimer = 0;

            Panes = new List<MenuPane>(); 
        }
        
        public void HandleInput()
        {
            foreach (MenuPane MP in Panes)
            {
                foreach (MenuItem MI in MP.PaneList)
                {
                    if (MouseInBounds(MP, MI))
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && mouseTimer == 0)
                        {
                            if (MI is Button)
                            {
                                if (this is MainMenu && ((Button)MI).buttonText.Text.Equals("PLAY THE GAME"))
                                {
                                    ScreenChosen = 0; //0 is game overlay
                                    inGame = true;
                                    Muzak(inGame);
                                }
                                else if (this is MainMenu && ((Button)MI).buttonText.Text.Equals("PLAY HOPELESS"))
                                {
                                    ScreenChosen = 0; //0 is game overlay
                                    inGame = true;
                                    Muzak(inGame);
                                }
                                else if (this is MainMenu && ((Button)MI).buttonText.Text.Equals("HOW TO PLAY"))
                                {
                                    ScreenChosen = 1; //1 is the instructions
                                }
                                else if (this is MainMenu && ((Button)MI).buttonText.Text.Equals("THE STORY"))
                                {
                                    
                                    ScreenChosen = 2; //2 is the story page
                                }
                                else if (this is StoryOverlay && ((Button)MI).buttonText.Text.Equals("BACK TO MENU"))
                                {
                                    ScreenChosen = 3; //3 is the main menu
                                }
                                else if (this is InstructionOverlay && ((Button)MI).buttonText.Text.Equals("BACK TO THE MENU"))
                                {
                                    ScreenChosen = 3; //3 iks the main menu
                                }
                                else if (this is InstructionOverlay && ((Button)MI).buttonText.Text.Equals("TO PAGE 2"))
                                {
                                    ScreenChosen = 4; //4 is instruction2 
                                }
                                else if (this is Instructiontwo && ((Button)MI).buttonText.Text.Equals("BACK TO MENU"))
                                {
                                    ScreenChosen = 3; //3 is the main menu
                                }
                                else if (this is Instructiontwo && ((Button)MI).buttonText.Text.Equals("BACK TO PAGE ONE"))
                                {
                                    ScreenChosen = 1; //1 is the the instuction
                                }

                                else if (this is PlayerTwoVictory && ((Button)MI).buttonText.Text.Equals("THANK YOU FOR PLAYING!"))
                                {
                                    Environment.Exit(0);
                                }


                                nScreen = ((Button)MI).nextScreen;

                            }
                            mouseTimer = MouseTime;
                        }
                    }
                }
            }
        }

        public void Muzak(bool ingame)
        {
            if(ingame==true)
            {
            MediaPlayer.Play(RNSEB.Music["GameMusic"]);
            MediaPlayer.Volume = 1.5f;
            MediaPlayer.IsRepeating = true;
            }
        }

        public bool MouseInBounds(MenuPane MP, MenuItem MI)
        {
            float MouseX = Mouse.GetState().X - RNSEB.RESOLUTION.X / 2;
            float MouseY = Mouse.GetState().Y - RNSEB.RESOLUTION.Y / 2;
            Vector2 Origin = MP.Position + MI.Position;

            if ((MouseX > Origin.X - MI.Dimension.X / 2) && (MouseX < Origin.X + MI.Dimension.X / 2) &&
               (MouseY > Origin.Y - MI.Dimension.Y / 2) && (MouseY < Origin.Y + MI.Dimension.Y / 2))
            {
                return true;
            }
            else
            {
                return false; 
            }
        }

        public new void Update(GameTime gt)
        {
            HandleInput();

            if (this is MainMenu)
            {
                KeyboardState kbs = Keyboard.GetState();
                if (kbs.IsKeyDown(Keys.T) && kbs.IsKeyDown(Keys.G) && kbs.IsKeyDown(Keys.K))
                {
                    RNSEB.GUMBALLMODE = true;
                    RNSEB.SoundEffs["Feeblehuman"].Play();
                }
            }

           
            if (titleSheet != null)
            {
                titleSheet.Update(gt);
            }
            if (MenuBack != null)
            {
                MenuBack.Update(gt);
                MenuBack.Rotation.Z += .0005f;
            } 
            if (Creditpic != null)
            {
                Creditpic.Update(gt);
            }
            if(mouseTimer > 0)
            {
                mouseTimer -= gt.ElapsedGameTime.TotalSeconds;
                if(mouseTimer <= 0)
                {
                    mouseTimer = 0; 
                }

            }

            foreach(MenuPane MP in Panes)
            {
                MP.Update(gt);
            }
            
        }

        public new void Draw( SpriteBatch sb)
        {
            if (MenuBack != null)
            {
                MenuBack.Draw(sb);
                
            }
            if (Creditword != null)
            {
                Creditword.Color = Color.White;
                Creditword.Scale = .8f;
                Creditword.Origin = new Vector3(40, -270, 0);
                Creditword.Draw(sb);
            }
            if (titleSheet != null)
            {
                titleSheet.Origin = new Vector3(500, 300, 0);
                titleSheet.Draw(sb);
            }
            if (infosheetOne != null)
            {
                infosheetOne.Origin = new Vector3(600, 400, 0);
                infosheetOne.Draw(sb);
            }
            if (infosheetTwo != null)
            {
                infosheetTwo.Origin = new Vector3(600, 400, 0);
                infosheetTwo.Draw(sb);
            }
            if (Creditpic != null)
            {
                Creditpic.Origin = new Vector3(600, 400, 0);
                Creditpic.Draw(sb);
            }
            foreach (MenuPane MP in Panes)
            {
                MP.Draw(sb);
            }
        }

    }
}
