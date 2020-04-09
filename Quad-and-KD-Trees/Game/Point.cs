using System;
using SFML.System;
using SFML.Graphics;

namespace Quad_and_KD_Trees
{
    class Point : CircleShape
    {
        private float _radius = 1;
        public Point(Vector2f pPosition) : base()
        {
            Position = pPosition;
            Radius = _radius;
        }
    }
}
