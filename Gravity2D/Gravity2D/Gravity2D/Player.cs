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
    class Player : DynamicMass
    {
        public Player(Texture2D texture)
            : base(texture, Vector2.Zero, Vector2.Zero, 0.1, 10)
        {
            Rotation = -MathHelper.PiOver2;
        }

        public Player(Texture2D texture, Vector2 position)
            : base(texture, position, Vector2.Zero, 0.1, 10)
        {
            Rotation = -MathHelper.PiOver2;
        }

        public Player(Texture2D texture, Vector2 position, Vector2 velocity)
            : base(texture, position, velocity, 0.1, 10)
        {
            Rotation = -MathHelper.PiOver2;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 cameraPosition, int width, int height)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, (float)Rotation, new Vector2(15, 10), 1.0f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            Position += Velocity * (1.00f * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 20));
        }
    }
}
