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

namespace teamstairwell
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        /*Player player;
        Boss boss;
        List<Bullet> bul;
        List<BulletSpawner> bsp;*/

        //GameEnvironment Env;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //public static Vector2 RESOLUTION = new Vector2(1600, 1000);
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

        public static Dictionary<string, SoundEffect> SoundEffs;


        public static Dictionary<string, Song> Music;
        //public static Song ForestMusic, ForestBoss;

        public static SpriteFont GameFont;

        public ScreenManage ScreenManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (int)RESOLUTION.X;
            graphics.PreferredBackBufferHeight = (int)RESOLUTION.Y;
            Content.RootDirectory = "Content";

            //Components.Add(new GamerServicesComponent(this));
            //this.Components.Add(new GamerServicesComponent(this));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
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

            GameFont = Content.Load<SpriteFont>("GameFont");

            MediaPlayer.Volume = 0.1f;

            //Env = new GameEnvironment();

            /*bsp = new List<BulletSpawner>();
            bul = new List<Bullet>();

            player = new Player(Vector2.Zero, Vector2.Zero, Vector2.Zero, 0f);
            boss = new Boss(new Vector2(0, -350), new Vector2(7, 0), Vector2.Zero, 0f, bsp);*/

            ScreenManager = new ScreenManage();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            if (IsActive)
            {
                ScreenManager.Update(gameTime);
            }

            /*player.update(gameTime, bul);
            boss.update(gameTime);

            foreach (BulletSpawner bs in bsp)
            {
                bs.update(gameTime);
            }

            foreach (Bullet b in bul)
            {
                b.update(gameTime);
            }*/

            //Env.update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Goldenrod);
            spriteBatch.Begin();

            ScreenManager.Draw(spriteBatch);
            /*player.draw(spriteBatch);
            boss.draw(spriteBatch);

            foreach (BulletSpawner bs in bsp)
            {
                bs.draw(spriteBatch);
            }

            foreach(Bullet b in bul)
            {
                b.draw(spriteBatch);
            }*/

            //Env.draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}