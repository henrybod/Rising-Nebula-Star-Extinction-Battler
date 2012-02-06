using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using teamstairwell;

namespace Hook.Graphics
{
    public class Camera
    {
        const float MAXZOOM = 5;
        const float MINZOOM = .5f;
        const float ZOOMSPD = .025f;

        public Vector3 Position, Velocity, Acceleration, Rotation;
        public float Zoom;

        public Camera()
            : this(Vector3.Zero)
        {
        }

        public Camera(Vector3 Position)
        {
            this.Position = Position;
            Velocity = Acceleration = Rotation = Vector3.Zero;
            Zoom = 1;
        }

        public void Update()
        {
            Velocity += Acceleration;
            Position += Velocity;
        }

        public void ConstrainPosition()
        {
            if(Position.Y > -Game1.RESOLUTION.Y / 2)
                Position.Y = -Game1.RESOLUTION.Y / 2;
            if(Position.Y < -7200 + Game1.RESOLUTION.Y / 2)
                Position.Y = -7200 + Game1.RESOLUTION.Y / 2;
        }

        public void ZoomOut()
        {
            Zoom -= ZOOMSPD;
            if(Zoom < MINZOOM)
                Zoom = MINZOOM;
        }
        public void ZoomIn()
        {
            Zoom += ZOOMSPD;
            if(Zoom > MAXZOOM)
                Zoom = MAXZOOM;
        }
    }
}
