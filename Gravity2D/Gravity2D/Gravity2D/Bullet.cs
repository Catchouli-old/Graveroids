using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Gravity2D
{
    class Bullet : DynamicMass
    {
        int timeAlive = 0;

        public Bullet(Player player, Texture2D texture) 
            : base(texture)
        {
        }

        public Bullet(Player player, Texture2D texture, Vector2 position)
            : base(texture, position)
        {
        }

        public Bullet(Player player, Texture2D texture, Vector2 position, Vector2 velocity)
            : base(texture, position, velocity)
        {
        }

        public Bullet(Player player, Texture2D texture, Vector2 position, Vector2 velocity, double mass)
            : base(texture, position, velocity, mass)
        {
        }

        public Bullet(Player player, Texture2D texture, Vector2 position, Vector2 velocity, double mass, float radius)
            : base(texture, position, velocity, mass, radius)
        {
        }

        public override void Update(GameTime gameTime)
        {
            timeAlive += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeAlive > 1000)
                Dead = true;
            Position += Velocity * (1.00f * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 15));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 cameraPosition, int width, int height)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0, new Vector2(Radius, Radius), (Radius * 2) / 100, SpriteEffects.None, -200);
        }
    }
}
