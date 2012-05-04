using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame2
{
    public enum EnemyState {Falling, Chasing}

    class Enemy
    {
        public bool isActive;
        Vector2 position;
        //Animation Anime;
        float speed;
        Texture2D image;
        Vector2 direction;
        EnemyState state;

        public Vector2 Position {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Direction {
            get { return direction; }
            set { direction = value; }
        }

        public float Speed {
            get { return speed; }
            set { speed = value; }
        }

        public EnemyState State {
            get { return state; }
            set { state = value; }
        }

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


        
        public void Initialize(Vector2 _position, Texture2D _animation, Vector2 _direction, float _speed, bool _isactive, EnemyState _state){
            position = _position;
            image = _animation;
            direction = _direction;
            speed = _speed;
            isActive = _isactive;
            state = _state;
        }
        
        public void Update(GameTime gt, Vector2 playerPosition) {
           switch(state){
               case EnemyState.Chasing:
                  // direction = new Vector2(0,-1);
                    direction = Vector2.Normalize(Vector2.Subtract(playerPosition, position));
                    break;
            }
            position += direction * speed * gt.ElapsedGameTime.Milliseconds;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, Position, Color.White);
            //Anime.Draw(sb);
        }
    }

}
