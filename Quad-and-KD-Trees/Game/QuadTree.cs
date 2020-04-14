using System;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Quad_and_KD_Trees
{
    class QuadTree
    {
        public List<Point> points;
        public bool subdivided = false;

        public Boundry boundry { get; private set; }

        public int capacity { get; private set; }

        public QuadTree[] childTrees { get; private set; }


        public QuadTree(Boundry pBoundry, int pCapacity)
        {
            boundry = pBoundry;
            capacity = pCapacity;

            points = new List<Point>();
        }

        //ask to insert a point, return false if we dont take it
        public bool Insert(Point pPoint)
        {
            //stop if we do not contain the point
            if (!boundry.Contains(pPoint.position)) return false;

            //if we have space add to the list
            if(points.Count < capacity && !subdivided)
            {
                points.Add(pPoint);
                return true;
            }
            //else we need to ask for help (subdivide)
            else
            {
                if(!subdivided) Subdivide();

                return (childTrees[0].Insert(pPoint) || childTrees[1].Insert(pPoint)
                        || childTrees[2].Insert(pPoint) || childTrees[3].Insert(pPoint));
            }
        }

        public List<Point> QueryCircleRange(float pCircleRange)
        {
            throw new NotImplementedException();
        }

        public List<Point> QueryRectangelRange(Boundry pRectRange, List<Point> pFound = null)
        {
            List<Point> found = pFound;
            if (found == null) found = new List<Point>();

            if (!pRectRange.Intersects(boundry)) return found;

            if(!subdivided)
            {
                foreach(Point p in points)
                {
                    if (pRectRange.Contains(p.position)) found.Add(p);
                }
            }
            else
            {
                foreach(QuadTree childTree in childTrees)
                {
                    childTree.QueryRectangelRange(pRectRange, found);
                }
            }
            return found;
        }

        public void DrawTree(RenderWindow pWindow, Color pColor)
        {
            boundry.Draw(pWindow, pColor);
            if (childTrees == null) return;
            foreach(QuadTree qTree in childTrees)
            {
                qTree.DrawTree(pWindow, pColor);
            }
        }

        public void DrawPoints(RenderWindow pWindow, Color pColor)
        {
            if(!subdivided)
            {
                foreach (Point p in points)
                {
                    if (p.userData == null)
                    {
                        CircleShape point = new CircleShape(2);
                        p.userData = point;
                        p.userData.Origin = p.userData.Scale * 0.5f;
                    }

                    p.userData.Position = p.position;
                    p.userData.FillColor = pColor;

                    pWindow.Draw(p.userData);
                }
            }
            else
            {
                foreach(QuadTree childTree in childTrees)
                {
                    childTree.DrawPoints(pWindow, pColor);
                }
            }
        }

        private void Subdivide()
        {
            childTrees = new QuadTree[4];

            // 0 | 1
            //-------
            // 2 | 3
            float xOffset = boundry.size.X / 2;
            float yOffset = boundry.size.Y / 2;
            childTrees[0] = new QuadTree(new Boundry(boundry.position.X - xOffset, boundry.position.Y - yOffset, boundry.size.X, boundry.size.Y), capacity);
            childTrees[1] = new QuadTree(new Boundry(boundry.position.X + xOffset, boundry.position.Y - yOffset, boundry.size.X, boundry.size.Y), capacity);
            childTrees[2] = new QuadTree(new Boundry(boundry.position.X - xOffset, boundry.position.Y + yOffset, boundry.size.X, boundry.size.Y), capacity);
            childTrees[3] = new QuadTree(new Boundry(boundry.position.X + xOffset, boundry.position.Y + yOffset, boundry.size.X, boundry.size.Y), capacity);

            //give all our points to the 

            foreach(Point p in points.ToList())
            {
                if (childTrees[0].Insert(p) || childTrees[1].Insert(p)
                        || childTrees[2].Insert(p) || childTrees[3].Insert(p))
                {
                    continue;
                }
            }


            subdivided = true;
        }
    }
}
