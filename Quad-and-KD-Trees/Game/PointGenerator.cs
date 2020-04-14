using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Quad_and_KD_Trees
{
    class PointGenerator
    {
        public enum Distrabution { Random, Cloud };
        public Distrabution distrabutionType;
        public int pointCount;

        private List<Point> _points;

        public PointGenerator(int pPointCount, Distrabution pDistrabutionType = Distrabution.Random)
        {
            pointCount = pPointCount;
            distrabutionType = pDistrabutionType;
        }

        public void Generate(Vector2i pSpawnRange)
        {
            //destroy all points
            if (_points != null)
            {
                foreach(Point p in _points)
                {
                    if(p != null)
                    {
                        p.userData.Dispose();
                    }
                }
            }

            _points = new List<Point>(pointCount);

            //add create new points
            switch (distrabutionType)
            {
                case Distrabution.Random:
                    GenerateRandomPoints(pSpawnRange);
                    break;
                case Distrabution.Cloud:
                    GenerateCloudPoints();
                    break;
            }
        }

        public List<Point> GetPoints()
        {
            return _points;
        }

        public void DrawPoints(RenderWindow pWindow, Color pColor)
        {
            foreach(Point p in _points)
            {
                if (p.userData == null)
                {
                    CircleShape point = new CircleShape(1);
                    p.userData = point;
                }

                p.userData.Position = p.position;
                p.userData.FillColor = pColor;

                pWindow.Draw(p.userData);
            }
        }

        private void GenerateRandomPoints(Vector2i pSpawnRange)
        {
            Random rand = new Random();

            for (int i = 0; i < _points.Capacity; i++)
            {
                Vector2f pos = new Vector2f(rand.Next(0, pSpawnRange.X), rand.Next(0, pSpawnRange.Y));
                _points.Add(new Point(pos));
            }
        }
        
        private void GenerateCloudPoints()
        {

        }
    }
}
