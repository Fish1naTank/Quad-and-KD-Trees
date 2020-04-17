using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Quad_and_KD_Trees
{
    class PointGenerator
    {
        public int pointCount;
        public bool shapeType = true;

        private int _defaultShapeSize = 5;
        private Vector2i _shapeSizeRange = new Vector2i(3, 20);
        private List<Point> _points = new List<Point>();

        private Random _rand = new Random();

        public PointGenerator(int pPointCount)
        {
            pointCount = pPointCount;
        }

        public List<Point> GetPoints()
        {
            return _points;
        }

        public void DrawPoints(RenderWindow pWindow, Color pColor, bool pVaryingSize = false)
        {
            if (_points == null) return;
            UpdateUserData(pColor, pVaryingSize);
            foreach(Point p in _points)
            {
                pWindow.Draw(p.userData);
            }
        }

        public void UpdateUserData(Color pColor, bool pVaryingSize = false)
        {
            foreach (Point p in _points)
            {
                if (p.userData == null)
                {
                    p.SetRandomMoveDirection(_rand);

                    if (shapeType == true)
                    {
                        CircleShape point;
                        if (pVaryingSize)
                        {
                            point = new CircleShape(_rand.Next(_shapeSizeRange.X, _shapeSizeRange.Y));
                        }
                        else
                        {
                            point = new CircleShape(_defaultShapeSize);
                        }

                        point.Origin = new Vector2f(point.Radius, point.Radius);

                        p.userData = point;
                    }
                    else //shapeType is false
                    {
                        RectangleShape point;
                        if (pVaryingSize)
                        {
                            point = new RectangleShape(new Vector2f(_rand.Next(_shapeSizeRange.X, _shapeSizeRange.Y) * 2,
                                                                    _rand.Next(_shapeSizeRange.X, _shapeSizeRange.Y) * 2));
                        }
                        else
                        {
                            point = new RectangleShape(new Vector2f(_defaultShapeSize * 2, _defaultShapeSize * 2));
                        }

                        point.Origin = point.Size / 2;

                        p.userData = point;
                    }
                }

                p.userData.Position = p.position;

                if (p.colliding == true)
                {
                    p.userData.FillColor = Color.Magenta;
                }
                else
                {
                    p.userData.FillColor = pColor;
                }
            }
        }

        public void GenerateRandomPoints(Vector2i pSpawnRange)
        {
            DestroyPoints();

            for (int i = 0; i < _points.Capacity; i++)
            {
                Vector2f pos = new Vector2f((float)_rand.NextDouble() * pSpawnRange.X, (float)_rand.NextDouble() * pSpawnRange.Y);
                _points.Add(new Point(pos));
            }
        }
        
        public void GenerateCloudPoints(Vector2f pCenter, int pRadius, int count)
        {
            Random rand = new Random();

            for (int i = 0; i < count; i++)
            {
                double r = pRadius * rand.NextDouble();
                double a = (Math.PI * 2) * rand.NextDouble();
                double x = r * Math.Cos(a);
                double y = r * Math.Sin(a);
                Vector2f pos = new Vector2f((float)x, (float)y) + pCenter;

                _points.Add(new Point(pos));
            }
        }

        public void GeneratePoint(Vector2f pPosition)
        {
            _points.Add(new Point(pPosition));
        }

        public void MovePoints(GameTime pGameTime, RenderWindow pWindow)
        {
            foreach(Point p in _points)
            {
                p.Move(pGameTime, pWindow);
            }
        }

        public void DestroyPoints()
        {
            //destroy all points
            if (_points != null)
            {
                foreach (Point p in _points)
                {
                    if (p != null && p.userData != null)
                    {
                        p.userData.Dispose();
                    }
                }
            }

            _points = new List<Point>(pointCount);
        }
    }
}
