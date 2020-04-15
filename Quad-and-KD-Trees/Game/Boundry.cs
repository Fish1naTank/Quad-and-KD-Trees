using System;
using SFML.System;
using SFML.Graphics;

namespace Quad_and_KD_Trees
{
    abstract class Boundry
    {
        public Vector2f position;
        public Vector2f size;

        public abstract bool Contains(Vector2f pPoint);

        public abstract bool Intersects(Boundry pBoundry);

        public abstract void Draw(RenderWindow pWindow, Color pColor);
    }
}
