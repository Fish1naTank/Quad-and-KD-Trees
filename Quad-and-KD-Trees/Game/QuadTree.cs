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

        public RectBoundry boundry { get; private set; }

        public int capacity { get; private set; }

        public QuadTree[] childTrees { get; private set; }

        public QuadTree() {}
        public QuadTree(RectBoundry pBoundry, int pCapacity)
        {
            boundry = pBoundry;
            capacity = pCapacity;

            points = new List<Point>();
        }

        
        public void Insert(List<Point> pPointList)
        {
            foreach (Point p in pPointList)
            {
                Insert(p);
            }
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
                if (!subdivided)
                {
                    //check if the points we have will stop us from subdividing
                    List<Point> allPoints = points;
                    allPoints.Add(pPoint);
                    Vector2f largestSize = getLargestPointSize(allPoints);
                    if(largestSize.X * 2 > boundry.size.X || largestSize.Y * 2 > boundry.size.Y)
                    {
                        points.Add(pPoint);
                        return true;
                    }

                    Subdivide();
                }

                return (childTrees[0].Insert(pPoint) || childTrees[1].Insert(pPoint)
                        || childTrees[2].Insert(pPoint) || childTrees[3].Insert(pPoint));
            }
        }

        //get close points
        public List<Point> QueryRange(Boundry pRange, List<Point> pFound = null)
        {
            List<Point> found = pFound;
            if (found == null) found = new List<Point>();

            //adjust range for our data set
            Boundry range = rangeAdjust(pRange);

            if (!range.Intersects(boundry)) return found;

            if(!subdivided)
            {
                foreach (Point p in points)
                {
                    found.Add(p);
                }
            }
            else
            {
                foreach(QuadTree childTree in childTrees)
                {
                    //recur
                    //pass the original search range
                    childTree.QueryRange(pRange, found);
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
            Vector2f offset = new Vector2f(boundry.size.X / 2, boundry.size.Y / 2);
            childTrees[0] = new QuadTree(new RectBoundry(boundry.position.X - offset.X, boundry.position.Y - offset.Y, boundry.size.X, boundry.size.Y), capacity);
            childTrees[1] = new QuadTree(new RectBoundry(boundry.position.X + offset.X, boundry.position.Y - offset.Y, boundry.size.X, boundry.size.Y), capacity);
            childTrees[2] = new QuadTree(new RectBoundry(boundry.position.X - offset.X, boundry.position.Y + offset.Y, boundry.size.X, boundry.size.Y), capacity);
            childTrees[3] = new QuadTree(new RectBoundry(boundry.position.X + offset.X, boundry.position.Y + offset.Y, boundry.size.X, boundry.size.Y), capacity);

            subdivided = true;

            //give all our points to the child trees
            foreach (Point p in points.ToList())
            {
                if (childTrees[0].Insert(p) || childTrees[1].Insert(p)
                        || childTrees[2].Insert(p) || childTrees[3].Insert(p))
                {
                    continue;
                }
            }
        }

        private Boundry rangeAdjust(Boundry pRange)
        {
            if (points != null)
            {
                Vector2f sizeIncrease = getLargestPointSize(points);

                if(sizeIncrease != new Vector2f(0,0))
                {
                    float largerSize = (float)length(sizeIncrease);
                    sizeIncrease = new Vector2f(largerSize, largerSize) / 2;

                    if(pRange is CircleBoundry)
                    {
                        return new CircleBoundry(pRange.position, pRange.size.X + sizeIncrease.X);
                    }
                    else if (pRange is RectBoundry)
                    {
                        return new RectBoundry(pRange.position, pRange.size + sizeIncrease);
                    }
                }
            }

            return pRange;
        }

        private Vector2f getLargestPointSize(List<Point> points)
        {
            Vector2f largestSize = new Vector2f(0, 0);
            foreach (Point point in points)
            {
                if (point != null)
                {
                    Vector2f pointSize = point.GetSize();
                    if (pointSize.X > largestSize.X || pointSize.Y > largestSize.Y)
                    {
                        largestSize = pointSize;
                    }
                }
            }
            return largestSize;
        }

        private double length(Vector2f v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }
    }
}
