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
using KillGarfieldForFun;


namespace WindowsGame2
{
    public interface IGetLevel {
        int getLevel();
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game, IGetLevel
    {


        public int getLevel()
        {
            return CurrentLevel;
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random;
        MouseState mPreviousMouseState;
        Vector2 mouseClickVec;
        KeyboardState currentKeyboardState;
        KeyboardState prevKeyboardState;

        TimeSpan enemytimeSpawn;
        TimeSpan prevEnemySpawnTime;

        Player player;
        List<Projectil> ActiveBullets;
        Texture2D bulletText;

        List<Enemy> enemys;
        Texture2D enemyText;

        const int LEVEL_RANGE = 10;
        const float INC_SPEED_PER_LEVEL = 0.05f;
        int CurrentLevel;
        int EnemiesKilled;
        float EnemySpeed;


        bool GAME_OVER;
        //Debug
        SpriteFont sf;
 //       DebugInfo gc1;

        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
                        
            mPreviousMouseState = Mouse.GetState();
            currentKeyboardState = prevKeyboardState = Keyboard.GetState();

            mouseClickVec = new Vector2(-1, -1);
            this.IsMouseVisible = true;
            enemytimeSpawn = TimeSpan.FromSeconds(2f);
            prevEnemySpawnTime = TimeSpan.Zero;
            enemys = new List<Enemy>();
            ActiveBullets = new List<Projectil>();
            random = new Random(DateTime.Now.Millisecond);

            CurrentLevel = 1;
            EnemiesKilled = 0;
            EnemySpeed = 0.01f;

            GAME_OVER = false;
      

            //gc1 = new DebugInfo(this);
            //Components.Add(gc1);

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
            bulletText = Content.Load<Texture2D>("bullet");
            Animation playerAnimation = new Animation();
            Texture2D tank = Content.Load<Texture2D>("tank"); 
            playerAnimation.Initialize(tank,
                                           new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2),
                                           tank.Width, 
                                           tank.Height,
                                           1,0,Color.White,1f,true);
            player = new Player();
            player.Initialize(new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2), tank, 10f);

            enemyText = Content.Load<Texture2D>("garfield");

