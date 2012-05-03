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
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static ContentManager cm;

        //Multiplayer
        NetworkSession networkSession;
        AvailableNetworkSessionCollection availableSessions;
        int selectedSessionIndex;
        PacketReader packetReader = new PacketReader();
        PacketWriter packetWriter = new PacketWriter();
        GameTime current = new GameTime();
        float gameStartTimer = 2.0f;
        float elapsedTime;

        HenryMenu Lobby;
        HenryMenu ListSessions;

        //Henry Stuff (just testin')
        bool HenryMode = true;
        HenryMouse TheMouse;
        Dictionary<string, HenryScreen> screens = new Dictionary<string, HenryScreen>();
        public delegate void OnClick(); //delegate type for lambda functions passed to buttons
        public enum HenryUpgrade {
            PlayerPulsePhaser, //focused offense
            PlayerQuadLaser, //focused offense
            PlayerIonBeam, //focused offense
            PlayerShockwave, //focused/diffuse offense

            PlayerTwinRockets, //diffuse offense
            PlayerSeekers, //diffuse
            PlayerSpiralRockets, //diffuse offense
            PlayerEnergyBomb, //diffuse offense (double spiral rockets)
            PlayerDrones, //diffuse offense + defense

            PlayerShieldRecharge1, //defense: increase rate of shield recharge
            PlayerShieldCapacity1, //defense: increase total shield capacity
            PlayerShieldRecharge2, //defense: increase rate of shield recharge
            PlayerShieldCapacity2, //defense: increase total shield capacity
            PlayerShieldRecovery, //defense: decrease shield downtime
            
            BossPhotonTorpedo, //focused
            BossPlasmaTorpedo,
            BossQuantumMines,
            BossReplicator,
            
            BossRingOfFire,
            BossHorizontalPlasmaWall, //offense: send out a line of purple plasma balls
            BossVerticalPlasmaWall, //offense: send out a line of purple plasma balls
            BossHulk,
            BossMagneto,

            BossStaticField, //defense: damage the player while he is in the blue bubble
            BossKevlar, //damage to boss reduced +25%
            BossNanoregenerativeSubsystems, //defense: the boss regains health slowly
            BossTritaniumBulkheads, //damage to boss reduced +25%
            BossAutoTurrets //offense: give the boss some automatically targeting/firing lasers
            //feel free to add ideas, ya'll
        }
        public static string PreviousScreen, CurrentScreen = "MainMenu"; //start by displaying the main menu
        public static HenrySpriteSheets HenrySprites; //a container for all spritedom
        public static SpriteFont ButtonFont, TitleFont, TextFont;
        public static HenryMediaPlayer Audio; //reference for audio manager
        public static HenryInput Input; //refence for input manager
        public static HenryBattlefield TheBattlefield; //reference for other classes to access current battlefield
        public static GameWindow Win;

        //static data members
        public static Vector2 RESOLUTION = new Vector2(1200, 750);
        public static Dictionary<string, SoundEffect> SoundEffs; //hash table to reference sound effects
        public static Dictionary<string, Song> Music; //hash table to reference music

        public static SpriteFont GameFont;

        //private HenryMenu Multiplayer;

      


        //functions
        public RNSEB() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (int)RESOLUTION.X;
            graphics.PreferredBackBufferHeight = (int)RESOLUTION.Y;
            //Window.AllowUserResizing = true; //I'm giving up on this. There's just too many complications.
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
            Content.RootDirectory = "Content";
            cm = this.Content;
            Win = Window;

            //Multiplayer sign in components
            Components.Add(new GamerServicesComponent(this));
            SignedInGamer.SignedIn +=new EventHandler<SignedInEventArgs>(SignedInGamer_SignedIn);
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

            
                //Henry Stuff
                //general setup
                ButtonFont = this.Content.Load<SpriteFont>("ButtonFont");
                ButtonFont.LineSpacing = 20;
                TitleFont = this.Content.Load<SpriteFont>("TitleFont");
                TextFont = this.Content.Load<SpriteFont>("TextFont");
                TheMouse = new HenryMouse(this.Content);
                Audio.LoadContent();
                screens.Add("Exit", new HenryMenu(this.Content));
                screens.Add("MainMenu", new HenryMenu(this.Content));
                screens.Add("PlayerUpgradeMenu", new HenryScreen());
                screens.Add("BossUpgradeMenu", new HenryScreen());
                screens.Add("SaveMenu", new HenryMenu(this.Content));
                screens.Add("LoadMenu", new HenryMenu(this.Content));
                screens.Add("PlayerVictory", new HenryMenu(this.Content));
                screens.Add("BossVictory", new HenryMenu(this.Content));
                screens.Add("Credits", new HenryMenu(this.Content));
                screens.Add("HowToPlay", new HenryMenu(this.Content));
                screens.Add("Battlefield", new HenryScreen()); //note: not an actual battlefield. gets replaced by a battlefield generated on start game button click
                screens.Add("SessionOption", new HenryMenu(this.Content));
                screens.Add("Lobby", new HenryMenu(this.Content));
                screens.Add("ListSessions", new HenryMenu(this.Content));
                screens.Add("PauseMenu", new HenryMenu(this.Content));
                screens.Add("SignIn", new HenryMenu(this.Content));

                //create main menu
                HenryMenu MainMenu = (HenryMenu)screens["MainMenu"];
                MainMenu.AddButton(0.3f, 0.5f, "Boss\nMode", new OnClick(() => {
                    screens["PlayerUpgradeMenu"] = new HenryUpgradeMenu(false);
                    screens["BossUpgradeMenu"] = new HenryUpgradeMenu(true);
                    screens["Battlefield"] = new HenryBattlefield(true); //new battlefield w/ boss mode true
                    TheBattlefield = (HenryBattlefield)screens["Battlefield"]; //done so some things can reference the battlefield
                    TheBattlefield.LoadContent();
                    RNSEB.CurrentScreen = "Battlefield";
                }));
                MainMenu.AddButton(0.175f, 0.5f, "Player\nMode", new OnClick(() => {
                    screens["PlayerUpgradeMenu"] = new HenryUpgradeMenu(false);
                    screens["BossUpgradeMenu"] = new HenryUpgradeMenu(true);
                    screens["Battlefield"] = new HenryBattlefield(false); //new battlefield w/ boss mode false, i.e. player mode true
                    TheBattlefield = (HenryBattlefield)screens["Battlefield"]; //done so some things can reference the battlefield
                    TheBattlefield.LoadContent();
                    RNSEB.CurrentScreen = "Battlefield";
                }));
                MainMenu.AddButton(0.34f, 0.675f, "Multi-\nplayer", new OnClick(()=>{RNSEB.CurrentScreen = "SignIn";}));
                MainMenu.AddButton(0.7f, 0.5f, "Load", new OnClick(()=>{RNSEB.CurrentScreen = "LoadMenu";}));
                MainMenu.AddButton(0.66f, 0.675f, "Quit", new OnClick(()=>{RNSEB.CurrentScreen = "Exit";}));
                MainMenu.AddButton(0.5f, 0.75f, "How to\n  Play", new OnClick(()=>{RNSEB.CurrentScreen = "HowToPlay";}));
                Color TitleColor = Color.White;
                MainMenu.AddText(0.5f, 0.15f, TitleFont, TitleColor, "Rising Nebula Star");
                MainMenu.AddText(0.5f, 0.225f, TitleFont, TitleColor, "Extinction Battler:");
                MainMenu.AddText(0.5f, 0.3f, TitleFont, TitleColor, "The Final Sin"); //todo: find a way to center justify text
                MainMenu.AddButton(0.5f, 0.5f, "", new OnClick(()=>{RNSEB.CurrentScreen = "Credits";}), "PlayerIdle", "PlayerHit", "PlayerDeath", 1.5f);

                //create how to play screen
                createHowToPlayMenus(); //see waay below!

                   
                    Stream strweam = File.Open("RNSEB_UGRAD.osl", FileMode.Open);
                    BossUpgradeMenu = (HenryUpgradeMenu)bformatter.Deserialize(strweam);
                    PlayerUpgradeMenu = (HenryUpgradeMenu)bformatter.Deserialize(strweam);
                    strweam.Close();
                    


                //Create Sign In Screen
                HenryMenu SignIn = (HenryMenu)screens["SignIn"];
                SignIn.AddButton(0.9f, 0.9f, "Back", new OnClick(() => { RNSEB.CurrentScreen = "MainMenu"; }));
                SignIn.AddText(0.5f, 0.1f, TitleFont, Color.White, "Sign In");
                SignIn.AddText(0.25f, 0.25f, TextFont, Color.White, "No profile signed in!\nPress the Home Key to Sign In.");

                //Create New/Search Session Screen
                HenryMenu SessionOption = (HenryMenu)screens["SessionOption"];
                SessionOption.AddButton(0.9f, 0.9f, "Back", new OnClick(() => { RNSEB.CurrentScreen = "MainMenu"; }));
                SessionOption.AddText(0.5f, 0.1f, TitleFont, Color.White, "Lobby");
                SessionOption.AddButton(0.35f, 0.3f, "New Session", new OnClick(() => { CreateSession(); RNSEB.CurrentScreen = "Lobby"; }));
                SessionOption.AddButton(0.65f, 0.3f, "Find Session", new OnClick(() => { FindSession();  RNSEB.CurrentScreen = "ListSessions"; }));

                //List Available Sessions Screen
                ListSessions = (HenryMenu)screens["ListSessions"];
                ListSessions.AddButton(0.9f, 0.9f, "Back", new OnClick(() => { RNSEB.CurrentScreen = "SessionOption"; }));

                //Create Player Lobby
                Lobby = (HenryMenu)screens["Lobby"];
                Lobby.AddButton(0.9f, 0.9f, "Back", new OnClick(() => { networkSession.Dispose(); RNSEB.CurrentScreen = "SessionOption"; }));

                //create pause menu
                HenryMenu PauseMenu = (HenryMenu)screens["PauseMenu"];
                PauseMenu.AddText(0.5f, 0.1f, TitleFont, Color.White, "Paused");
                PauseMenu.AddButton(0.5f, 0.5f, "Back", new OnClick(()=>{RNSEB.CurrentScreen = "Battlefield";}));


                    Stream stream = File.Open("RNSEB_UGRAD.osl", FileMode.Create);
                    bformatter.Serialize(stream, BossUpgradeMenu);
                    bformatter.Serialize(stream, PlayerUpgradeMenu);
                    
                    stream.Close();
                //create credits screen
                HenryMenu Credits = (HenryMenu)screens["Credits"];
                Credits.AddText(0.25f, 0.5f, TextFont, Color.White, "Matt Groot\nIan Wilbanks\nChris Rose\nEric See\nMatt Paniagua");
                Credits.AddText(0.75f, 0.5f, TextFont, Color.White, "Henry Bodensteiner\nRyan Koym\nParker Leech\nEric See");
                Credits.AddButton(0.5f, 0.75f, "Back", new OnClick(()=>{RNSEB.CurrentScreen = "MainMenu";}));

                //create player victory screen
                HenryMenu PlayerVictory = (HenryMenu)screens["PlayerVictory"];
                PlayerVictory.AddText(0.5f, 0.1f, TitleFont, Color.White, "Zihao is teh victor!");
                PlayerVictory.AddText(0.5f, 0.5f, TextFont, Color.White, "The inexorable time marching shall be notus revenge");
                PlayerVictory.AddButton(0.5f, 0.8f, "Main\nMenu", new OnClick(()=>{RNSEB.CurrentScreen = "MainMenu";}));

                //create boss victory screen
                HenryMenu BossVictory = (HenryMenu)screens["BossVictory"];
                BossVictory.AddText(0.5f, 0.1f, TitleFont, Color.White, "Notus is teh victor!");
                BossVictory.AddText(0.5f, 0.5f, TextFont, Color.White, "An infinity years of dark befall the universe");
                BossVictory.AddButton(0.5f, 0.8f, "Main\nMenu", new OnClick(()=>{RNSEB.CurrentScreen = "MainMenu";}));
            
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
        
            //Checks Lobby Sign In State
            if (RNSEB.CurrentScreen == "SignIn")
            {
                if (SignedInGamer.SignedInGamers.Count != 0)
                {
                    RNSEB.CurrentScreen = "SessionOption";
                }
            }

            //Maintains Lobby
            if(RNSEB.CurrentScreen == "Lobby")
            {
                HandleLobby(gameTime);
            }

            //Maintains Session Select Screen
            if (RNSEB.CurrentScreen == "ListSessions")
            {
                HandleListSessions();
            }
            RESOLUTION = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
            Input.Update(gameTime);
            TheMouse.Update(gameTime);


            if (RNSEB.CurrentScreen == "Battlefield" && networkSession != null)
            {
                if (TheBattlefield.BossMode)
                    SendDataBoss(TheBattlefield.Notus, gameTime);
                else
                {
                    SendDataPlayer(TheBattlefield.Zihao, gameTime);
                }
                networkSession.Update();
            }

                if (CurrentScreen == "Exit")
                    this.Exit();
                else
                    screens[CurrentScreen].Update(gameTime);
            
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

            
                //Henry Stuff
                screens[CurrentScreen].Draw(spriteBatch);
                TheMouse.Draw(spriteBatch); //mouse is always the last thing drawn so that it appears on top
            
            spriteBatch.End();
            base.Draw(gameTime);
        }

        void Window_ClientSizeChanged(object sender, EventArgs e) {
            //update the RESOLUTION variable
            //RESOLUTION.X = ((GameWindow)sender).ClientBounds.Width;
            //RESOLUTION.Y = ((GameWindow)sender).ClientBounds.Height;
        }

        //Ryan's MULTIPLAYER CODE
        //Multiplayer Sign In
        void SignedInGamer_SignedIn(object sender, SignedInEventArgs e)
        {
            e.Gamer.Tag = new User();
        }

        //Create new network session
        void CreateSession()
        {
            networkSession = NetworkSession.Create(
                NetworkSessionType.SystemLink,
                1, 8, 2,
                null);

            networkSession.AllowHostMigration = true;
            networkSession.AllowJoinInProgress = true;

            HookSessionEvents();
        }

        //Find network Sessions
        void FindSession()
        {
            availableSessions = NetworkSession.Find(NetworkSessionType.SystemLink, 1, null);
            selectedSessionIndex = 0;
        }

        private void HookSessionEvents()
        {
            networkSession.GamerJoined +=
                new EventHandler<GamerJoinedEventArgs>(
                    networkSession_GamerJoined);
        }

        void networkSession_GamerJoined(object sender, GamerJoinedEventArgs e)
        {
            if (!e.Gamer.IsLocal)
            {
                e.Gamer.Tag = new User();
            }
            else
            {
                e.Gamer.Tag = GetUser(e.Gamer.Gamertag);
            }
        }

        User GetUser(String gamertag)
        {
            foreach (SignedInGamer signedInGamer in
                SignedInGamer.SignedInGamers)
            {
                if (signedInGamer.Gamertag == gamertag)
                {
                    return signedInGamer.Tag as User;
                }
            }

            return new User();
        }

        //Handle Lobby Updates and Redraws
        public void HandleLobby(GameTime gameTime)
        {
            if (networkSession != null)
            {
                if (networkSession.SessionState == NetworkSessionState.Lobby)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
                            gamer.IsReady = true;
                    }

                    Lobby.clearTexts();
                    Lobby.AddText(0.5f, 0.1f, TitleFont, Color.White, "Lobby");
                    Lobby.AddText(0.5f, 0.18f, TextFont, Color.White, "Press A When Ready");
                    float y = 0.25f;
                    int gamernum = 0;
                    foreach (NetworkGamer gamer in networkSession.AllGamers)
                    {
                        string text = gamer.Gamertag;
                        User player = gamer.Tag as User;

                        if (gamer.IsReady)
                            text += " - ready!";

                        if (gamernum == 0)
                            player.BossMode = false;
                        else
                            player.BossMode = true;

                        Lobby.AddText(0.2f, y, TextFont, Color.White, text);
                        y += 0.06f;
                        gamernum++;
                    }

                    // The host checks if everyone is ready, and moves to game play if true.
                    /*if (networkSession.IsHost)
                    {
                        if (networkSession.IsEveryoneReady && networkSession.AllGamers.Count == 2)
                        {
                            networkSession.StartGame();
                            screens["PlayerUpgradeMenu"] = new HenryUpgradeMenu(false);
                            screens["BossUpgradeMenu"] = new HenryUpgradeMenu(true);
                            //If player is local (i.e Us) then create a battlefield with the correct Player/Boss mode
                            foreach (NetworkGamer gamer in networkSession.AllGamers)
                            {
                                User player = gamer.Tag as User;
                                if (gamer.IsLocal)
                                {
                                    screens["Battlefield"] = new HenryBattlefield(player.BossMode, true);
                                }
                            }
                            //screens["Battlefield"] = new HenryBattlefield(false, true);
                            TheBattlefield = (HenryBattlefield)screens["Battlefield"];
                            TheBattlefield.LoadContent();
                            RNSEB.CurrentScreen = "Battlefield";
                        }
                    }*/
                    if (networkSession.IsEveryoneReady && networkSession.AllGamers.Count == 2)
                    {
                        elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        gameStartTimer -= elapsedTime;
                        if (networkSession.IsHost && gameStartTimer <= 0)
                            networkSession.StartGame();

                        if (gameStartTimer <= 0)
                        {
                            screens["PlayerUpgradeMenu"] = new HenryUpgradeMenu(false);
                            screens["BossUpgradeMenu"] = new HenryUpgradeMenu(true);
                            //If player is local (this computer) then create a battlefield with the correct Player/Boss mode
                            foreach (NetworkGamer gamer in networkSession.AllGamers)
                            {
                                User player = gamer.Tag as User;
                                if (gamer.IsLocal)
                                {
                                    screens["Battlefield"] = new HenryBattlefield(player.BossMode, true);
                                }
                            }
                            TheBattlefield = (HenryBattlefield)screens["Battlefield"];
                            TheBattlefield.LoadContent();
                            TheBattlefield.Zihao.nonRotate = TheBattlefield.BossMode;
                            RNSEB.CurrentScreen = "Battlefield";
                        }
                    }


                    // Pump the underlying session object.
                    networkSession.Update();
                }
            }
            else
            {
                RNSEB.CurrentScreen = "SessionOption";
            }
        }

        //Handle updates and redrawing for List of Available Sessions Screen
        public void HandleListSessions()
        {
            ListSessions.clearTexts();
            ListSessions.AddText(0.5f, 0.1f, TitleFont, Color.White, "Availabe Sessions");
            ListSessions.AddText(0.5f, 0.18f, TextFont, Color.White, "Press A to join");

            float y = 0.2f;

            for (int sessionIndex = 0; sessionIndex < availableSessions.Count; sessionIndex++)
            {
                Color color = Color.White;
                if (sessionIndex == selectedSessionIndex)
                    color = Color.Red;

                ListSessions.AddText(0.5f, y, TextFont, color, availableSessions[sessionIndex].HostGamertag);
                y += 0.06f;
            }

            //Handle Keyboard
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                //Join selected Session
                if (availableSessions.Count > 0)
                {
                    networkSession = NetworkSession.Join(availableSessions[selectedSessionIndex]);
                    HookSessionEvents();

                    availableSessions.Dispose();
                    availableSessions = null;
                    RNSEB.CurrentScreen = "Lobby";
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (selectedSessionIndex > 0)
                    selectedSessionIndex--;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (selectedSessionIndex < availableSessions.Count - 1)
                    selectedSessionIndex++;
            }
        }

        //recieve data from network
        void RecieveRemoteData(LocalNetworkGamer gamer, GameTime gameTime)
        {
            //System.Diagnostics.Debug.WriteLine("gamer: " + gamer.IsDataAvailable);
            while (gamer.IsDataAvailable)
            {
                NetworkGamer sender;
                gamer.ReceiveData(packetReader, out sender);

                if (!sender.IsLocal)
                {
                    if (TheBattlefield.BossMode)
                    {
                        TheBattlefield.Zihao.acceleration = packetReader.ReadVector2();
                        TheBattlefield.Zihao.Position = packetReader.ReadVector2();
                        TheBattlefield.Zihao.Rotation = (float)packetReader.ReadDouble();
                    }
                    else
                    {
                        TheBattlefield.Notus.acceleration = packetReader.ReadVector2();
                        TheBattlefield.Notus.Position = packetReader.ReadVector2();
                        bool firing = false;
                        firing = packetReader.ReadBoolean();
                        if (firing)
                        {
                            RNSEB.Input.Equals("BossFire2");
                        }
                    }
                }

                /*if (!sender.IsLocal && ((string)gamer.Tag =="Player"))
                {
                    HenryBoss remoteboss = gamer.Tag as HenryBoss;
                    remoteboss.acceleration = packetReader.ReadVector2();
                    remoteboss.Position = packetReader.ReadVector2();
                    remoteboss.Update(gameTime);
                }
                else if (!sender.IsLocal)
                {
                    HenryPlayer remoteplayer = gamer.Tag as HenryPlayer;
                    remoteplayer.acceleration = packetReader.ReadVector2();
                    remoteplayer.Position = packetReader.ReadVector2();
                    remoteplayer.Update(gameTime);
                }*/
             }
        }

        //send data over network (in order) for player
        private void SendDataPlayer(HenryPlayer player, GameTime gameTime)
        {
            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                RecieveRemoteData(gamer, gameTime);
                packetWriter.Write(player.acceleration);
                packetWriter.Write(player.Position);
                packetWriter.Write((double)player.Rotation);
                gamer.SendData(packetWriter, SendDataOptions.InOrder);
            }
        }

        //send data over network (in order) for boss
        private void SendDataBoss(HenryBoss boss, GameTime gameTime)
        {

            foreach (LocalNetworkGamer gamer in networkSession.LocalGamers)
            {
                RecieveRemoteData(gamer, gameTime);
                
                packetWriter.Write(boss.acceleration);
                packetWriter.Write(boss.Position);
                /*for (int i = 0; i < TheBattlefield.Notus.LaunchBays.Count; i++)
                {
                    packetWriter.Write(TheBattlefield.Notus.LaunchBays[i].);
                }*/
                if (RNSEB.Input.GetKey("BossFire2"))
                {
                    packetWriter.Write(true);
                }
                else
                    packetWriter.Write(false);
                gamer.SendData(packetWriter, SendDataOptions.InOrder);
            }
        }








        //SAVE GAME CODE
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

        void createHowToPlayMenus() {
            //function to make how to play menus, because they, being several in nature,
            //hath taken up much space, and being so did necessitate their moving to an
            //isolated and manageable locale

            //screen 1
            HenryMenu HowToPlay = (HenryMenu)screens["HowToPlay"];
            HowToPlay.AddButton(0.9f, 0.9f, "Back", new OnClick(() => {
                RNSEB.CurrentScreen = "MainMenu";
            }));
            HowToPlay.AddText(0.5f, 0.1f, TitleFont, Color.White, "The Players");
            HowToPlay.AddText(0.35f, 0.25f, TextFont, Color.White, "Zihao is the last fighter pilot\nof ... something, something ...");
            HowToPlay.AddButton(0.65f, 0.25f, "", new OnClick(() => {
                RNSEB.CurrentScreen = "HowToPlay2";
            }), "PlayerIdle", "PlayerHit", "PlayerDeath", 1.0f);
            HowToPlay.AddText(0.65f, 0.6f, TextFont, Color.White, "*Unknown* is the last juggernaut\nof Notus ... something, something ...");
            HowToPlay.AddButton(0.30f, 0.6f, " ", new OnClick(() => {
                RNSEB.CurrentScreen = "HowToPlay3";
            }), "BossIdle", "BossHit", "BossDeath", 1.0f);

            //screen 2 (zihao's)
            screens.Add("HowToPlay2", new HenryMenu(cm));
            HenryMenu HowToPlay2 = (HenryMenu)screens["HowToPlay2"];
            HowToPlay2.AddText(0.5f, 0.1f, TitleFont, Color.White, "Zihao");
            //story
            HowToPlay2.AddText(0.5f, 0.2f, TextFont, Color.White, "Zihao backstory goes here.");
            //controls
            HowToPlay2.AddText(0.5f, 0.5f, TextFont, Color.White, "Zihao relies on superior maneuverablility to survive.\n"
                + "He also carries an advanced shield generator\nthat recharges quickly.\n"
                + "When the shield is destabilized by weaponfire,\nit takes some time to be reestablished.\n");
            HowToPlay2.AddText(0.5f, 0.75f, TextFont, Color.White, "You carry at most two weapons.\n"
                + "One is \"focused\" and targeted by the mouse. (Button 1)\n"
                + "The other is \"diffuse\" and hits foes around you. (Button 2)\n");
            HowToPlay2.AddText(0.5f, 0.85f, TextFont, Color.White, "Move with W,A,S,D.");
            //back button
            HowToPlay2.AddButton(0.9f, 0.9f, "Back", new OnClick(() => {
                RNSEB.CurrentScreen = "HowToPlay";
            }));

            //screen 3 (notus's)
            screens.Add("HowToPlay3", new HenryMenu(cm));
            HenryMenu HowToPlay3 = (HenryMenu)screens["HowToPlay3"];
            HowToPlay3.AddText(0.5f, 0.1f, TitleFont, Color.White, "Notus");
            //story
            HowToPlay3.AddText(0.5f, 0.25f, TextFont, Color.White, "The last dreadnaught of a long dead warrior race,\n"
                + "the Notus, this devastating engine of destruction\n"
                + "carries out its ancient programming:\n"
                + "  if(x != Notus) Destroy(x);");
            //controls
            HowToPlay3.AddText(0.5f, 0.48f, TextFont, Color.White, "Notus relies on superior durability to survive.\n"
                + "It launches mindless drones to decimate foes.\n");
            HowToPlay3.AddText(0.5f, 0.67f, TextFont, Color.White, "Launch spawners with the left mouse button.\n"
                + "Hold the left mouse button to supercharge a spawner.\n"
                + "Spawners are launched in the direction of the mouse.\n"
                + "Select which spawner to launch with 0-9 or the mouse wheel.\n"
                + "Some other weapons may be fired with the right mouse button");
            HowToPlay3.AddText(0.5f, 0.85f, TextFont, Color.White, "Move with W,A,S,D.");
            //back button
            HowToPlay3.AddButton(0.9f, 0.9f, "Back", new OnClick(() => {
                RNSEB.CurrentScreen = "HowToPlay";
            }));
        }
    }
}