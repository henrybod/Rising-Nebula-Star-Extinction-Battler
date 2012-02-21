using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Hook.Graphics.SpriteSheets;
using teamstairwell.Interface;
using teamstairwell.Graphics.SpriteSheets;
using teamstairwell.Audio;

namespace teamstairwell{

    public class RNSEB : Microsoft.Xna.Framework.Game {

        //XNA objects for managing content
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Henry Stuff (just testin')
        bool HenryMode = true;
        HenryMenu MainMenu, Credits, UpgradeMenu;
        HenryBattlefield TheBattlefield;
        HenryMouse TheMouse;
        public enum HenryScreen {
            MainMenu,
            Battlefield,
            UpgradeMenu,
            PauseMenu,
            Credits,
            LoadSaveMenu,
            HowToPlay,
            Exit,
            Null
        };
        public static HenryScreen PreviousScreen, CurrentScreen = HenryScreen.MainMenu; //start by displaying the main menu
        public static HenrySpriteSheets HenrySprites; //a container for all spritedom
        public static SpriteFont ButtonFont, TitleFont, TextFont;
        public static HenryMediaPlayer Audio;

        //static data members
        public static Vector2 RESOLUTION = new Vector2(1200, 750);
        public static bool GUMBALLMODE = false;
        public static Background Background;
        public static GumBackground GumBackground;
        public static TitleBackground TitleBackground;
        public static PrimitiveSheet PrimitiveSheet;
        public static EnvironmentSheet EnvironmentSheet;
        public static PropSheet PropSheet;
        public static PanelSheet PanelSheet;
        public static PlayerSheet PlayerSheet;
        public static ParticlePlayerSheet ParticlePlayerSheet;
        public static EffectSheet EffectSheet;
        public static ScreenBomb DaBomb;
        public static TitleSheet TitleSheet;
        public static InfoOne InfoOne;
        public static InfoTwo InfoTwo;

        public static teamstairwell.Graphics.SpriteSheets.Credits creditp;

        public static Dictionary<string, SoundEffect> SoundEffs; //hash table to reference sound effects
        public static Dictionary<string, Song> Music; //hash table to reference music

        public static SpriteFont GameFont;

        public ScreenManage ScreenManager;




        //functions
        public RNSEB() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (int)RESOLUTION.X;
            graphics.PreferredBackBufferHeight = (int)RESOLUTION.Y;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            MediaPlayer.Volume = 0.1f;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            if(HenryMode){
                TheMouse = new HenryMouse();
                HenrySprites = new HenrySpriteSheets();
                Audio = new HenryMediaPlayer(this.Content);
            }

            base.Initialize();
        }

