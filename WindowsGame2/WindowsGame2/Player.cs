using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace WindowsGame2
{
    class Player
    {
        
        public Vector2 Position;
        //Animation Anime;
        public float Speed;
        Texture2D image;

        // Get the width of the player ship
        public int Width
        {
            get { return image.Width; }
        }

        // Get the height of the player ship
        public int Height
        {
            get { return image.Height; }
        }
        
        public void Initialize(Vector2 _position, Texture2D _animation, float _speed){
            Position = _position;
            image = _animation;
            Speed = _speed;
        }
        
            public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, Position, null, Color.White);
            //Anime.Draw(sb);
        }
    }
}
