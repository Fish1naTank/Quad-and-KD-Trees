using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using System.Collections.Generic;

namespace Quad_and_KD_Trees
{
    class TreesGame : GameLoop
    {
        public const uint DEFAULT_WINDOW_WIDTH = 1080;
        public const uint DEFAULT_WINDOW_HEIGHT = 720;
        public const string WINDOW_TITLE = "Quad and KD Trees";

        public PointGenerator pointGenerator;
        public QuadTree quadTree;
        public KDTree kdTree;

        MouseBox mouseBox;

        public TreesGame() : base(DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, WINDOW_TITLE, Color.Black)
        {

        }

        public override void LoadContent()
        {
            DebugUtility.LoadContent();
        }

        public override void Initialize()
        {
            pointGenerator = new PointGenerator(10);
            //pointGenerator.GenerateRandomPoints((Vector2i)window.Size);
            //pointGenerator.GenerateCloudPoints((Vector2f)window.Size / 2, 720);


            /**
            //QuadTree
            Boundry quadTreeBoundry = new Boundry(window.Size.X / 2, window.Size.Y / 2, window.Size.X, window.Size.Y);
            quadTree = new QuadTree(quadTreeBoundry, 1);

            //add points to tree
            foreach(Point p in pointGenerator.GetPoints())
            {
                quadTree.Insert(p);
            }
            /**/

            /**
            //KDTree
            kdTree = new KDTree(pointGenerator.GetPoints(), 4);
            kdTree.GenerateTree();
            /**/

            mouseBox = new MouseBox(new Vector2f(0, 0), new Vector2f(100, 100));
        }

        public override void Update(GameTime pGameTime)
        {
            //live testing
            /**/
            //KDTree
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                pointGenerator.GenerateCloudPoints((Vector2f)Mouse.GetPosition(window), 100, 1);

                switch (mouseBox.mode)
                {
                    case 1:
                        //QuadTree
                        Boundry quadTreeBoundry = new Boundry(window.Size.X / 2, window.Size.Y / 2, window.Size.X, window.Size.Y);
                        quadTree = new QuadTree(quadTreeBoundry, 1, pointGenerator.GetPoints());
                        break;

                    case 2:
                        kdTree = new KDTree(pointGenerator.GetPoints(), 4);
                        break;
                }
            }
            /**/

            if(Keyboard.IsKeyPressed(Keyboard.Key.Num1))
            {
                mouseBox.mode = 1;
                clearScreen();
            }
            else if(Keyboard.IsKeyPressed(Keyboard.Key.Num2))
            {
                mouseBox.mode = 2;
                clearScreen();
            }

            //clearup
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                clearScreen();
            }

            if (quadTree != null) quadTree.GenerateTree();
            if (kdTree != null) kdTree.GenerateTree();

            mouseBox.Update(window);

            if (quadTree != null) mouseBox.pointsFound = quadTree.QueryRectangelRange(mouseBox);
            if (kdTree != null) mouseBox.pointsFound = kdTree.QueryRectangelRange(mouseBox);
        }

        public override void Draw(GameTime pGameTime)
        {
            DebugUtility.DrawPreformanceData(this, Color.Red);

            if (quadTree != null) quadTree.DrawTree(window, Color.White);
            if (kdTree != null)  kdTree.DrawSplitLines(window, (Vector2f)window.Size, Color.White, new Vector2f(0,0));

            pointGenerator.DrawPoints(window, Color.Red);
            mouseBox.Draw(window, Color.Green);
        }

        private void clearScreen()
        {
            pointGenerator.DestroyPoints();

            quadTree = null;
            kdTree = null;

            mouseBox.pointsFound = null;
        }
    }
}
