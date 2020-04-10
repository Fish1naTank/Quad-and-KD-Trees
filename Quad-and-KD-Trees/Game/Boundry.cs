using System;
using SFML.System;
using SFML.Graphics;

namespace Quad_and_KD_Trees
{
    class Boundry
    {
        public Vector2f position;
        //size.x == w / 2
        public Vector2f size;

        public Boundry(Vector2f pPosition, Vector2f pSize)
        {
            position = pPosition;
            size = pSize * 0.5f;
        }

        public Boundry(float x, float y, float w, float h)
        {
            position = new Vector2f(x, y);
            size = new Vector2f(w / 2, h / 2);
        }

        public bool Contains(Vector2f pPoint)
        {
            return (pPoint.X >= position.X - size.X && pPoint.X <= position.X + size.X
                && pPoint.Y >= position.Y - size.Y && pPoint.Y <= position.Y + size.Y);
        }

        public bool Intersects(Boundry pBoundry)
        {
            return !(position.X + size.X < pBoundry.position.X - pBoundry.size.X)
                || (position.X - size.X > pBoundry.position.X + pBoundry.size.X)
                || (position.Y + size.Y < pBoundry.position.Y - pBoundry.size.Y)
                || (position.Y - size.Y > pBoundry.position.Y + pBoundry.size.Y);

        }

        public virtual void Draw(RenderWindow pWindow, Color pColor)
        {
            RectangleShape boundry = new RectangleShape(size * 2);
            boundry.Origin = size;
            boundry.Position = position;
            boundry.FillColor = Color.Transparent;
            boundry.OutlineThickness = 0.5f;
            boundry.OutlineColor = pColor;
            
            pWindow.Draw(boundry);
        }
    }
}
