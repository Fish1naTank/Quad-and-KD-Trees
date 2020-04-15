using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;

namespace Quad_and_KD_Trees
{
    class MouseBox : Boundry
    {
        public List<Point> pointsFound;
        public bool mouseButtonReleased = false;
        public int treeMode = 1;
        public int drawMode = 1;

        private bool _mouseDown = false;
        public MouseBox(Vector2f pPosition, Vector2f pSize) : base(pPosition, pSize)
        {
            position = pPosition;
            size = pSize;
        }

        public void Update(RenderWindow pGameWindow)
        {
            position = (Vector2f)Mouse.GetPosition(pGameWindow);

            mouseReleased();
        }

        public override void Draw(RenderWindow pWindow, Color pColor)
        {
            drawBoundry(pWindow, pColor);
            drawPoints(pWindow, pColor);
        }

        private void drawBoundry(RenderWindow pWindow, Color pColor)
        {
            RectangleShape boundry = new RectangleShape(size * 2);
            boundry.Origin = size;
            boundry.Position = position;
            boundry.FillColor = Color.Transparent;
            boundry.OutlineThickness = 0.6f;
            boundry.OutlineColor = pColor;

            pWindow.Draw(boundry);
        }

        private void drawPoints(RenderWindow pWindow, Color pColor)
        {
            if (pointsFound == null) return;
            foreach(Point p in pointsFound)
            {
                p.userData.FillColor = pColor;

                pWindow.Draw(p.userData);
            }
        }

        private void mouseReleased()
        {
            mouseButtonReleased = false;
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                mouseButtonReleased = false;
                _mouseDown = true;
            }

            if (_mouseDown && !Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                mouseButtonReleased = true;
                _mouseDown = false;
            }
        }
    }
}
