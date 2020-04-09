using System;
using SFML.System;
using SFML.Graphics;

namespace Quad_and_KD_Trees
{
    class Boundry
    {
        public Vector2f position;
        public Vector2f size;

        public Boundry(Vector2f pPosition, Vector2f pSize)
        {
            position = pPosition;
            size = pSize;
        }

        public Boundry(float x, float y, float w, float h)
        {
            position = new Vector2f(x, y);
            size = new Vector2f(w, h);
        }

        public bool Contains(Vector2f pPoint)
        {
            float wHalf = size.X / 2;
            float hHalf = size.Y / 2;

            return (pPoint.X >= position.X - wHalf && pPoint.X <= position.X + wHalf
                && pPoint.Y >= position.Y - hHalf && pPoint.Y <= position.Y + hHalf);
        }

        public void Draw(RenderWindow pWindow, Color pColor)
        {
            RectangleShape boundry = new RectangleShape(size);
            boundry.Origin = size / 2;
            boundry.Position = position;
            boundry.FillColor = Color.Transparent;
            boundry.OutlineThickness = 0.5f;
            boundry.OutlineColor = pColor;
            
            pWindow.Draw(boundry);
        }
    }
}
