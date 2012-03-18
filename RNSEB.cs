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
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;


namespace teamstairwell{
  [Serializable]
        public struct SaveGameData
        {
            public HenryPlayer Notus;
            public HenryBoss Zihao;
            public List<HenryBullet> dBs;
        }
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
                PlayerUpgradeMenu.AddText(0.5f, 0.1f, TitleFont, Color.White, "Upgrades");
                PlayerUpgradeMenu.AddButton(0.9f, 0.9f, "Done", "Battlefield");

                //create pause menu

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
       
        

        IAsyncResult result;
        Object stateobj;
        bool GameSaveRequested = false;
        GamePadState currentState;


        protected override void Update(GameTime gameTime) {
             GamePadState previousState = currentState;
            
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) //this looks like xbox to me
                this.Exit();
              // Allows the default game to exit on Xbox 360 and Windows
        
        if ((currentState.Buttons.A == ButtonState.Pressed) &&
                (previousState.Buttons.A == ButtonState.Released))
            {
                // Set the request flag
                if ((!Guide.IsVisible) && (GameSaveRequested == false))
                {
                    GameSaveRequested = true;
                    result = StorageDevice.BeginShowSelector(
                            PlayerIndex.One, null, null);
                }
            }

            if ((currentState.Buttons.B == ButtonState.Pressed) &&
                (previousState.Buttons.B == ButtonState.Released))
            {
                if (!Guide.IsVisible)
                {
                    // Reset the device
                    device = null;                   
                    stateobj = (Object)"GetDevice for Player One";
                    StorageDevice.BeginShowSelector(
                            PlayerIndex.One, this.GetDevice, stateobj);
                }
            }
            // If a save is pending, save as soon as the
            // storage device is chosen
            if ((GameSaveRequested) && (result.IsCompleted))
            {
                StorageDevice device = StorageDevice.EndShowSelector(result);
                if (device != null && device.IsConnected)
                {
                    DoSaveGame(device);
                   
                }
                // Reset the request flag
                GameSaveRequested = false;
            }
        

       
       

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
        
