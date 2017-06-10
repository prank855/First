﻿#define DEBUG

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using First.MainGame.GameObjects;

namespace First.MainGame {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Viewport viewport;
        public static Vector2 screensize;

        public Handler handler;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.SynchronizeWithVerticalRetrace = false;
            screensize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            TargetElapsedTime = TimeSpan.FromSeconds(1f / 144f);
            Content.RootDirectory = "Content";
            IsFixedTimeStep = false;
            IsMouseVisible = false;
        }

        protected override void Initialize() {
            base.Initialize();
            viewport = graphics.GraphicsDevice.Viewport;
            Start();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Initialize SpriteDictionary
            Sprite.SpriteDictionary = new Dictionary<string, Texture2D>();
            //Load Sprites
            LoadSprites();
        }

        void LoadSprites() {
            Sprite.AddSprite("Car", Content.Load<Texture2D>("Car"));
            Sprite.AddSprite("Player", Content.Load<Texture2D>("player"));
            Sprite.AddSprite("Grass", Content.Load<Texture2D>("grass"));
            Sprite.AddSprite("Selectable", Content.Load<Texture2D>("Selectable"));
            Sprite.AddSprite("NotSelectable", Content.Load<Texture2D>("NotSelectable"));
            Sprite.AddSprite("CursorRed", Content.Load<Texture2D>("CursorRed"));
            Sprite.AddSprite("CursorBlue", Content.Load<Texture2D>("CursorBlue"));
            Sprite.AddSprite("CanMove", Content.Load<Texture2D>("CanMove"));
            Sprite.AddSprite("Concrete", Content.Load<Texture2D>("Concrete"));
            Sprite.AddSprite("Blank", Content.Load<Texture2D>("Blank"));
            Sprite.AddSprite("Error", Content.Load<Texture2D>("Error"));
            Sprite.AddSprite("LongGrass", Content.Load<Texture2D>("LongGrass"));
            Sprite.AddSprite("White", Content.Load<Texture2D>("White"));
            Sprite.AddSprite("Black", Content.Load<Texture2D>("Black"));
        }

        protected override void UnloadContent() {
            //maybe later
        }


        void Start() {

            //Custom Addition
            Random rnd = new Random();

            Handler.addGameObject(new Player(new Vector2(0), new Sprite(Sprite.SpriteDictionary ["Player"])));
            Handler.addGameObject(new Selection());
            World.addLight(new Light(Vector2.Zero, 5, null, Color.White));
            World.addLight(new Light(Vector2.Zero + new Vector2(5, 0), 5, null, Color.White));
            World.addLight(new Light(Vector2.Zero + new Vector2(0, 5), 5, null, Color.White));
        }

        float elapsed = 0;
        protected override void Update(GameTime gameTime) {
            Camera.matrix = Camera.get_transformation(GraphicsDevice);
            Time.deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            elapsed += Time.deltaTime;

#if DEBUG
            while(elapsed > 1f) {
                elapsed -= 1f;
                Console.WriteLine((1 / Time.deltaTime) + " fps");
                Console.WriteLine(World.map.Count);

            }
#endif

            //emergency exit
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            //update
            Handler.Update();

        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, new RasterizerState { MultiSampleAntiAlias = true }, null, Camera.get_transformation(GraphicsDevice));
            Handler.Render(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
