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
        public Shape userData;

        private Vector2f _moveDirection = new Vector2f(0,0);
        public Point(Vector2f pPosition)
        {
            position = pPosition;

            _moveDirection = randomMoveDirection();
        }

        public Point(Vector2f pPosition, Shape pUserData)
        {
            userData = pUserData;
            position = pPosition;
        }

        public void Move(GameTime pGameTime, RenderWindow pWindow)
        {
            position += _moveDirection * pGameTime.DeltaTime;

            outOfScreen(pWindow);
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
                return new RectBoundry(position, shape.Size);
            }

            throw new NotImplementedException();
        }

        public void HandleCollision(List<Point> pOtherPoints)
        {
            if (pOtherPoints == null) return;
            foreach(Point p in pOtherPoints)
            {
                if(overlapHighlight(p))
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

        private bool overlapHighlight(Point pOther)
        {
            if (pOther == this) return false;

            CircleShape a = (CircleShape)userData;
            CircleShape b = (CircleShape)pOther.userData;
            if (a == null || b == null) return false;
            if (dist(position, pOther.position) <= a.Radius + b.Radius)
            {
                return true;
            }
            return false;
        }

        private void outOfScreen(RenderWindow pWindow)
        {
            /**/
            if (userData is CircleShape)
            {
                CircleShape shape = (CircleShape)userData;
                if (position.X + shape.Radius > pWindow.Size.X)
                {
                    position.X = pWindow.Size.X - shape.Radius;
                    _moveDirection.X *= -1;
                }
                else if (position.X - shape.Radius < 0)
                {
                    position.X = 0 + shape.Radius;
                    _moveDirection.X *= -1;
                }

                if (position.Y + shape.Radius > pWindow.Size.Y)
                {
                    position.Y = pWindow.Size.Y - shape.Radius;
                    _moveDirection.Y *= -1;
                }
                else if (position.Y - shape.Radius < 0)
                {
                    position.Y = 0 + shape.Radius;
                    _moveDirection.Y *= -1;
                }
            }
            //default case
            else
            {
                if (position.X > pWindow.Size.X)
                {
                    position.X = pWindow.Size.X;
                    _moveDirection.X *= -1;
                }
                else if (position.X < 0)
                {
                    position.X = 0;
                    _moveDirection.X *= -1;
                }

                if (position.Y > pWindow.Size.Y)
                {
                    position.Y = pWindow.Size.Y;
                    _moveDirection.Y *= -1;
                }
                else if (position.Y < 0)
                {
                    position.Y = 0;
                    _moveDirection.Y *= -1;
                }
            }
            /**/
        }

        private Vector2f randomMoveDirection()
        {
            Random rand = new Random();

            double s = 100 * rand.NextDouble();
            double a = (Math.PI * 2) * rand.NextDouble();
            double x = s * Math.Cos(a);
            double y = s * Math.Sin(a);

            return new Vector2f((float)x, (float)y);
        }

        private double dist(Vector2f a, Vector2f b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
    }
}