        StorageDevice device;
        private static HenryPlayer Notus;
        private static HenryBoss Zihao;
        private static List<HenryBullet> dBs;
        void GetDevice(IAsyncResult result)
        {
            device = StorageDevice.EndShowSelector(result);
            if (device != null && device.IsConnected)
            {
                DoSaveGame(device);
                //DoLoadGame(device);
                //DoCreate(device);
                //DoOpen(device);   //obviously we do not want to do all at once this is just to demonstrate the syntax
                //DoCopy(device);
                //DoEnumerate(device);
                //DoRename(device);
                //DoDelete(device);
                //DoOpenFile();
            }
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
    
 /// <summary>
        /// This method serializes a data object into
        /// the StorageContainer for this game.
        /// </summary>
        /// <param name="device"></param>
        private static void DoSaveGame(StorageDevice device)
        {

            // Create the data to save.
            SaveGameData data = new SaveGameData();
            data.Notus = Notus;
            data.Zihao = Zihao;
            data.dBs = dBs;

            // Open a storage container.
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

           string filename = "savegame.sav";

           // Check to see whether the save exists.
           if (container.FileExists(filename))
              // Delete it so that we can create one fresh.
              container.DeleteFile(filename);

           // Create the file.
           Stream stream = container.CreateFile(filename);

           // Convert the object to XML data and put it in the stream.
           XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
           serializer.Serialize(stream, data);

           // Close the file.
           stream.Close();

           // Dispose the container, to commit changes.
           container.Dispose();
        }
        /// <summary>
        /// This method loads a serialized data object
        /// from the StorageContainer for this game.
        /// </summary>
        /// <param name="device"></param>
        private static void DoLoadGame(StorageDevice device)
        {
            // Open a storage container.
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            string filename = "savegame.sav";

            // Check to see whether the save exists.
            if (!container.FileExists(filename))
            {
               // If not, dispose of the container and return.
               container.Dispose();
               return;
            }

            // Open the file.
            Stream stream = container.OpenFile(filename, FileMode.Open);

            // Read the data from the file.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            SaveGameData data = (SaveGameData)serializer.Deserialize(stream);

            // Close the file.
            stream.Close();

            // Dispose the container.
            container.Dispose();
           

            // Report the data to the console.
            Debug.WriteLine("Name:     " + data.Notus.ToString());
            Debug.WriteLine("Level:    " + data.Zihao.ToString());
            Debug.WriteLine("Score:    " + data.dBs.ToString());
           
        }
        /// <summary>
        /// This method creates a file called demobinary.sav and places
        /// it in the StorageContainer for this game.
        /// </summary>
        /// <param name="device"></param>
        private static void DoCreate(StorageDevice device)
        {
            // Open a storage container.
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Add the container path to our file name.
            string filename = "demobinary.sav";

            // Create a new file.
            if (!container.FileExists(filename))
            {
               Stream file = container.CreateFile(filename);
               file.Close();
            }
            // Dispose the container, to commit the data.
            container.Dispose();
        }
        /// <summary>
        /// This method illustrates how to open a file. It presumes
        /// that demobinary.sav has been created.
        /// </summary>
        /// <param name="device"></param>
        private static void DoOpen(StorageDevice device)
        {
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Add the container path to our file name.
            string filename = "demobinary.sav";

            Stream file = container.OpenFile(filename, FileMode.Open);
            file.Close();

            // Dispose the container.
            container.Dispose();
        }
        /// <summary>
        /// This method illustrates how to copy files.  It presumes
        /// that demobinary.sav has been created.
        /// </summary>
        /// <param name="device"></param>
        private static void DoCopy(StorageDevice device)
        {
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Add the container path to our file name.
            string filename = "demobinary.sav";
            string copyfilename = "copybinary.sav";

            if (container.FileExists(filename) && !container.FileExists(copyfilename))
            {
                Stream file = container.OpenFile(filename, FileMode.Open);
               Stream copyfile = container.CreateFile(copyfilename);
               file.CopyTo(copyfile);

               file.Close();
               copyfile.Close();
            }

            // Dispose the container, to commit the change.
            container.Dispose();
        }
        /// <summary>
        /// This method illustrates how to rename files.  It presumes
        /// that demobinary.sav has been created.
        /// </summary>
        /// <param name="device"></param>
        private static void DoRename(StorageDevice device)
        {
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Add the container path to our file name.
            string oldfilename = "demobinary.sav";
            string newfilename = "renamebinary.sav";

            if (container.FileExists(oldfilename) && !container.FileExists(newfilename))
            {
               Stream oldfile = container.OpenFile(oldfilename, FileMode.Open);
               Stream newfile = container.CreateFile(newfilename);
               oldfile.CopyTo(newfile);

               oldfile.Close();
               newfile.Close();
               container.DeleteFile(oldfilename);
            }

            // Dispose the container, to commit the change.
            container.Dispose();
        }
        /// <summary>
        /// This method illustrates how to enumerate files in a 
        /// StorageContainer.
        /// </summary>
        /// <param name="device"></param>
        private static void DoEnumerate(StorageDevice device)
        {
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            string[] FileList = container.GetFileNames();
            foreach (string filename in FileList)
            {
               Console.WriteLine(filename);
            }

            // Dispose the container.
            container.Dispose();
        }
        /// <summary>
        /// This method deletes a file previously created by this demo.
        /// </summary>
        /// <param name="device"></param>
        private static void DoDelete(StorageDevice device)
        {
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Add the container path to our file name.
            string filename = "demobinary.sav";

            if (container.FileExists(filename))
            {
               container.DeleteFile(filename);
            }

            // Dispose the container, to commit the change.
            container.Dispose();
        }
        /// <summary>
        /// This method opens a file using System.IO classes and the
        /// TitleLocation property.  It presumes that a file named
        /// ship.dds has been deployed alongside the game.
        /// </summary>
        private static void DoOpenFile()
        {
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream("ship.dds");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data

                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }
            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }
        }
    }
}