            sf = Content.Load<SpriteFont>("gameFont");
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
            prevKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            if (GAME_OVER)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Escape))
                    this.Exit();
            }
   
                
            // TODO: Add your update logic here
            //Get the current state of the Mouse
            MouseState aMouse = Mouse.GetState();

            //If the user has just clicked the Left mouse button, then set the start location for the Selection box
            if (aMouse.LeftButton == ButtonState.Pressed && mPreviousMouseState.LeftButton == ButtonState.Released)
            {
                AddBullet(new Vector2(player.Position.X + player.Width / 2, player.Position.Y + player.Height /2), new Vector2(aMouse.X, aMouse.Y));
            }
            
            //Store the previous mouse state
            mPreviousMouseState = aMouse;
            prevKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
           
            UpdateBullets(gameTime);
            UpdatePlayer(gameTime);
            UpdateEnemys(gameTime);

            UpdateCollision();
            updateLevel();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            
            spriteBatch.Begin();
            
            if (GAME_OVER) {
                spriteBatch.DrawString(sf, "Prime 'Esc' para sair.", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - sf.MeasureString("Prime 'Esc' para sair.").X / 2, sf.MeasureString("P").Y), Color.DarkOrange);
                if (EnemiesKilled < 50)
                {
                    spriteBatch.DrawString(sf, "Game over", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - sf.MeasureString("Perdeste parvinha :PPPP").X / 2, graphics.GraphicsDevice.Viewport.Height / 2 - 2 * sf.MeasureString("P").Y), Color.DarkOrange, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(sf, "Try to kill 50 garfields", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - sf.MeasureString("Quando conseguires matar 70").X / 2, graphics.GraphicsDevice.Viewport.Height / 2), Color.DarkOrange, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                    
                }
                else {
                    spriteBatch.DrawString(sf, "Good you are improving your skills", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - sf.MeasureString("Catarina larga o viciu e vai estudar :P").X / 2, graphics.GraphicsDevice.Viewport.Height / 2-sf.MeasureString("Ca").Y), Color.DarkOrange, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                }
            }
            else
            {

                player.Draw(spriteBatch);
                drawActiveBullets(spriteBatch);
                // DrawEnemiesPos(spriteBatch);d
                DrawEnemys(spriteBatch);
                spriteBatch.DrawString(sf, "Death Garfield's: " + EnemiesKilled, new Vector2(0, 10), Color.DarkOrange);
            }
            spriteBatch.End();
            
            base.Draw(gameTime);
        }


        private void UpdatePlayer(GameTime gameTime)
        {
            Vector2 prevPosition = player.Position;
            // Use the Keyboard
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                player.Position.X -= player.Speed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                player.Position.X += player.Speed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                player.Position.Y -= player.Speed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                player.Position.Y += player.Speed;
            }

            // Make sure that the player does not go out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);
            

        }

        private void updateLevel(){
            if(EnemiesKilled > CurrentLevel)
            {   
                CurrentLevel += LEVEL_RANGE;
                EnemySpeed += INC_SPEED_PER_LEVEL;
                enemytimeSpawn -=TimeSpan.FromSeconds(0.2f);
            }
        }

        private void UpdateCollision() {
            // Use the Rectangle's built-in intersect function to 
            // determine if two objects are overlapping
            Rectangle rectangle1;
            Rectangle rectangle2;

            // Only create the rectangle once for the player
            rectangle1 = new Rectangle((int)(player.Position.X + player.Width*0.10),
                        (int)(player.Position.Y + player.Height*0.10),
                        (int)(player.Width*0.80),
                        (int)(player.Height*0.80));
            spriteBatch.Begin();
            spriteBatch.Draw(enemyText, rectangle1,Color.Yellow);
            spriteBatch.End();

            //Enemy vs Player collision
            foreach (Enemy e in enemys) {

                rectangle2 = new Rectangle((int)e.Position.X,
                      (int)e.Position.Y, e.Width, e.Height);

                if (rectangle1.Intersects(rectangle2))
                {
                    GAME_OVER = true;
                }
            }

            // Projectile vs Enemy Collision
            for (int i = 0; i < ActiveBullets.Count; i++)
            {
                // Create the rectangles we need to determine if we collided with each other
                rectangle1 = new Rectangle((int)ActiveBullets[i].Position.X, (int)ActiveBullets[i].Position.Y,
                    ActiveBullets[i].Width, ActiveBullets[i].Height);
                for (int j = 0; j < enemys.Count; j++)
                {
                    if (enemys[j].isActive)
                    {
                        rectangle2 = new Rectangle((int)enemys[j].Position.X,
                            (int)enemys[j].Position.Y, enemys[j].Width, enemys[j].Height);

                        // Determine if the two objects collided with each other
                        if (rectangle1.Intersects(rectangle2))
                        {
                            enemys[j].isActive = false;
                            ActiveBullets[i].isActive = false;
                            EnemiesKilled++;
                        }
                    }
                }
            }
        }

        private void AddBullet(Vector2 o, Vector2 d){
            ActiveBullets.Add(new Projectil(o, d, new Vector2(500f, 500f), bulletText, (float)graphics.GraphicsDevice.Viewport.Width, 1 / 25f, true));
        }

        private Vector2 getRandomPosition(){
            return new Vector2(random.Next(graphics.GraphicsDevice.Viewport.Width+1), random.Next(graphics.GraphicsDevice.Viewport.Height+1));
        }

        private void AddEnemy() 
        {
            Enemy e = new Enemy();
            e.Initialize(new Vector2(random.Next(graphics.GraphicsDevice.Viewport.Width - enemyText.Width+ 1), 0), enemyText, new Vector2(0, 1), EnemySpeed, true, EnemyState.Falling);
            enemys.Add(e);
        }

        private void UpdateBullets(GameTime gt){
            for(int i=0;i<ActiveBullets.Count;i++)
            {
                if(ActiveBullets[i].isActive)
                    ActiveBullets[i].Update(gt);
                else
                    ActiveBullets.RemoveAt(i);
            }
        }

        private void UpdateEnemys(GameTime gt) { 
            prevEnemySpawnTime += gt.ElapsedGameTime;
            if (prevEnemySpawnTime > enemytimeSpawn)
            {
                AddEnemy();
                prevEnemySpawnTime = TimeSpan.Zero;
            }

            for (int i = 0; i < enemys.Count; i++)
            {
                if (enemys[i].isActive)
                {
                    if (enemys[i].Position.Y >= GraphicsDevice.Viewport.Width - enemys[i].Position.Y-enemys[i].Height)
                        enemys[i].State = EnemyState.Chasing;
                    enemys[i].Update(gt, player.Position);
                    //enemys[i].Position =  new Vector2(
                    //                        MathHelper.Clamp(enemys[i].Position.X, 0, GraphicsDevice.Viewport.Width - enemys[i].Width),
                    //                        MathHelper.Clamp(enemys[i].Position.Y, 0, GraphicsDevice.Viewport.Height - enemys[i].Height));
                }
                else
                    enemys.RemoveAt(i);
            }
        }


        private void drawActiveBullets(SpriteBatch sb) {
            foreach (Projectil p in ActiveBullets)
            {
                if (p.isActive)
                    p.Draw(sb);
            }
        }
        private void DrawEnemys(SpriteBatch sb) {
            foreach (Enemy e in enemys) {
                if (e.isActive)
                    e.Draw(sb);
            }
        }

  /*      //TEsTE method
        private void DrawEnemiesPos(SpriteBatch sb) {
            int y = 10, factor = 50;
            for(int i =0 ; i < enemys.Count;i++)
            {
                sb.DrawString(sf, "D: " + enemys[i].Position.ToString(), new Vector2(10, y + (i * factor)), Color.Black);
            }
        }*/
    }
}
