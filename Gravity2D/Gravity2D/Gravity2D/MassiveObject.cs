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

namespace Gravity2D
{
    public class MassiveObject
    {
        public Texture2D _texture;
        Vector2 _position;
        BoundingSphere _boundingSphere;
        double _mass;
        double _scale;
        bool _dead;
        float _rotation;
        float _rotationSpeed;

        public MassiveObject()
        {
            throw new Exception("Cannot create pointmass without texture.");
        }

        public MassiveObject(Texture2D texture)
        {
            Initialize(texture, Vector2.Zero, 1, 50);
        }

        public MassiveObject(Texture2D texture, Vector2 position)
        {
            Initialize(texture, position, 1, 50);
        }

        public MassiveObject(Texture2D texture, Vector2 position, double mass)
        {
            Initialize(texture, position, mass, 50);
        }

        public MassiveObject(Texture2D texture, Vector2 position, double mass, float radius)
        {
            Initialize(texture, position, mass, radius);
        }

        private void Initialize(Texture2D texture, Vector2 position, double mass, float radius)
        {
            _texture = texture;
            Position = position;
            _mass = mass;
            _boundingSphere = new BoundingSphere(new Vector3(Position, 0), radius);
            _scale = radius * 2 / 2;
            _dead = false;
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                _boundingSphere.Center.X = value.X;
                _boundingSphere.Center.Y = value.Y;
            }
        }

        public float X
        {
            get
            {
                return _position.X;
            }
            set
            {
                _position.X = value;
                _boundingSphere.Center.X = value;
            }
        }

        public float Y
        {
            get
            {
                return _position.Y;
            }
            set
            {
                _position.Y = value;
                _boundingSphere.Center.Y = value;
            }
        }

        public double Mass
        {
            get
            {
                return _mass;
            }
            set
            {
                _mass = value;
            }
        }

        public float Radius
        {
            get
            {
                return _boundingSphere.Radius;
            }
            set
            {
                _boundingSphere.Radius = value;
            }
        }

        public BoundingSphere BoundingSphere
        {
            get
            {
                return _boundingSphere;
            }
            set
            {
            }
        }

        public bool Dead
        {
            get
            {
                return _dead;
            }
            set
            {
                _dead = value;
            }
        }

        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }

        public float RotationSpeed
        {
            get
            {
                return 0;
            }
            set
            {
                _rotationSpeed = value;
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 cameraPosition, int width, int height)
        {
            Draw(gameTime, spriteBatch, cameraPosition, width, height, Color.White);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 cameraPosition, int width, int height, Color colour, float priority = 0)
        {
            Vector2 radiusVector = Vector2.Zero;
            spriteBatch.Draw(_texture, new Rectangle((int)(Position.X - Radius), (int)(Position.Y - Radius), (int)Radius * 2, (int)Radius * 2), null, colour, Rotation, radiusVector, SpriteEffects.None, priority);

            if (Y < -height / 16.0)
            {
                spriteBatch.Draw(_texture, new Rectangle((int)(Position.X - Radius), (int)(Position.Y - Radius + height), (int)Radius, (int)Radius), null, colour, Rotation, radiusVector, SpriteEffects.None, priority);
            }
            else if (Y > (height * 7.0) / 16.0)
            {
                spriteBatch.Draw(_texture, new Rectangle((int)(Position.X - Radius), (int)(Position.Y - Radius - height), (int)Radius, (int)Radius), null, colour, Rotation, radiusVector, SpriteEffects.None, priority);
            }

            if (X < -width / 16.0)
            {
                spriteBatch.Draw(_texture, new Rectangle((int)(Position.X - Radius - width), (int)(Position.Y - Radius + height), (int)Radius, (int)Radius), null, colour, Rotation, radiusVector, SpriteEffects.None, priority);
            }
            else if (X > (width * 7.0) / 16.0)
            {
                spriteBatch.Draw(_texture, new Rectangle((int)(Position.X - Radius + width), (int)(Position.Y - Radius + height), (int)Radius, (int)Radius), null, colour, Rotation, radiusVector, SpriteEffects.None, priority);
            }
        }
    }
}
