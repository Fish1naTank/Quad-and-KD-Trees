using SFML.Graphics;
using SFML.System;
using System;

namespace Quad_and_KD_Trees
{
    class RectBoundry : Boundry
    {
        //size.x = w / 2
        public RectBoundry(Vector2f pPosition, Vector2f pSize)
        {
            position = pPosition;
            size = pSize;
        }

        public RectBoundry(float x, float y, float w, float h)
        {
            position = new Vector2f(x, y);
            size = new Vector2f(w / 2, h / 2);
        }

        public override bool Contains(Vector2f pPoint)
        {
            return (pPoint.X >= position.X - size.X && pPoint.X <= position.X + size.X
                && pPoint.Y >= position.Y - size.Y && pPoint.Y <= position.Y + size.Y);
        }

        public override bool Intersects(Boundry pBoundry)
        {
            if (pBoundry is CircleBoundry)
            {
                Vector2f CirclePos = pBoundry.position;
                Vector2f RectPos = position;
                Vector2f RectSize = size;
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
                float distX = CirclePos.X - closestEdge.X;
                float distY = CirclePos.Y - closestEdge.Y;
                float distance = (float)Math.Sqrt((distX * distX) + (distY * distY));

                //collision
                if (distance <= pBoundry.size.X)
                {
                    return true;
                }
                return false;
            }
            else if (pBoundry is RectBoundry)
            {
                return !((position.X + size.X < pBoundry.position.X - pBoundry.size.X)
                || (position.X - size.X > pBoundry.position.X + pBoundry.size.X)
                || (position.Y + size.Y < pBoundry.position.Y - pBoundry.size.Y)
                || (position.Y - size.Y > pBoundry.position.Y + pBoundry.size.Y));
            }

            throw new NotImplementedException();
        }

        public override void Draw(RenderWindow pWindow, Color pColor)
        {
            RectangleShape boundry = new RectangleShape(size * 2);
            boundry.Origin = size;
            boundry.Position = position;
            boundry.FillColor = Color.Transparent;
            boundry.OutlineThickness = 0.5f;
            boundry.OutlineColor = pColor;

            pWindow.Draw(boundry);
        }
    }
}
