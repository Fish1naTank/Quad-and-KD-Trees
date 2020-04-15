using SFML.Graphics;
using SFML.System;
using System;

namespace Quad_and_KD_Trees
{
    class CircleBoundry : Boundry
    {
        public new float size;
        //size.x = w / 2
        public CircleBoundry(Vector2f pPosition, float pRadius)
        {
            position = pPosition;
            size = pRadius;
        }

        public CircleBoundry(float x, float y, float pRadius)
        {
            position = new Vector2f(x, y);
            size = pRadius;
        }

        public override bool Contains(Vector2f pPoint)
        {
            if( dist(position, pPoint) <= size)
            {
                return true;
            }
            return false;
        }

        public override bool Intersects(Boundry pBoundry)
        {
            if (pBoundry is CircleBoundry)
            {
                CircleBoundry pOther = (CircleBoundry)pBoundry;
                if (dist(position, pOther.position) <= size + pOther.size)
                {
                    return true;
                }
                return false;
            }
            else if (pBoundry is RectBoundry)
            {
                Vector2f CirclePos = position;
                Vector2f RectPos = pBoundry.position;
                Vector2f RectSize = pBoundry.size;
                //   2
                // 0   1
                //   3
                float[] RectSidePos = new float[4] { RectPos.X - RectSize.X, RectPos.X + RectSize.X, 
                                                    RectPos.Y - RectSize.Y, RectPos.Y + RectSize.Y };

                Vector2f closestEdge = new Vector2f();

                //closest edge
                if (CirclePos.X < RectSidePos[0]) closestEdge.X = RectSidePos[0];       //left
                else if (CirclePos.X > RectSidePos[1]) closestEdge.X = RectSidePos[1];  //right
                if (CirclePos.Y < RectSidePos[2]) closestEdge.Y = RectSidePos[2];       //top
                else if (CirclePos.Y > RectSidePos[3]) closestEdge.Y = RectSidePos[3];  //bot

                //distance from closest edges
                float distX = CirclePos.X - closestEdge.X;
                float distY = CirclePos.Y - closestEdge.Y;
                float distance = (float)Math.Sqrt((distX * distX) + (distY * distY));

                //collision
                if (distance <= size)
                {
                    return true;
                }
                return false;
            }

            throw new NotImplementedException();
        }

        public override void Draw(RenderWindow pWindow, Color pColor)
        {
            CircleShape boundry = new CircleShape(size);
            boundry.Origin = new Vector2f(size, size);
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
