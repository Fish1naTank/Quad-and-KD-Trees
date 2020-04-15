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

        private List<Point> _points = new List<Point>();

        public PointGenerator(int pPointCount)
        {
            pointCount = pPointCount;
        }

        public List<Point> GetPoints()
        {
            return _points;
        }

        public void DrawPoints(RenderWindow pWindow, Color pColor)
        {
            if (_points == null) return;
            foreach(Point p in _points)
            {
                if (p.userData == null)
                {
                    Random rand = new Random();
                    CircleShape point = new CircleShape(rand.Next(3, 20));
                    point.Origin = new Vector2f(point.Radius, point.Radius);
                    p.userData = point;
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

                pWindow.Draw(p.userData);
            }
        }

        public void GenerateRandomPoints(Vector2i pSpawnRange)
        {
            DestroyPoints();

            Random rand = new Random();

            for (int i = 0; i < _points.Capacity; i++)
            {
                Vector2f pos = new Vector2f((float)rand.NextDouble() * pSpawnRange.X, (float)rand.NextDouble() * pSpawnRange.Y);
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
