using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;

namespace Quad_and_KD_Trees
{
    class MouseBox
    {
        public Boundry boundry;
        public List<Point> possiblePoints;
        private List<Point> _pointsFound;

        public MouseBox(Boundry pBoundry)
        {
            boundry = pBoundry;
        }

        public void Update(RenderWindow pGameWindow)
        {
            boundry.position = (Vector2f)Mouse.GetPosition(pGameWindow);
        }

        public void Draw(RenderWindow pWindow, Color pColor)
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
                    if (boundry.Intersects(p.Boundry()))
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
            if (boundry is CircleBoundry)
            {
                CircleShape circleBoundry = new CircleShape(boundry.size.X);
                circleBoundry.Origin = boundry.size;
                circleBoundry.Position = boundry.position;
                circleBoundry.FillColor = Color.Transparent;
                circleBoundry.OutlineThickness = 0.6f;
                circleBoundry.OutlineColor = pColor;

                pWindow.Draw(circleBoundry);
            }
            else if (boundry is RectBoundry)
            {
                RectangleShape Rectboundry = new RectangleShape(boundry.size);
                Rectboundry.Origin = boundry.size;
                Rectboundry.Position = boundry.position;
                Rectboundry.FillColor = Color.Transparent;
                Rectboundry.OutlineThickness = 0.6f;
                Rectboundry.OutlineColor = pColor;

                pWindow.Draw(Rectboundry);
            }
        }

        private void drawPoints(RenderWindow pWindow, Color pColor)
        {
            if (_pointsFound == null) return;
            foreach(Point p in _pointsFound)
            {
                if (p == null || p.userData == null) return;

                if(!p.colliding) p.userData.FillColor = pColor;
                else
                {
                    p.userData.FillColor = Color.Cyan;
                }

                pWindow.Draw(p.userData);
            }
        }
    }
}
