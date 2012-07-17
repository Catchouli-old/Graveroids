// Credit david amador
// http://www.david-amador.com/2009/10/xna-camera-2d-with-zoom-and-rotation/
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

public class Camera2d
{
    protected float _zoom;
    public Matrix _transform;
    public Vector2 _pos;
    protected float _rotation;

    public Camera2d()
    {
        _zoom = 1.0f;
        _rotation = 0.0f;
        _pos = Vector2.Zero;
    }

    // Sets and gets zoom
    public float Zoom
    {
        get { return _zoom; }
        set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
    }

    public float Rotation
    {
        get { return _rotation; }
        set { _rotation = value; }
    }

    // Auxiliary function to move the camera
    public void Move(Vector2 amount)
    {
        _pos += amount;
    }
    // Get set position
    public Vector2 Position
    {
        get { return _pos; }
        set { _pos = value; }
    }

    public Matrix get_transformation(GraphicsDevice graphicsDevice)
    {
        _transform =       // Thanks to o KB o for this solution
          Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                     Matrix.CreateRotationZ(_rotation) *
                                     Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) *
                                     Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
        return _transform;
    }
}