        protected override void LoadContent(){

            // TODO: use this.Content to load your game content here
            GameFont = this.Content.Load<SpriteFont>("GameFont");

            if(!HenryMode){
                PrimitiveSheet = new PrimitiveSheet(Content.Load<Texture2D>("Spritesheets/PrimitiveSheet"));
                PropSheet = new PropSheet(Content.Load<Texture2D>("Spritesheets/PropSheet"));
                PanelSheet = new PanelSheet(Content.Load<Texture2D>("Spritesheets/PanelSheet"));
                EnvironmentSheet = new EnvironmentSheet(Content.Load<Texture2D>("Spritesheets/EnvironmentSheet"));
                PlayerSheet = new PlayerSheet(Content.Load<Texture2D>("Spritesheets/PlayerSheet"));
                ParticlePlayerSheet = new ParticlePlayerSheet(Content.Load<Texture2D>("Spritesheets/PlayerSheet"));
                //EffectSheet = new EffectSheet(Content.Load<Texture2D>("Spritesheets/EffectSheet"));
                Background = new Background(Content.Load<Texture2D>("Spritesheets/Background"));
                GumBackground = new GumBackground(Content.Load<Texture2D>("Spritesheets/Gumball Background"));
                TitleBackground = new TitleBackground(Content.Load<Texture2D>("Spritesheets/TitleBackground"));
                DaBomb = new ScreenBomb(Content.Load<Texture2D>("Spritesheets/BombAnimSheet"));
                TitleSheet = new TitleSheet(Content.Load<Texture2D>("Spritesheets/TitleSheet"));
                InfoOne = new InfoOne(Content.Load<Texture2D>("Spritesheets/InfoOne"));
                InfoTwo = new InfoTwo(Content.Load<Texture2D>("Spritesheets/InfoTwo"));
                creditp = new teamstairwell.Graphics.SpriteSheets.Credits(Content.Load<Texture2D>("Spritesheets/Credits"));

                Music = new Dictionary<string, Song>();
                Music.Add("ForestMusic", Content.Load<Song>("Audio/Music/Forest Theme"));
                Music.Add("ForestBoss", Content.Load<Song>("Audio/Music/Forest Boss"));
                Music.Add("MenuMusic", Content.Load<Song>("Audio/Music/teamstairwell Intro"));
                Music.Add("GameMusic", Content.Load<Song>("Audio/Music/teamstairwell Theme"));

                SoundEffs = new Dictionary<string, SoundEffect>();
                SoundEffs.Add("MissleSound", Content.Load<SoundEffect>("Audio/SoundEffects/Missle"));
                SoundEffs.Add("MegaDead", Content.Load<SoundEffect>("Audio/SoundEffects/MEGAMAN!"));
                SoundEffs.Add("Feeblehuman", Content.Load<SoundEffect>("Audio/SoundEffects/Feeble Human Quote"));

                ScreenManager = new ScreenManage();

            }else{
                //Henry Stuff
                //general setup
                MainMenu = new HenryMenu(this.Content, "MenuBackground");
                UpgradeMenu = new HenryMenu(this.Content, "MenuBackground");
                Credits = new HenryMenu(this.Content, "MenuBackground");
                ButtonFont = this.Content.Load<SpriteFont>("ButtonFont");
                ButtonFont.LineSpacing = 20;
                TitleFont = this.Content.Load<SpriteFont>("TitleFont");
                TextFont = this.Content.Load<SpriteFont>("TextFont");
                TheMouse.LoadContent(this.Content, "Cursor");
                Audio.LoadContent();
                
                //create main menu
                MainMenu.AddButton(0.3f, 0.55f, "Single\nPlayer", HenryScreen.Battlefield);
                MainMenu.AddButton(0.34f, 0.73f, "Multi-\nplayer", HenryScreen.Battlefield);
                MainMenu.AddButton(0.7f, 0.55f, "Load /\n Save", HenryScreen.LoadSaveMenu);
                MainMenu.AddButton(0.66f, 0.73f, "Quit", HenryScreen.Exit);
                MainMenu.AddButton(0.5f, 0.8f, "How to\n  Play", HenryScreen.HowToPlay);
                Color TitleColor = Color.HotPink; //should change this. change font by editing TitleFont.spritefont under content
                MainMenu.AddText(0.5f, 0.15f, TitleFont, TitleColor, "Rising Nebula Star");
                MainMenu.AddText(0.5f, 0.225f, TitleFont, TitleColor, "Extinction Battler:");
                MainMenu.AddText(0.5f, 0.3f, TitleFont, TitleColor, "The Final Sin"); //todo: find a way to center justify text
                MainMenu.AddButton(0.5f, 0.5f, "", HenryScreen.Credits, "PlayerIdle", 1.5f);
                
                //create upgrade menu
                UpgradeMenu.AddButton(0.5f, 0.75f, "Back", HenryScreen.MainMenu);

                //create credits screen
                Credits.AddText(0.25f, 0.5f, TextFont, Color.White, "Matt Groot\nIan Wilbanks\nChris Rose\nEric See\nMatt Paniagua");
                Credits.AddText(0.75f, 0.5f, TextFont, Color.White, "Henry Bodensteiner\nRyan Koym\nParker Leech\nEric See");
                Credits.AddButton(0.5f, 0.75f, "Back", HenryScreen.MainMenu);

                //create battlefield
                TheBattlefield = new HenryBattlefield(this.Content, "BattlefieldBackground");

            }
        }

        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
            // mayhaps utilization of this function will clear up our generous memory usage problem
        }

        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) //this looks like xbox to me
                this.Exit();

            if(!HenryMode){

                if (IsActive)
                    ScreenManager.Update(gameTime);

            }else{
                teamstairwell.Interface.HenryInput.Update(gameTime);
                TheMouse.Update(gameTime);
                if (PreviousScreen != CurrentScreen && CurrentScreen == RNSEB.HenryScreen.Battlefield)
                    Audio.StopMusic();
                PreviousScreen = CurrentScreen;
                switch(CurrentScreen){
                    case HenryScreen.Exit:
                        this.Exit();
                        break;
                    case HenryScreen.MainMenu:
                        MainMenu.Update(gameTime);
                        break;
                    case HenryScreen.Credits:
                        Credits.Update(gameTime);
                        break;
                    case HenryScreen.UpgradeMenu:
                        UpgradeMenu.Update(gameTime);
                        break;
                    case HenryScreen.Battlefield:
                        TheBattlefield.Update(gameTime);
                        break;
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            if(!HenryMode){
                ScreenManager.Draw(spriteBatch);
            }else{
                //Henry Stuff
                switch(CurrentScreen){
                    case HenryScreen.MainMenu:
                        MainMenu.Draw(spriteBatch);
                        break;
                    case HenryScreen.Credits:
                        Credits.Draw(spriteBatch);
                        break;
                    case HenryScreen.UpgradeMenu:
                        UpgradeMenu.Draw(spriteBatch);
                        break;
                    case HenryScreen.Battlefield:
                        TheBattlefield.Draw(spriteBatch);
                        break;
                    //more cases later
                }
                TheMouse.Draw(spriteBatch); //mouse is always the last thing drawn so that it appears on top
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}