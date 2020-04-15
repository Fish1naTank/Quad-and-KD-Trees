using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using System.Collections.Generic;
using System.Linq;

//Controls
//    KEYBOARD:                            MOUSE:
// Space : clear screen               // LMB : draw point
// Num0  : no tree                    // RMB : draw points
// Num1  : QuadTree Mode            
// Num2  : KDTree Mode              
// Num3  : drawMode point             
// Num4  : drawMode cloud             
// D     : toggle Tree draw           
// F     : toggle moving points       
// C     : toggle point collision     
// P     : toggle possiblePoint Highlight     

namespace Quad_and_KD_Trees
{
    class TreesGame : GameLoop
    {
        public const uint DEFAULT_WINDOW_WIDTH = 1080;
        public const uint DEFAULT_WINDOW_HEIGHT = 720;
        public const string WINDOW_TITLE = "Quad and KD Trees";

        public PointGenerator pointGenerator;
        public QuadTree quadTree;
        RectBoundry quadTreeBoundry;
        public KDTree kdTree;

        MouseBox mouseBox;
        TreeManager treeManager;

        public TreesGame() : base(DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, WINDOW_TITLE, Color.Black)
        {

        }

        public override void LoadContent()
        {
            DebugUtility.LoadContent();
        }

        public override void Initialize()
        {
            treeManager = new TreeManager();

            pointGenerator = new PointGenerator(10);
            //pointGenerator.GenerateRandomPoints((Vector2i)window.Size);
            //pointGenerator.GenerateCloudPoints((Vector2f)window.Size / 2, 720);

            quadTreeBoundry = new RectBoundry(window.Size.X / 2, window.Size.Y / 2, window.Size.X, window.Size.Y);

            mouseBox = new MouseBox(new Vector2f(0, 0), new Vector2f(100, 100));
        }

        public override void Update(GameTime pGameTime)
        {
            liveTesting(pGameTime);

            //check mousebox points
            mouseBox.Update(window);
            mouseBox.CheckPoints(treeManager.drawPossiblePoints);

            //check collisions
            if (treeManager.collidingPoints) particleCollision();
        }

        public override void Draw(GameTime pGameTime)
        {
            DebugUtility.DrawPreformanceData(this, Color.Red);

            //draw trees
            if (treeManager.drawTrees)
            {
                if (quadTree != null) quadTree.DrawTree(window, Color.White);
                if (kdTree != null) kdTree.DrawSplitLines(window, (Vector2f)window.Size, Color.White, new Vector2f(0, 0));
            }

            //draw points
            pointGenerator.DrawPoints(window, Color.Red);

            //draw mouseBox
            mouseBox.Draw(window, Color.Green);
        }

        private void clearScreen()
        {
            //destroy points
            pointGenerator.DestroyPoints();

            //reset trees
            quadTree = null;
            kdTree = null;

            //clear mousepoints
            mouseBox.possiblePoints = null;
        }

        private void liveTesting(GameTime pGameTime)
        {
            //move points
            if (treeManager.movingPoints) pointGenerator.MovePoints(pGameTime, window);

            //ModeSelect
            if (treeManager.MouseReleased(Mouse.Button.Left) || Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                switch (treeManager.pointDrawMode)
                {
                    case TreeManager.PointDrawMode.NoDraw:
                        break;
                    case TreeManager.PointDrawMode.DrawPoint:
                        pointGenerator.GeneratePoint((Vector2f)Mouse.GetPosition(window));
                        break;
                    case TreeManager.PointDrawMode.DrawCloud:
                        pointGenerator.GenerateCloudPoints((Vector2f)Mouse.GetPosition(window), 100, 1);
                        break;
                }

                switch (treeManager.treeMode)
                {
                    case TreeManager.TreeMode.NoTree:
                        mouseBox.possiblePoints = pointGenerator.GetPoints();
                        break;
                    case TreeManager.TreeMode.QuadTree:
                        quadTree = new QuadTree();
                        break;
                    case TreeManager.TreeMode.KDTree:
                        kdTree = new KDTree(pointGenerator.GetPoints(), 1);
                        break;
                }
            }

            //Generate Tree
            switch (treeManager.treeMode)
            {
                case TreeManager.TreeMode.QuadTree:
                    if (quadTree != null)
                    {
                        quadTree = quadTree = new QuadTree(quadTreeBoundry, 1);
                        quadTree.Insert(pointGenerator.GetPoints());
                        mouseBox.possiblePoints = quadTree.QueryRange(mouseBox);
                    }
                    break;
                case TreeManager.TreeMode.KDTree:
                    if (kdTree != null)
                    {
                        kdTree.GenerateTree();
                        mouseBox.possiblePoints = kdTree.QueryRectangelRange(mouseBox);
                    }
                    break;
            }

            #region ////////////INPUT/////////////
            //tree modes
            if (Keyboard.IsKeyPressed(Keyboard.Key.Num0))
            {
                treeManager.treeMode = TreeManager.TreeMode.NoTree;
                clearScreen();
                Console.WriteLine("No Tree Mode");
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num1))
            {
                treeManager.treeMode = TreeManager.TreeMode.QuadTree;
                clearScreen();
                Console.WriteLine("QuadTree Mode");
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num2))
            {
                treeManager.treeMode = TreeManager.TreeMode.KDTree;
                clearScreen();
                Console.WriteLine("KDTree Mode");
            }
            //draw modes
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num3))
            {
                treeManager.pointDrawMode = TreeManager.PointDrawMode.DrawPoint;
                Console.WriteLine("DrawPoint mode");
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num4))
            {
                treeManager.pointDrawMode = TreeManager.PointDrawMode.DrawCloud;
                Console.WriteLine("DrawCloud mode");
            }
            else if (treeManager.KeyboardReleased(Keyboard.Key.P))
            {
                treeManager.drawPossiblePoints = !treeManager.drawPossiblePoints;
                Console.WriteLine($"Highlight Possible Points : {treeManager.drawPossiblePoints}");
            }
            //draw trees
            else if (treeManager.KeyboardReleased(Keyboard.Key.D))
            {
                treeManager.drawTrees = !treeManager.drawTrees;
                Console.WriteLine($"Draw Tree : {treeManager.drawTrees}");
            }
            //move points
            else if (treeManager.KeyboardReleased(Keyboard.Key.F))
            {
                treeManager.movingPoints = !treeManager.movingPoints;
                Console.WriteLine($"Move Points : {treeManager.movingPoints}");
            }
            //point collision
            else if (treeManager.KeyboardReleased(Keyboard.Key.C))
            {
                treeManager.collidingPoints = !treeManager.collidingPoints;
                Console.WriteLine($"Calculate Collisions : {treeManager.collidingPoints}");
            }
            //clearup
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                clearScreen();
                Console.WriteLine("Clear");
            }
            #endregion
        }

        private void particleCollision()
        {
            List<Point> pointList = pointGenerator.GetPoints();
            switch (treeManager.treeMode)
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
                        if (quadTree == null) return;
                        if (p.userData == null) return;
                        p.HandleCollision(quadTree.QueryRange(p.Boundry()));
                    }
                    break;
                case TreeManager.TreeMode.KDTree:

                    break;
            }
        }
    }
}
