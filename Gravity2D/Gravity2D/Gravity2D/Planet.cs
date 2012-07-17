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
    public class Planet : DynamicMass
    {
        Resources _resource;
        Color _colour;

        public Planet(Resources resource, Texture2D texture)
            : base(texture)
        {
            SetResource(resource);
        }

        public Planet(Resources resource, Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            SetResource(resource);
        }

        public Planet(Resources resource, Texture2D texture, Vector2 position, Vector2 velocity)
            : base(texture, position, velocity)
        {
            SetResource(resource);
        }

        public Planet(Resources resource, Texture2D texture, Vector2 position, Vector2 velocity, double mass)
            : base(texture, position, velocity, mass)
        {
            SetResource(resource);
        }

        public Planet(Resources resource, Texture2D texture, Vector2 position, Vector2 velocity, double mass, float radius)
            : base(texture, position, velocity, mass, radius)
        {
            SetResource(resource);
        }

        public void SetResource(Resources resource)
        {
            _resource = resource;
            switch (resource)
            {
                case Resources.None:
                    _colour = Color.White;
                    break;
                case Resources.Iron:
                    _colour = Color.LightSteelBlue;
                    break;
                case Resources.Hydrogen:
                    _colour = Color.Peru;
                    break;
                case Resources.Unobtanium:
                    _colour = Color.LightSlateGray;
                    break;
            }
        }

        public Resources Resource
        {
            get
            {
                return _resource;
            }
            set
            {
                _resource = value;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 cameraPosition, int width, int height, float priority)
        {
            base.Draw(gameTime, spriteBatch, cameraPosition, width, height, _colour, priority);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 cameraPosition, int width, int height)
        {
            Draw(gameTime, spriteBatch, cameraPosition, width, height, _colour, 0);
        }
    }

    public enum Resources
    {
        None,
        Iron,
        Hydrogen,
        Unobtanium
    }
}