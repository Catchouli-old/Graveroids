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
    public class DynamicMass : MassiveObject
    {
        Vector2 _velocity;

        public DynamicMass(Texture2D texture) 
            : base(texture)
        {
        }

        public DynamicMass(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            _velocity = Vector2.Zero;
        }

        public DynamicMass(Texture2D texture, Vector2 position, Vector2 velocity)
            : base(texture, position)
        {
            _velocity = velocity;
        }

        public DynamicMass(Texture2D texture, Vector2 position, Vector2 velocity, double mass)
            : base(texture, position, mass)
        {
            _velocity = velocity;
        }

        public DynamicMass(Texture2D texture, Vector2 position, Vector2 velocity, double mass, float radius)
            : base(texture, position, mass, radius)
        {
            _velocity = velocity;
        }

        public virtual void Update(GameTime gameTime)
        {
            Position += Velocity * (1.00f * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 30));
        }

        public Vector2 Velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                _velocity = value;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 cameraPosition, int width, int height, Color colour, float priority = 0)
        {
            base.Draw(gameTime, spriteBatch, cameraPosition, width, height, colour, priority);
        }
    }
}
