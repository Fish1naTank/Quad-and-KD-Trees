using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using System.Collections.Generic;
using System.Linq;

//This class should test different object states and output that data to file
namespace Quad_and_KD_Trees
{
    class TreesTester : GameLoop
    {
        public const uint DEFAULT_WINDOW_WIDTH = 1080;
        public const uint DEFAULT_WINDOW_HEIGHT = 720;
        public const string WINDOW_TITLE = "Tree Tester";

        public PointGenerator pointGenerator;
        public QuadTree quadTree;
        RectBoundry quadTreeBoundry;
        public KDTree kdTree;

        public string consoleText = "";

        private TreeManager _treeManager;

        public TreesTester() : base(DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, WINDOW_TITLE, Color.Black)
        {

        }

        public override void LoadContent()
        {
            DebugUtility.LoadContent();
        }

        public override void Initialize()
        {
            _treeManager = new TreeManager();

            pointGenerator = new PointGenerator(0);
            //pointGenerator.GenerateRandomPoints((Vector2i)window.Size);
            //pointGenerator.GenerateCloudPoints((Vector2f)window.Size / 2, 720);

            quadTreeBoundry = new RectBoundry(window.Size.X / 2, window.Size.Y / 2, window.Size.X, window.Size.Y);
        }

        public override void Update(GameTime pGameTime)
        {
            //check collisions
            if (_treeManager.collidingPoints) particleCollision();
        }

        public override void Draw(GameTime pGameTime)
        {
            //draw trees
            if (_treeManager.drawTrees)
            {
                if (quadTree != null) quadTree.DrawTree(window, Color.White);
                if (kdTree != null) kdTree.DrawSplitLines(window, (Vector2f)window.Size, Color.White, new Vector2f(0, 0));
            }

            //draw points
            pointGenerator.DrawPoints(window, Color.Red, _treeManager.varyingPointSize);

            DebugUtility.DrawPreformanceData(this, window, consoleText, pointGenerator.GetPoints().Count, Color.Red);
        }

        private void clearScreen()
        {
            //destroy points
            pointGenerator.DestroyPoints();

            clearTrees();
        }

        private void clearTrees()
        {
            //reset trees
            quadTree = null;
            kdTree = null;
        }

        private void particleCollision()
        {
            List<Point> pointList = pointGenerator.GetPoints();
            switch (_treeManager.treeMode)
            {
                case TreeManager.TreeMode.NoTree:
                    if (pointList == null) return;
                    foreach (Point p in pointList)
                    {
                        p.HandleCollision(pointList);
                    }
                    break;

                case TreeManager.TreeMode.QuadTree:
                    foreach(Point p in pointList)
                    {
                        if (quadTree != null && p.userData != null)
                            p.HandleCollision(quadTree.QueryRange(p.Boundry()));
                    }
                    break;

                case TreeManager.TreeMode.KDTree:
                    foreach (Point p in pointList)
                    {
                        if (kdTree != null && p.userData != null)
                            p.HandleCollision(kdTree.QueryRange(p.Boundry()));
                    }
                    break;
            }
        }
    }
}
