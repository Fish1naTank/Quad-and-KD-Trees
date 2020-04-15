using SFML.Graphics;
using SFML.System;
using System;

namespace Quad_and_KD_Trees
{
    class CircleBoundry : Boundry
    {
        public CircleBoundry(Vector2f pPosition, float pRadius)
        {
            position = pPosition;
            size = new Vector2f(pRadius, pRadius);
        }

        public CircleBoundry(float x, float y, float pRadius)
        {
            position = new Vector2f(x, y);
            size = new Vector2f(pRadius, pRadius);
        }

        public override bool Contains(Vector2f pPoint)
        {
            if( dist(position, pPoint) <= size.X)
            {
                return true;
            }
            return false;
        }

        public override bool Intersects(Boundry pBoundry)
        {
            if (pBoundry is CircleBoundry)
            {
                if (dist(position, pBoundry.position) <= size.X + pBoundry.size.X)
                {
                    return true;
                }
                return false;
            }
            else if (pBoundry is RectBoundry)
            {
                //check sides
                Vector2f CirclePos = position;
                Vector2f RectPos = pBoundry.position;
                Vector2f RectSize = pBoundry.size;
                //   2
                // 0   1
                //   3
                float[] RectSidePos = new float[4] { RectPos.X - RectSize.X, RectPos.X + RectSize.X, 
                                                    RectPos.Y - RectSize.Y, RectPos.Y + RectSize.Y };

                Vector2f closestEdge = CirclePos;

                //closest edge
                if (CirclePos.X < RectSidePos[0]) closestEdge.X = RectSidePos[0];       //left
                else if (CirclePos.X > RectSidePos[1]) closestEdge.X = RectSidePos[1];  //right

                if (CirclePos.Y < RectSidePos[2]) closestEdge.Y = RectSidePos[2];       //top
                else if (CirclePos.Y > RectSidePos[3]) closestEdge.Y = RectSidePos[3];  //bot

                //distance from closest edges
                float distance = (float)dist(CirclePos, closestEdge);

                //collision
                if (distance <= size.X)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public override void Draw(RenderWindow pWindow, Color pColor)
        {
            CircleShape boundry = new CircleShape(size.X);
            boundry.Origin = size;
            boundry.Position = position;
            boundry.FillColor = Color.Transparent;
            boundry.OutlineThickness = 0.5f;
            boundry.OutlineColor = pColor;

            pWindow.Draw(boundry);
        }

        private double dist(Vector2f a, Vector2f b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
    }
}
