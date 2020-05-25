using SFML.System;
using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace Quad_and_KD_Trees
{
    class Point
    {
        public Vector2f position;
        public bool colliding = false;
        public bool outOfScreen = false;
        public Shape userData;

        private Vector2f _moveDirection = new Vector2f(0,0);
        public Point(Vector2f pPosition)
        {
            position = pPosition;
        }

        public Point(Vector2f pPosition, Shape pUserData)
        {
            userData = pUserData;
            position = pPosition;
        }

        public void Move(GameTime pGameTime, RenderWindow pWindow, bool pEnableWindowBoundry)
        {
            position += _moveDirection * pGameTime.DeltaTime;

            checkOutOfScreen(pWindow, pEnableWindowBoundry);
        }

        public Boundry Boundry()
        {
            if (userData == null) return null;
            if(userData is CircleShape)
            {
                CircleShape shape = (CircleShape)userData;
                return new CircleBoundry(position, shape.Radius);
            }
            else if(userData is RectangleShape)
            {
                RectangleShape shape = (RectangleShape)userData;
                return new RectBoundry(position, shape.Size / 2);
            }

            throw new NotImplementedException();
        }

        public void HandleCollision(List<Point> pOtherPoints)
        {
            if (userData == null) return;
            if (pOtherPoints == null) return;
            Boundry myBounds = Boundry();
            foreach(Point p in pOtherPoints)
            {
                if(overlapHighlight(myBounds, p))
                {
                    colliding = true;
                    break;
                }
                else
                {
                    colliding = false;
                }
                
            }
        }

        //rect size = w / 2
        public Vector2f GetSize()
        {
            Vector2f size = new Vector2f(0, 0);
            if (userData != null)
            {
                if(userData is CircleShape)
                {
                    CircleShape shape = userData as CircleShape;
                    size = new Vector2f(shape.Radius, shape.Radius);
                }
                if(userData is RectangleShape)
                {
                    RectangleShape shape = userData as RectangleShape;
                    size = shape.Size / 2;
                }
            }
            return size;
        }

        public void SetRandomMoveDirection(int seed = -1)
        {
            Random rand;
            if(seed != -1)
            {
                rand = new Random(seed);
            }
            else
            {
                rand = new Random();
            }

            double s = 100 * rand.NextDouble() + 10;
            double a = (Math.PI * 2) * rand.NextDouble();
            float x = (float)(s * Math.Cos(a));
            float y = (float)(s * Math.Sin(a));

            _moveDirection = new Vector2f(x, y);
        }

        private bool overlapHighlight(Boundry pMyBounds, Point pOther)
        {
            if (pOther == this) return false;
            return pMyBounds.Intersects(pOther.Boundry());
        }

        private void checkOutOfScreen(RenderWindow pWindow, bool pEnableWindowBoundry)
        {
            Boundry boundry = Boundry();

            if (pEnableWindowBoundry)
            {
                if (position.X + boundry.size.X > pWindow.Size.X)
                {
                    position.X = pWindow.Size.X - boundry.size.X;
                    _moveDirection.X *= -1;
                }
                else if (position.X - boundry.size.X < 0)
                {
                    position.X = 0 + boundry.size.X;
                    _moveDirection.X *= -1;
                }

                if (position.Y + boundry.size.Y > pWindow.Size.Y)
                {
                    position.Y = pWindow.Size.Y - boundry.size.Y;
                    _moveDirection.Y *= -1;
                }
                else if (position.Y - boundry.size.Y < 0)
                {
                    position.Y = 0 + boundry.size.Y;
                    _moveDirection.Y *= -1;
                }
            }
            else
            {
                if (!outOfScreen)
                {
                    if (position.X - boundry.size.X > pWindow.Size.X || position.X + boundry.size.X < 0
                        || position.Y - boundry.size.Y > pWindow.Size.Y || position.Y + boundry.size.Y < 0)
                    {
                        //mark for removal
                        outOfScreen = true;
                    }
                }
            }
        }

        private double dist(Vector2f a, Vector2f b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
    }
}
