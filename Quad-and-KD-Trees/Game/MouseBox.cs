using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;

namespace Quad_and_KD_Trees
{
    class MouseBox : RectBoundry
    {
        public List<Point> possiblePoints;
        private List<Point> _pointsFound;

        public MouseBox(Vector2f pPosition, Vector2f pSize) : base(pPosition, pSize)
        {
            position = pPosition;
            size = pSize;
        }

        public void Update(RenderWindow pGameWindow)
        {
            position = (Vector2f)Mouse.GetPosition(pGameWindow);
        }

        public override void Draw(RenderWindow pWindow, Color pColor)
        {
            drawBoundry(pWindow, pColor);
            drawPoints(pWindow, pColor);
        }

        public void CheckPoints(bool pCheckPossiblePoints = true)
        {
            if(possiblePoints == null)
            {
                _pointsFound = possiblePoints;
                return;
            }
            _pointsFound = new List<Point>();
            //check intersection
            foreach (Point p in possiblePoints)
            {
                if (!pCheckPossiblePoints)
                {
                    if (Contains(p.position))
                    {
                        _pointsFound.Add(p);
                    }
                }
                else
                {
                    _pointsFound.Add(p);
                }
            }
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
            if (_pointsFound == null) return;
            foreach(Point p in _pointsFound)
            {
                if (p == null || p.userData == null) return;

                if(!p.colliding) p.userData.FillColor = pColor;

                pWindow.Draw(p.userData);
            }
        }
    }
}
