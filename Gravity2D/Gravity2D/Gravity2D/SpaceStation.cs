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
    class SpaceStation : MassiveObject
    {
        public int[] Resources = { 0, 0, 0 };

        public SpaceStation(Texture2D texture)
            : base(texture, Vector2.Zero, 25, 200)
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 cameraPosition, int width, int height)
        {
            base.Draw(gameTime, spriteBatch, cameraPosition, width, height, Color.White, 100);
        }
    }
}
