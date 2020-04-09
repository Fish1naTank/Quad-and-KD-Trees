using System;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;

namespace Quad_and_KD_Trees
{
    class QuadTree
    {
        public List<Shape> points;
        public bool subdivided = false;

        public Boundry boundry { get; private set; }

        public int capacity { get; private set; }

        public QuadTree[] childTrees { get; private set; }


        public QuadTree(Boundry pBoundry, int pCapacity)
        {
            boundry = pBoundry;
            capacity = pCapacity;

            points = new List<Shape>();
        }

        //ask to insert a point, return false if we dont take it
        public bool Insert(Shape pShape)
        {
            Vector2f shapePos = pShape.Position;

            //stop if we do not contain the point
            if (!boundry.Contains(shapePos)) return false;

            //if we have space add to the list
            if(points.Count < capacity && !subdivided)
            {
                points.Add(pShape);
                return true;
            }
            //else we need to ask for help (subdivide)
            else
            {
                if(!subdivided) Subdivide();

                return (childTrees[0].Insert(pShape) || childTrees[1].Insert(pShape)
                        || childTrees[2].Insert(pShape) || childTrees[3].Insert(pShape));
            }
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

        private void Subdivide()
        {
            childTrees = new QuadTree[4];

            // 0 | 1
            //-------
            // 2 | 3
            float wHalf = boundry.size.X / 2;
            float xOffset = wHalf / 2;
            float hHalf = boundry.size.Y / 2;
            float yOffset = hHalf / 2;
            childTrees[0] = new QuadTree(new Boundry(boundry.position.X - xOffset, boundry.position.Y - yOffset, wHalf, hHalf), capacity);
            childTrees[1] = new QuadTree(new Boundry(boundry.position.X + xOffset, boundry.position.Y - yOffset, wHalf, hHalf), capacity);
            childTrees[2] = new QuadTree(new Boundry(boundry.position.X - xOffset, boundry.position.Y + yOffset, wHalf, hHalf), capacity);
            childTrees[3] = new QuadTree(new Boundry(boundry.position.X + xOffset, boundry.position.Y + yOffset, wHalf, hHalf), capacity);

            //give all our points to the 

            foreach(Shape p in points)
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
