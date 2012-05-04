using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WindowsGame2
{
    class Projectil
    {

        public bool isActive;

        public float Distance;
        public  Vector2 Position;
        //Vector2 Destiny;
        Vector2 Speed;
        Vector2 Direction;
        Texture2D Texture;
        public readonly float rotation;
        private readonly Vector2 origin;
        float Scale;


        // Get the width of the player ship
        public int Width
        {
            get { return (int)(Texture.Width*Scale); }
        }

        // Get the height of the player ship
        public int Height
        {
            get { return (int)(Texture.Height * Scale); }
        }

        public Projectil(Vector2 _position, Vector2 _destiny, Vector2 _speed, Texture2D _texture, float _distance, float _scale, bool _isActive)
        {
            Position = _position;
            //Destiny = _destiny;
            Speed = _speed;
            Distance = _distance;
          //  GetDirection(Position, Destiny, out Direction);

            Direction = Vector2.Subtract(_destiny, Position);
            Direction = Vector2.Normalize(Direction);

            Texture = _texture;
            
            rotation = Angle2Vector(Direction);
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Scale = _scale;
            isActive = _isActive;
        }

        public void Update(GameTime gametime)
        {
            Vector2 lastPosition = Position;   
            Position += Direction * Speed * (float)gametime.ElapsedGameTime.TotalSeconds;
            if ((Distance -= Vector2.Distance(Position, lastPosition)) <= 0)
                isActive = false;

        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, rotation, origin, Scale, SpriteEffects.None, 0);
        }
        
        public static float Angle2Vector(Vector2 v)
        {
            return (float)Math.Atan2(v.Y, v.X);
        }
    }
}