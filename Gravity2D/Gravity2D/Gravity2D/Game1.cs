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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Camera2d _camera = new Camera2d();
        int _bulletTimer = 0;

        // Data
        double stationHealth = 100;
        double stationRepair = 100;
        int maxScrollH = 8192;
        int maxScrollV = 8192;
        int[] resources = { 0, 0, 0 };
        float score;
        int livesLeft = 3;
        
        // State
        GameState _gameState;
        MouseState _mouseStatePrevious;
        KeyboardState _keyboardStatePrevious;
        Vector2 _screenPosition;
        double respawnTimer = 0;

        // Resources
        Texture2D _circle;
        Texture2D _fourpx;
        Texture2D _square;
        Texture2D _rocket;
        Texture2D _starfield;
        Texture2D _needle;
        Texture2D _stationTexture;
        Texture2D _theFuckingMoon;
        Texture2D _menuBackground;
        Texture2D _playUp;
        Texture2D _playDown;
        Texture2D _exitUp;
        Texture2D _exitDown;
        SoundEffect _kerchingSE;
        SoundEffect _explosionSE;
        SoundEffect _whipSE;
        SoundEffect _laserSE;
        SpriteFont _spriteFont;

        // Instances
        Player _player;
        SpaceStation _station;
        List<MassiveObject> _objects;
        List<Planet> _planets;
        List<Bullet> _projectiles;
        Random _generator;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            TargetElapsedTime = TimeSpan.FromMilliseconds(17);
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _gameState = GameState.MENU;

            _objects = new List<MassiveObject>();
            _planets = new List<Planet>();
            _projectiles = new List<Bullet>();

            _mouseStatePrevious = Mouse.GetState();
            _keyboardStatePrevious = Keyboard.GetState();

            _generator = new Random();

            //objects.Add(new MassiveObject(new Vector2(899.807621f, 500.0f), new Vector2(0.5f, -0.866025f)));
            //objects.Add(new MassiveObject(new Vector2(50, 50), Vector2.Zero, 1));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _circle = Content.Load<Texture2D>("circle");
            _fourpx = Content.Load<Texture2D>("fourpx");
            _square = Content.Load<Texture2D>("square");
            _rocket = Content.Load<Texture2D>("rocket");
            _starfield = Content.Load<Texture2D>("starfield");
            _needle = Content.Load<Texture2D>("needle");
            _stationTexture = Content.Load<Texture2D>("deathstar");
            _theFuckingMoon = Content.Load<Texture2D>("moon");
            _menuBackground = Content.Load<Texture2D>("menuBackground");
            _playUp = Content.Load<Texture2D>("playUp");
            _playDown = Content.Load<Texture2D>("playDown");
            _exitUp = Content.Load<Texture2D>("exitUp");
            _exitDown = Content.Load<Texture2D>("exitDown");
            _kerchingSE = Content.Load<SoundEffect>("kerching");
            _explosionSE = Content.Load<SoundEffect>("explosion");
            _whipSE = Content.Load<SoundEffect>("whip");
            _laserSE = Content.Load<SoundEffect>("laser");
            _spriteFont = Content.Load<SpriteFont>("SpriteFont1");
            _screenPosition = Vector2.Zero;

            // Post load init
            /*objects.Add(new PointMass(_circle, new Vector2(640, 50), Vector2.Zero, 50, 100));
            objects.Add(new PointMass(_circle, new Vector2(1100, 300), Vector2.Zero, 50, 100));
            objects.Add(new PointMass(_circle, new Vector2(380.192379f, 500.0f), Vector2.Zero, 25, 50));*/
            _player = new Player(_rocket);
            _station = new SpaceStation(_stationTexture);
            _objects.Add(_player);
            _objects.Add(_station);

            _objects.Add((MassiveObject)new DynamicMass(_circle, new Vector2(400, 350), new Vector2(0f, 10.0f), 10, 50));

            //objects.Add((PointMass)new DynamicMass(_circle, new Vector2(890, 350), new Vector2(0f, -10.0f), 100, 25));

            // TODO: use this.Content to load your game content here
        }

        Planet createPlanet()
        {
            const int max = 8192;
            Resources resource = Resources.None;
            int randomInt = _generator.Next(16);
            if (randomInt < 8)
                resource = Resources.Hydrogen;
            else if (randomInt < 14)
                resource = Resources.Iron;
            else
                resource = Resources.Unobtanium;

            Vector2 position = new Vector2(_generator.Next(max) - max / 2, _generator.Next(max) - max / 2);
            while ((_player.Position - position).LengthSquared() < 1000000)
            {
                position = new Vector2(_generator.Next(max) - max / 2, _generator.Next(max) - max / 2);
            }
            int mass = _generator.Next(5, 20);
            int radius = mass * 5;
            Planet planet = new Planet(resource, _theFuckingMoon, position, new Vector2(_generator.Next(0, 15), _generator.Next(0, 15)), mass, radius);
            return planet;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (!Keyboard.GetState().IsKeyDown(Keys.Escape) && _keyboardStatePrevious.IsKeyDown(Keys.Escape))
            {
                if (_gameState == GameState.MENU)
                    this.Exit();
                else
                    _gameState = GameState.MENU;
            }

            switch (_gameState)
            {
                case GameState.MENU:
                    if (Mouse.GetState().LeftButton == ButtonState.Released && _mouseStatePrevious.LeftButton == ButtonState.Pressed)
                    {
                        if (_mouseStatePrevious.X > 521 && _mouseStatePrevious.Y > 321 && _mouseStatePrevious.X < 618 && _mouseStatePrevious.Y < 370)
                        {
                            _planets.Clear();
                            _objects.Clear();
                            _projectiles.Clear();
                            _player = new Player(_rocket);
                            stationHealth = 100;
                            stationRepair = 100;
                            resources[0] = 0;
                            resources[1] = 0;
                            resources[2] = 0;
                            respawnTimer = 0;
                            _station = new SpaceStation(_stationTexture);
                            _objects.Add(_player);
                            _objects.Add(_station);
                            livesLeft = 3;
                            score = 0;
                            _gameState = GameState.PLAY;
                        }
                        else if (_mouseStatePrevious.X > 523 && _mouseStatePrevious.Y > 371 && _mouseStatePrevious.X < 614 && _mouseStatePrevious.Y < 418)
                        {
                            this.Exit();
                        }
                    }
                    break;
                case GameState.PLAY:
                    UpdateGame(gameTime);
                    break;
                case GameState.GAMEOVER:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        _gameState = GameState.MENU;
                    break;
                default:
                    this.Exit();
                    break;
            }

            // TODO: Add your update logic here
            _mouseStatePrevious = Mouse.GetState();
            _keyboardStatePrevious = Keyboard.GetState();

            base.Update(gameTime);
        }

        protected void UpdateGame(GameTime gameTime)
        {
            if (stationHealth <= 0)
            {
                _gameState = GameState.GAMEOVER;
            }

            if (_player.Dead)
            {
                if (livesLeft <= 0)
                {
                    _gameState = GameState.GAMEOVER;
                }

                if (respawnTimer < 0)
                {
                    respawnTimer = 0;
                    _player.Position = Vector2.Zero;
                    _player.Velocity = Vector2.Zero;
                    resources[0] = 0;
                    resources[1] = 0;
                    resources[2] = 0;
                    _player.Dead = false;
                }
                else
                {
                    respawnTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }

            _bulletTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            while (_planets.Count < 100)
            {
                Planet planet = createPlanet();
                _objects.Add(planet);
                _planets.Add(planet);
            }

            Vector2 playerDirection = new Vector2((float)(Math.Cos(_player.Rotation)), (float)(Math.Sin(_player.Rotation)));

            for (int i = 0; i < _objects.Count; i++)
            {
                for (int j = 0; j < _objects.Count; j++)
                {
                    if (i != j)
                    {
                        if (_objects[i] is SpaceStation)
                        {
                            if (_objects[j] == _player && !_player.Dead)
                            {
                                if (_objects[i].BoundingSphere.Intersects(_objects[j].BoundingSphere))
                                {
                                    if (resources[0] > 0 || resources[1] > 0 || resources[2] > 0)
                                    {
                                        _kerchingSE.Play();
                                        score += resources[0] * 500 + resources[1] * 200 + resources[2] * 100;
                                        stationHealth += resources[0] + (0.5 * resources[1]);
                                        stationRepair += resources[2];
                                        _station.Resources[0] += resources[0];
                                        _station.Resources[1] += resources[1];
                                        _station.Resources[2] += resources[2];
                                        resources[0] = 0;
                                        resources[1] = 0;
                                        resources[2] = 0;
                                    }
                                }
                            }
                            else if (_objects[j] is Planet && _objects[i].BoundingSphere.Intersects(_objects[j].BoundingSphere))
                            {
                                _objects[j].Dead = true;
                                double damage = ((Planet)_objects[j]).Velocity.Length();
                                if (stationRepair > 0)
                                {
                                    damage /= 2;
                                    stationRepair -= damage;
                                }
                                if (stationRepair < 0)
                                {
                                    stationHealth -= stationRepair;
                                    stationRepair = 0;
                                }

                                stationHealth -= damage;
                            }
                        }
                        if (_objects[i] is DynamicMass)
                        {
                            if (_objects[i] == _player && _objects[j] is Planet && !_player.Dead)
                            {
                                if ((_objects[i] == _player || _objects[j] == _player) && _player.Dead)
                                    continue;

                                if (((Planet)_objects[j]).Resource != Resources.None)
                                {
                                    if ((_player.Position - _objects[j].Position).LengthSquared() < 10000 + (_objects[j].Radius * _objects[j].Radius))
                                    {
                                        switch (((Planet)_objects[j]).Resource)
                                        {
                                            case Resources.Unobtanium:
                                                resources[0] += (int)((Planet)_objects[j]).Radius;
                                                break;
                                            case Resources.Iron:
                                                resources[1] += (int)((Planet)_objects[j]).Radius;
                                                break;
                                            case Resources.Hydrogen:
                                                resources[2] += (int)((Planet)_objects[j]).Radius;
                                                break;
                                        }
                                        ((Planet)_objects[j]).SetResource(Resources.None);
                                        _whipSE.Play();
                                    }
                                }
                            }
                            MassiveObject planet = _objects[i];
                            MassiveObject other = _objects[j];
                            if (planet.BoundingSphere.Intersects(other.BoundingSphere))
                            {
                                if (other is DynamicMass && !(planet is Bullet))
                                {
                                    if (planet != _player && other != _player)
                                    {
                                        //for (int k = 0; k < _projectiles.Count; k++)
                                        //{
                                        //    Bullet bullet = _projectiles[k];

                                        //    if (_objects[i].BoundingSphere.Intersects(bullet.BoundingSphere))
                                        //    {
                                        //        _objects[i].Radius *= 0.5f;
                                        //    }
                                        //}
                                        if (!(other is Bullet))
                                        {
                                            if (_generator.NextDouble() < 0.5)
                                            {
                                                other.Dead = true;
                                                planet.Mass += other.Mass;
                                                planet.Radius += (float)Math.Sqrt(other.Radius);
                                                ((DynamicMass)planet).Velocity += ((DynamicMass)other).Velocity;
                                                if (planet.Mass > 500)
                                                    planet.Mass = 500;
                                                if (planet.Radius > 100)
                                                    planet.Radius = 100;

                                            }
                                            else
                                            {
                                                planet.Dead = true;
                                                other.Dead = true;
                                            }
                                        }
                                        else if (other is Bullet)
                                        {
                                            ((Planet)planet).Velocity = ((Bullet)other).Velocity;
                                            other.Dead = true;
                                        }
                                    }
                                    else if (!_player.Dead)
                                    {
                                        _explosionSE.Play();
                                        _player.Dead = true;
                                        livesLeft--;
                                        respawnTimer = 3000;
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                double distanceSquared = (_objects[i].Position - _objects[j].Position).LengthSquared();
                                double accelerationMagnitude = (250 * _objects[j].Mass) / (distanceSquared != 0 ? distanceSquared : 1);
                                double direction = Math.Atan2(_objects[j].Position.X - _objects[i].Position.X, _objects[j].Position.Y - _objects[i].Position.Y);

                                ((DynamicMass)_objects[i]).Velocity += new Vector2((float)(Math.Sin(direction) * accelerationMagnitude), (float)(Math.Cos(direction) * accelerationMagnitude));
                            }
                        }
                    }
                }

                if (!_player.Dead)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        if (_bulletTimer > 800)
                        {
                            _laserSE.Play();
                            Bullet bullet = new Bullet(_player, _circle, _player.Position + (playerDirection * 30.0f), Vector2.Normalize(playerDirection) * 15.0f, 0.1, 3);
                            _objects.Add(bullet);
                            _projectiles.Add(bullet);
                            _bulletTimer = 0;
                        }
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        _player.Velocity += playerDirection * 0.005f * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 17);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        _player.Velocity -= playerDirection * 0.005f * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 17);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        _player.Rotation -= 0.0005f * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 17);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        _player.Rotation += 0.0005f * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 17);
                    }
                }

                if (i >= 0 && i < _objects.Count)
                {
                    if (_objects[i] is DynamicMass)
                    {
                        if (((DynamicMass)_objects[i]).Velocity.LengthSquared() > 100)
                        {
                            ((DynamicMass)_objects[i]).Velocity = Vector2.Normalize(((DynamicMass)_objects[i]).Velocity) * 10.0f;
                        }
                        ((DynamicMass)_objects[i]).Update(gameTime);
                    }
                }
            }

            foreach (Bullet bullet in _projectiles)
            {
                bullet.Update(gameTime);
            }

            // Remove dead objects and make living objects wrap around
            for (int i = 0; i < _objects.Count; i++)
            {
                MassiveObject currentObject = _objects[i];
                if (currentObject.Dead)
                {
                    if (currentObject != _player)
                    {
                        _objects.Remove(currentObject);
                        if (currentObject is DynamicMass)
                        {
                            if (currentObject is Planet)
                                _planets.Remove((Planet)currentObject);
                            if (currentObject is Bullet)
                                _projectiles.Remove((Bullet)currentObject);

                        }
                    }
                }
                else if (currentObject.X > (maxScrollH / 2))
                {
                    currentObject.X -= maxScrollH;
                }
                else if (currentObject.X < -(maxScrollH / 2))
                {
                    currentObject.X += maxScrollH;
                }
                else if (currentObject.Y > (maxScrollV / 2))
                {
                    currentObject.Y -= maxScrollH;
                }
                else if (currentObject.Y < -(maxScrollV / 2))
                {
                    currentObject.Y += maxScrollH;
                }
            }

            //if (Keyboard.GetState().IsKeyDown(Keys.Left))
            //    _camera.Rotation -= 0.01f;
            //else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            //    _camera.Rotation += 0.01f;

            if (_objects.Count > 0)
                _camera.Position = _player.Position;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        _camera.get_transformation(GraphicsDevice));
            {
                switch (_gameState)
                {
                    case GameState.MENU:
                        _camera.Position = new Vector2(640, 360);
                        DrawMenu(gameTime);
                        break;
                    case GameState.PLAY:
                        DrawGame(gameTime);
                        break;
                    case GameState.GAMEOVER:
                        _camera.Position = new Vector2(640, 360);
                        spriteBatch.DrawString(_spriteFont, "YOU LOSE. Only got a score of " + score + "? Pitiful.", new Vector2(350, 360), Color.White);
                        spriteBatch.DrawString(_spriteFont, "Press return to continue.", new Vector2(350, 400), Color.White);
                        break;
                    default:
                        this.Exit();
                        break;
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawMenu(GameTime gameTime)
        {
            spriteBatch.Draw(_menuBackground, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            spriteBatch.Draw((_mouseStatePrevious.X > 521 && _mouseStatePrevious.Y > 321 && _mouseStatePrevious.X < 618 && _mouseStatePrevious.Y < 370 && _mouseStatePrevious.LeftButton != ButtonState.Pressed ? _playDown : _playUp), Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw((_mouseStatePrevious.X > 523 && _mouseStatePrevious.Y > 371 && _mouseStatePrevious.X < 614 && _mouseStatePrevious.Y < 418 && _mouseStatePrevious.LeftButton != ButtonState.Pressed ? _exitDown : _exitUp), Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public void DrawGame(GameTime gameTime)
        {
            // Draw starfields

            for (float y = (_camera.Position.Y * 1.0f) - graphics.PreferredBackBufferHeight - 512; y < (_camera.Position.Y * 1.0f) + graphics.PreferredBackBufferHeight + 512; y += 256)
            {
                for (float x = _camera.Position.X - graphics.PreferredBackBufferWidth - 512; x < _camera.Position.X + graphics.PreferredBackBufferWidth + 512; x += 256)
                {
                    spriteBatch.Draw(_starfield, new Vector2(((int)x / 256) * 256.0f,
                        ((int)y / 256) * 256.0f), null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 100000);
                }
            }

            //foreach (Star star in _starFieldLayer1)
            //{
            //    star.Draw(gameTime, spriteBatch, 1.5f);
            //}

            //                spriteBatch.DrawString(_spriteFont, Mouse.GetState().X.ToString() + ", " + Mouse.GetState().Y.ToString(), Vector2.Transform(new Vector2(-Mouse.GetState().X, -Mouse.GetState().Y), -_camera.get_transformation(GraphicsDevice)), Color.White);
            foreach (MassiveObject planet in _objects)
            {
                if (planet is Player)
                {
                    if (_player.Dead)
                        continue;
                    //spriteBatch.DrawString(_spriteFont, planet.Velocity.Length().ToString(), planet.Position, Color.Red);
                }
                if (planet is Planet)
                    ((Planet)planet).Draw(gameTime, spriteBatch, _camera.Position, maxScrollH, maxScrollV, 50);
            }
            foreach (Bullet bullet in _projectiles)
            {
                bullet.Draw(gameTime, spriteBatch, _camera.Position, maxScrollH, maxScrollV);
            }

            if (!_player.Dead)
                _player.Draw(gameTime, spriteBatch, _camera.Position, maxScrollH, maxScrollV);
            _station.Draw(gameTime, spriteBatch, _camera.Position, maxScrollH, maxScrollV);

            //if (_startPosition != Vector2.Zero)
            //    DrawLine(spriteBatch, _fourpx, _startPosition, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);
            //spriteBatch.DrawString(_spriteFont, "(" + _player.X.ToString() + ", " + _player.Y.ToString() + ")", _player.Position, Color.Red);
            spriteBatch.DrawString(_spriteFont, "Station Health: " + (int)stationHealth, _camera.Position - new Vector2(620, 160), Color.Red);
            spriteBatch.DrawString(_spriteFont, "Station Energy: " + (int)stationRepair, _camera.Position - new Vector2(620, 140), Color.Red);

            // Draw HUD
            spriteBatch.Draw(_needle, _camera.Position - new Vector2((graphics.PreferredBackBufferWidth / 2) - 100, (graphics.PreferredBackBufferHeight / 2) - 100), null, Color.White, -(float)Math.Atan2(_camera.Position.X, _camera.Position.Y), new Vector2(50, 50), 1.0f, SpriteEffects.None, -500);

            spriteBatch.DrawString(_spriteFont, "Unobtanium: " + resources[0], _camera.Position + new Vector2(380, -320), Color.Red);
            spriteBatch.DrawString(_spriteFont, "Iron:       " + resources[1], _camera.Position + new Vector2(400, -300), Color.Red);
            spriteBatch.DrawString(_spriteFont, "Hydrogen:   " + resources[2], _camera.Position + new Vector2(420, -280), Color.Red);
            spriteBatch.DrawString(_spriteFont, "Score:      " + score, _camera.Position + new Vector2(380, -240), Color.Red);

            for (int i = 0; i < livesLeft; i++)
            {
                spriteBatch.Draw(_rocket, _camera.Position - new Vector2(580 - i * 40, 0), null, Color.White, -MathHelper.PiOver2, Vector2.Zero, 1.4f, SpriteEffects.None, -300);
            }

            if (_station.BoundingSphere.Intersects(_player.BoundingSphere))
            {
                //spriteBatch.DrawString(_spriteFont, "Press return to begin docking procedure", _camera.Position + new Vector2(-220, 50), Color.Red);
            }

            if (respawnTimer > 0)
            {
                spriteBatch.DrawString(_spriteFont, "Deploying new ship in " + (int)(respawnTimer / 1000 + 0.5), _camera.Position - new Vector2(300, 0), Color.White);
            }
        }

        public void DrawLine(SpriteBatch spriteBatch, Texture2D spr, Vector2 a, Vector2 b, Color col)
        {
            Vector2 Origin = new Vector2(0.5f, 0.0f);
            Vector2 diff = b - a;
            float angle;
            Vector2 Scale = new Vector2(1.0f, diff.Length() / spr.Height);

            angle = (float)(Math.Atan2(diff.Y, diff.X)) - MathHelper.PiOver2;

            spriteBatch.Draw(spr, a, null, col, angle, Origin, Scale, SpriteEffects.None, 1.0f);
        }
    }

    enum GameState
    {
        MENU,
        PLAY,
        GAMEOVER, 
        PAUSE
    }
}
