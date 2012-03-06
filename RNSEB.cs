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
        HenryMouse TheMouse;
        Dictionary<string, HenryScreen> screens = new Dictionary<string, HenryScreen>();
        
        public enum HenryUpgrade {
            PlayerSuperLaser, //focused offense
            PlayerQuadLaser, //mostly focused but a little diffuse offense
            PlayerTwinRockets, //focused offense
            PlayerSpiralRockets, //diffuse offense
            PlayerDeathLotus, //diffuse offense (double spiral rockets)
            PlayerEnergyBomb, //diffuse offense
            PlayerShieldRecharge, //defense: increase rate of shield recharge
            PlayerShieldCapacity1, //defense: increase total shield capacity
            PlayerShieldCapacity2, //defense: increase total shield capacity
            PlayerShieldRecovery, //defense: decrease shield downtime
            PlayerSpeed, //increase player's maximum velocity
            BossLineSpawner, //offense: I'm not sure what all these do yet
            BossRollSpawner, //offense
            BossSplitSpawner, //offense
            BossBurstSpawner, //offense
            BossMarbleSpawner, //offense
            BossGravityWellSpawner, //offense: will be perhaps the last boss offensive upgrade
            BossAttritionField, //defense: damage the player while he is in the blue bubble
            BossAutoRepair, //defense: the boss regains health slowly
            BossPlating, //damage to boss reduced 50%
            BossEMP, //offense: (concept) manually target weapon that takes out all shields
            BossAutoTurrets //offense: give the boss some automatically targeting/firing lasers
            //feel free to add ideas
        }
        public static string PreviousScreen, CurrentScreen = "MainMenu"; //start by displaying the main menu
        public static HenrySpriteSheets HenrySprites; //a container for all spritedom
        public static SpriteFont ButtonFont, TitleFont, TextFont;
        public static HenryMediaPlayer Audio;
        public static HenryInput Input;

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
                HenrySprites = new HenrySpriteSheets();
                Audio = new HenryMediaPlayer(this.Content);
                Input = new HenryInput();
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
                screens.Add("Exit", new HenryMenu(this.Content));
                screens.Add("MainMenu", new HenryMenu(this.Content));
                screens.Add("PlayerUpgradeMenu", new HenryMenu(this.Content));
                screens.Add("BossUpgradeMenu", new HenryMenu(this.Content));
                screens.Add("SaveMenu", new HenryMenu(this.Content));
                screens.Add("LoadMenu", new HenryMenu(this.Content));
                screens.Add("PlayerVictory", new HenryMenu(this.Content));
                screens.Add("BossVictory", new HenryMenu(this.Content));
                screens.Add("Credits", new HenryMenu(this.Content));
                screens.Add("HowToPlay", new HenryMenu(this.Content));
                screens.Add("Battlefield", new HenryBattlefield(this.Content, "BattlefieldBackground"));
                screens.Add("PauseMenu", new HenryMenu(this.Content));
                ButtonFont = this.Content.Load<SpriteFont>("ButtonFont");
                ButtonFont.LineSpacing = 20;
                TitleFont = this.Content.Load<SpriteFont>("TitleFont");
                TextFont = this.Content.Load<SpriteFont>("TextFont");
                TheMouse = new HenryMouse(this.Content);
                Audio.LoadContent();
                
                //create main menu
                HenryMenu MainMenu = (HenryMenu)screens["MainMenu"];
                MainMenu.AddButton(0.3f, 0.5f, "Single\nPlayer", "Battlefield");
                MainMenu.AddButton(0.34f, 0.675f, "Multi-\nplayer", "Battlefield");
                MainMenu.AddButton(0.7f, 0.5f, "Load /\n Save", "LoadMenu");
                MainMenu.AddButton(0.66f, 0.675f, "Quit", "Exit");
                MainMenu.AddButton(0.5f, 0.75f, "How to\n  Play", "HowToPlay");
                Color TitleColor = Color.White;
                MainMenu.AddText(0.5f, 0.15f, TitleFont, TitleColor, "Rising Nebula Star");
                MainMenu.AddText(0.5f, 0.225f, TitleFont, TitleColor, "Extinction Battler:");
                MainMenu.AddText(0.5f, 0.3f, TitleFont, TitleColor, "The Final Sin"); //todo: find a way to center justify text
                MainMenu.AddButton(0.5f, 0.5f, "", "Credits", "PlayerIdle", "PlayerHit", "PlayerDeath", 1.5f);

                //create how to play screen
                HenryMenu HowToPlay = (HenryMenu)screens["HowToPlay"];
                HowToPlay.AddButton(0.9f, 0.9f, "Back", "MainMenu");
                HowToPlay.AddText(0.5f, 0.1f, TitleFont, Color.White, "The Players");

                HowToPlay.AddText(0.35f, 0.25f, TextFont, Color.White, "Zihao is the last fighter pilot\nof ... something, something ...");
                HowToPlay.AddButton(0.65f, 0.25f, "", "HowToPlayZihao", "PlayerIdle", "PlayerHit", "PlayerDeath", 1.0f);
                HowToPlay.AddText(0.65f, 0.6f, TextFont, Color.White, "*Unknown* is the last juggernaut\nof Notus ... something, something ...");
                HowToPlay.AddButton(0.30f, 0.6f, "", "HowToPlayNotus", "BossIdle", "BossHit", "BossDeath", 1.0f);

                //create battlefield
                HenryBattlefield Battlefield = (HenryBattlefield)screens["Battlefield"];
                //nothing to do here as of yet

                //create player's upgrade menu
                HenryMenu PlayerUpgradeMenu = (HenryMenu)screens["PlayerUpgradeMenu"];
                PlayerUpgradeMenu.AddText(0.5f, 0.1f, TitleFont, Color.White, "Upgrades");
                PlayerUpgradeMenu.AddButton(0.9f, 0.9f, "Done", "Battlefield");
                PlayerUpgradeMenu.AddUpgradeButton(0.5f, 0.5f, Battlefield, Content, "Skull", "SkullAura", HenryUpgrade.PlayerSuperLaser, "Gives lazer thing");
                //todo: make semi-transparent background for upgrade & pause menu

                //create boss's upgrade menu
                HenryMenu BossUpgradeMenu = (HenryMenu)screens["BossUpgradeMenu"];
                BossUpgradeMenu.AddText(0.5f, 0.1f, TitleFont, Color.White, "Upgrades");
                BossUpgradeMenu.AddButton(0.9f, 0.9f, "Done", "Battlefield");

                //create pause menu
                HenryMenu PauseMenu = (HenryMenu)screens["PauseMenu"];
                PauseMenu.AddText(0.5f, 0.1f, TitleFont, Color.White, "Paused");
                PauseMenu.AddButton(0.5f, 0.5f, "Back", "Battlefield");

                //create credits screen
                HenryMenu Credits = (HenryMenu)screens["Credits"];
                Credits.AddText(0.25f, 0.5f, TextFont, Color.White, "Matt Groot\nIan Wilbanks\nChris Rose\nEric See\nMatt Paniagua");
                Credits.AddText(0.75f, 0.5f, TextFont, Color.White, "Henry Bodensteiner\nRyan Koym\nParker Leech\nEric See");
                Credits.AddButton(0.5f, 0.75f, "Back", "MainMenu");

                //create player victory screen
                HenryMenu PlayerVictory = (HenryMenu)screens["PlayerVictory"];
                PlayerVictory.AddText(0.5f, 0.1f, TitleFont, Color.White, "Zihao is teh victor!");
                PlayerVictory.AddText(0.5f, 0.5f, TextFont, Color.White, "The inexorable time marching shall be notus revenge");
                PlayerVictory.AddButton(0.5f, 0.8f, "Main\nMenu", "MainMenu");

                //create boss victory screen
                HenryMenu BossVictory = (HenryMenu)screens["BossVictory"];
                BossVictory.AddText(0.5f, 0.1f, TitleFont, Color.White, "Notus is teh victor!");
                BossVictory.AddText(0.5f, 0.5f, TextFont, Color.White, "An infinity years of dark befall the universe");
                BossVictory.AddButton(0.5f, 0.8f, "Main\nMenu", "MainMenu");
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
                Input.Update(gameTime);
                TheMouse.Update(gameTime);
                if (CurrentScreen == "Battlefield")
                    ((HenryBattlefield)(screens[CurrentScreen])).Update(gameTime);
                else if (CurrentScreen == "Exit")
                    this.Exit();
                else
                    ((HenryMenu)(screens[CurrentScreen])).Update(gameTime);
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
                if(CurrentScreen == "Battlefield")
                    ((HenryBattlefield)(screens[CurrentScreen])).Draw(spriteBatch);
                else
                    ((HenryMenu)(screens[CurrentScreen])).Draw(spriteBatch);

                TheMouse.Draw(spriteBatch); //mouse is always the last thing drawn so that it appears on top
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}