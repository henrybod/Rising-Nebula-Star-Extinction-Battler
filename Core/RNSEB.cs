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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;


namespace teamstairwell{
   [Serializable()]
       
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

        HenryMenu Lobby;
        HenryMenu ListSessions;

        //Henry Stuff
        bool HenryMode = true;
        HenryMouse TheMouse;
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
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
        [field: NonSerialized]
        public static SpriteFont ButtonFont, TitleFont, TextFont;
        public static HenryMediaPlayer Audio; //reference for audio manager
        public static HenryInput Input; //refence for input manager
        public static HenryBattlefield TheBattlefield; //reference for other classes to access current battlefield
        public static GameWindow Win;
        public static HenryUpgradeMenu PlayerUpgradeMenu;
        public static HenryUpgradeMenu BossUpgradeMenu;
        //static data members
        public static Vector2 RESOLUTION = new Vector2(1200, 750);
        public static Dictionary<string, SoundEffect> SoundEffs; //hash table to reference sound effects
        public static Dictionary<string, Song> Music; //hash table to reference music
        [field: NonSerialized]
        public static SpriteFont GameFont;

        //private HenryMenu Multiplayer;

      


        //functions
        public RNSEB() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (int)RESOLUTION.X;
            graphics.PreferredBackBufferHeight = (int)RESOLUTION.Y;
            //Window.AllowUserResizing = true; //nah, too complicated
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
                Fonts["Button"] = ButtonFont;
                Fonts["Title"] = TitleFont;
                Fonts["Text"] = TextFont;

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
                MainMenu.AddButton(0.3f, 0.5f, "Notus SP", new OnClick(() => {
                    screens["PlayerUpgradeMenu"] = new HenryUpgradeMenu(false);
                    screens["BossUpgradeMenu"] = new HenryUpgradeMenu(true);
                    screens["Battlefield"] = new HenryBattlefield(true); //new battlefield w/ boss mode true
                    PlayerUpgradeMenu = (HenryUpgradeMenu)screens["PlayerUpgradeMenu"];
                    BossUpgradeMenu = (HenryUpgradeMenu)screens["BossUpgradeMenu"];
                    TheBattlefield = (HenryBattlefield)screens["Battlefield"]; //done so some things can reference the battlefield
                    TheBattlefield.LoadContent();
                    RNSEB.CurrentScreen = "Battlefield";
                }));
                MainMenu.AddButton(0.175f, 0.5f, "Zihao SP", new OnClick(() => {
                    screens["PlayerUpgradeMenu"] = new HenryUpgradeMenu(false);
                    screens["BossUpgradeMenu"] = new HenryUpgradeMenu(true);
                    screens["Battlefield"] = new HenryBattlefield(false); //new battlefield w/ boss mode false, i.e. player mode true
                    TheBattlefield = (HenryBattlefield)screens["Battlefield"]; //done so some things can reference the battlefield
                    PlayerUpgradeMenu = (HenryUpgradeMenu)screens["PlayerUpgradeMenu"];
                    BossUpgradeMenu = (HenryUpgradeMenu)screens["BossUpgradeMenu"];
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

                ////////////////////create load menu

                HenryMenu LoadMenu = (HenryMenu)screens["LoadMenu"];
                LoadMenu.AddButton(0.9f, 0.9f, "Back", new OnClick(() => { RNSEB.CurrentScreen = "MainMenu"; }));
                LoadMenu.AddButton(0.5f, 0.5f, "Load Game", new OnClick(() =>
                {
                    Stream stream = File.Open("RNSEB_SAVE.osl", FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    TheBattlefield = (HenryBattlefield)bformatter.Deserialize(stream);
                    BossUpgradeMenu = (HenryUpgradeMenu)bformatter.Deserialize(stream);
                    PlayerUpgradeMenu = (HenryUpgradeMenu)bformatter.Deserialize(stream);
                    stream.Close();
                    screens["Battlefield"] = TheBattlefield;
                    screens["PlayerUpgradeMenu"] = PlayerUpgradeMenu;
                    screens["BossUpgradeMenu"] = BossUpgradeMenu;
                    RNSEB.CurrentScreen = "Battlefield";
                }));

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
                PauseMenu.AddButton(0.5f, 0.5f, "Back", new OnClick(() => { RNSEB.CurrentScreen = "Battlefield"; }));
                PauseMenu.AddButton(0.7f, 0.5f, "Main Menu", new OnClick(() => { RNSEB.CurrentScreen = "MainMenu"; }));
                PauseMenu.AddButton(0.3f, 0.5f, "Save Game", new OnClick(() =>
                {
                    Stream strweam = File.Open("RNSEB_SAVE.osl", FileMode.Create);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    bformatter.Serialize(strweam, TheBattlefield);
                    bformatter.Serialize(strweam, BossUpgradeMenu);
                    bformatter.Serialize(strweam, PlayerUpgradeMenu);
                    strweam.Close();
                    RNSEB.CurrentScreen = "Battlefield";
                }));
                //create credits screen
                HenryMenu Credits = (HenryMenu)screens["Credits"];
                Credits.AddText(0.25f, 0.5f, TextFont, Color.White, "Matt Groot\nIan Wilbanks\nChris Rose\nEric See\nMatt Paniagua");
                Credits.AddText(0.75f, 0.5f, TextFont, Color.White, "Henry Bodensteiner\nRyan Koym\nParker Leech\nEric See");
                Credits.AddButton(0.5f, 0.75f, "Back", new OnClick(()=>{RNSEB.CurrentScreen = "MainMenu";}));

                //create player victory screen
                HenryMenu PlayerVictory = (HenryMenu)screens["PlayerVictory"];
                PlayerVictory.AddText(0.5f, 0.1f, TitleFont, Color.White, "Zihao is the victor!");
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
       
        

       


        protected override void Update(GameTime gameTime) {
            
            
           
      
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
                HandleLobby();
            }

            //Maintains Session Select Screen
            if (RNSEB.CurrentScreen == "ListSessions")
            {
                HandleListSessions();
            }



            
                RESOLUTION = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
                Input.Update(gameTime);
                TheMouse.Update(gameTime);
                if (CurrentScreen == "Exit")
                    this.Exit();
                else
                    screens[CurrentScreen].Update(gameTime);
            
            base.Update(gameTime);
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
        public void HandleLobby()
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
                    foreach (NetworkGamer gamer in networkSession.AllGamers)
                    {
                        string text = gamer.Gamertag;
                        User player = gamer.Tag as User;

                        if (gamer.IsReady)
                            text += " - ready!";

                        Lobby.AddText(0.2f, y, TextFont, Color.White, text);
                        y += 0.06f;
                    }

                    // The host checks if everyone is ready, and moves to game play if true.
                    if (networkSession.IsHost)
                    {
                        if (networkSession.IsEveryoneReady)
                            networkSession.StartGame();
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
            HowToPlay.AddText(0.35f, 0.25f, TextFont, Color.White, "Outmaneuver your enemy");
            HowToPlay.AddButton(0.65f, 0.25f, "", new OnClick(() => {
                RNSEB.CurrentScreen = "HowToPlay2";
            }), "PlayerIdle", "PlayerHit", "PlayerDeath", 1.0f);
            HowToPlay.AddText(0.65f, 0.75f, TextFont, Color.White, "Overwhelm your enemy");
            HowToPlay.AddButton(0.30f, 0.75f, " ", new OnClick(() => {
                RNSEB.CurrentScreen = "HowToPlay3";
            }), "BossIdle", "BossHit", "BossDeath", 1.0f);
            HowToPlay.AddText(0.5f, 0.5f, TextFont, Color.White, "Use W,A,S,D to move.\nUse Left & Right Mouse to fire.\nChoose 1 upgrade every level.");

            //screen 2 (zihao's)
            screens.Add("HowToPlay2", new HenryMenu(cm));
            HenryMenu HowToPlay2 = (HenryMenu)screens["HowToPlay2"];
            HowToPlay2.AddText(0.5f, 0.1f, TitleFont, Color.White, "Zihao");
            //story
            HowToPlay2.AddText(0.5f, 0.25f, TextFont, Color.White, "The last remaining member of a race\nslaughtered by the Notus, Zihao races towards\ncertain death to avenge his father,\nthe inventor of the self-evolving XJ99 Starfighter.");
            //controls
            HowToPlay2.AddText(0.5f, 0.55f, TextFont, Color.White, "Zihao relies on superior maneuverablility to survive.\n"
                + "He also carries an advanced shield generator\nthat recharges quickly.\n"
                + "When the shield is destabilized by weaponfire,\nit takes some time to be reestablished.\n");
            HowToPlay2.AddText(0.5f, 0.8f, TextFont, Color.White, "You carry at most two weapons.\n"
                + "One is \"focused\" and targeted by the mouse. (Left Mouse)\n"
                + "The other is \"diffuse\" and hits foes around you. (Right Mouse)\n");
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
            //back button
            HowToPlay3.AddButton(0.9f, 0.9f, "Back", new OnClick(() => {
                RNSEB.CurrentScreen = "HowToPlay";
            }));
        }
    }
}