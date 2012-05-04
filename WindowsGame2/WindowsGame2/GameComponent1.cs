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
using WindowsGame2;

namespace KillGarfieldForFun
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class DebugInfo : Microsoft.Xna.Framework.DrawableGameComponent
    {
        TimeSpan timeElepsedFromLastSecond, timeElepsedFromLastDraw;
        int frameCountPrev, frameCountCurr;
        SpriteBatch spriteBatch;
        SpriteFont sf;
        
        
        public DebugInfo(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            
            timeElepsedFromLastDraw = timeElepsedFromLastSecond = TimeSpan.Zero;
            frameCountPrev = 0;
            frameCountCurr = 0;
            Game.Window.AllowUserResizing = true;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            sf = Game.Content.Load<SpriteFont>("gameFont"); 
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            //Frame per secund rate
            timeElepsedFromLastSecond += gameTime.TotalGameTime - timeElepsedFromLastDraw;
            timeElepsedFromLastDraw = gameTime.TotalGameTime;
            frameCountCurr++;

            if (timeElepsedFromLastSecond > TimeSpan.FromMilliseconds(1000))
            {
                timeElepsedFromLastSecond = TimeSpan.Zero;
                frameCountPrev = frameCountCurr;
                frameCountCurr = 0;
            }

            spriteBatch.Begin();
            spriteBatch.DrawString(sf, "fps: " + frameCountPrev, new Vector2(Game.GraphicsDevice.Viewport.Width - (sf.MeasureString("fps: " + frameCountPrev)).X, 0), Color.Tan);
            int lvl = ((IGetLevel)this.Game).getLevel();
            spriteBatch.DrawString(sf, "Level: " + lvl, new Vector2(Game.GraphicsDevice.Viewport.Width - (sf.MeasureString("Level: " + lvl)).X, sf.MeasureString("fps: " + frameCountPrev).Y), Color.Tan);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
