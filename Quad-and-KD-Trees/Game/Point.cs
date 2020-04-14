using SFML.System;
using SFML.Graphics;

namespace Quad_and_KD_Trees
{
    class Point
    {
        public Vector2f position;
        public Shape userData;
        public Point(Vector2f pPosition)
        {
            position = pPosition;
        }

        public Point(Vector2f pPosition, Shape pUserData)
        {
            userData = pUserData;
            position = pPosition;
        }

        //collide something
    }
}
