using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using System.Collections.Generic;
using System.Linq;

//Controls
//    KEYBOARD:                                      MOUSE:
// Space : clear screen                         // LMB : draw point
// Num0  : no tree                              // RMB : draw points
// Num1  : QuadTree Mode            
// Num2  : KDTree Mode              
// Num3  : drawMode point             
// Num4  : drawMode cloud             
// D     : toggle tree draw           
// I     : increase tree capacity         
// K     : decrease tree capacity          
// F     : toggle moving points       
// C     : toggle point collision     
// P     : toggle possiblePoint highlight     
// V     : toggle varying point size     

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

        public string consoleText = "";

        private MouseBox _mouseBox;
        private TreeManager _treeManager;

        public TreesGame() : base(DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, WINDOW_TITLE, Color.Black)
        {

        }

        public override void LoadContent()
        {
            DebugUtility.LoadContent();
        }

        public override void Initialize()
        {
            _treeManager = new TreeManager();

            pointGenerator = new PointGenerator(10);
            //pointGenerator.GenerateRandomPoints((Vector2i)window.Size);
            //pointGenerator.GenerateCloudPoints((Vector2f)window.Size / 2, 720);

            quadTreeBoundry = new RectBoundry(window.Size.X / 2, window.Size.Y / 2, window.Size.X, window.Size.Y);

            _mouseBox = new MouseBox(new RectBoundry(new Vector2f(0, 0), new Vector2f(100, 100)));
            //_mouseBox = new MouseBox(new CircleBoundry(new Vector2f(0, 0), 100));
        }

        public override void Update(GameTime pGameTime)
        {
            liveTesting(pGameTime);

            //check mousebox points
            if (_mouseBox != null)
            {
                _mouseBox.Update(window);
                _mouseBox.CheckPoints(_treeManager.drawPossiblePoints);
            }

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

            //draw mouseBox
            if(_mouseBox != null) _mouseBox.Draw(window, Color.Green);

            DebugUtility.DrawPreformanceData(this, window, consoleText, Color.Red);
        }

        private void clearScreen()
        {
            //destroy points
            pointGenerator.DestroyPoints();

            clearTrees();

            //clear mousepoints
            if (_mouseBox != null) _mouseBox.possiblePoints = null;
        }

        private void clearTrees()
        {
            //reset trees
            quadTree = null;
            kdTree = null;
        }

        private void liveTesting(GameTime pGameTime)
        {
            //move points
            if (_treeManager.movingPoints) pointGenerator.MovePoints(pGameTime, window);

            //ModeSelect
            if (_treeManager.MouseReleased(Mouse.Button.Left) || Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                switch (_treeManager.pointDrawMode)
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
            }

            //Generate Tree
            switch (_treeManager.treeMode)
            {
                case TreeManager.TreeMode.NoTree:
                    if (_mouseBox != null) _mouseBox.possiblePoints = pointGenerator.GetPoints();
                    break;

                case TreeManager.TreeMode.QuadTree:

                    quadTree = new QuadTree(quadTreeBoundry, _treeManager.treeCapacity);
                    quadTree.Insert(pointGenerator.GetPoints());
                    if (_mouseBox != null)  _mouseBox.possiblePoints = quadTree.QueryRange(_mouseBox.boundry);
                    break;

                case TreeManager.TreeMode.KDTree:
                    kdTree = new KDTree(pointGenerator.GetPoints(), _treeManager.treeCapacity);
                    kdTree.GenerateTree();
                    if (_mouseBox != null) _mouseBox.possiblePoints = kdTree.QueryRange(_mouseBox.boundry);
                    break;
            }

            #region ////////////INPUT/////////////
            //tree modes
            if (Keyboard.IsKeyPressed(Keyboard.Key.Num0))
            {
                _treeManager.treeMode = TreeManager.TreeMode.NoTree;
                clearTrees();
                consoleText = "No Tree Mode";
                Console.WriteLine("No Tree Mode");
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num1))
            {
                _treeManager.treeMode = TreeManager.TreeMode.QuadTree;
                clearTrees();
                consoleText = "QuadTree Mode";
                Console.WriteLine("QuadTree Mode");
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num2))
            {
                _treeManager.treeMode = TreeManager.TreeMode.KDTree;
                clearTrees();
                consoleText = "KDTree Mode";
                Console.WriteLine("KDTree Mode");
            }
            //tree capacity
            else if (_treeManager.KeyboardReleased(Keyboard.Key.I))
            {
                _treeManager.treeCapacity += 1;
                clearTrees();
                consoleText = $"Tree Capacity : {_treeManager.treeCapacity}";
                Console.WriteLine($"Tree Capacity : {_treeManager.treeCapacity}");
            }
            else if (_treeManager.KeyboardReleased(Keyboard.Key.K))
            {
                if (_treeManager.treeCapacity > 1)
                {
                    _treeManager.treeCapacity -= 1;
                    clearTrees();
                    consoleText = $"Tree Capacity : {_treeManager.treeCapacity}";
                    Console.WriteLine($"Tree Capacity : {_treeManager.treeCapacity}");
                }
                else
                {
                    consoleText = $"Min Tree Capacity : {_treeManager.treeCapacity}";
                    Console.WriteLine($"Min Tree Capacity : {_treeManager.treeCapacity}");
                }
            }
            //draw modes
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num3))
            {
                _treeManager.pointDrawMode = TreeManager.PointDrawMode.DrawPoint;
                consoleText = "DrawPoint mode";
                Console.WriteLine("DrawPoint mode");
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Num4))
            {
                _treeManager.pointDrawMode = TreeManager.PointDrawMode.DrawCloud;
                consoleText = "DrawCloud mode";
                Console.WriteLine("DrawCloud mode");
            }
            else if (_treeManager.KeyboardReleased(Keyboard.Key.P))
            {
                _treeManager.drawPossiblePoints = !_treeManager.drawPossiblePoints;
                consoleText = $"Highlight Possible Points : {_treeManager.drawPossiblePoints}";
                Console.WriteLine($"Highlight Possible Points : {_treeManager.drawPossiblePoints}");
            }
            //draw trees
            else if (_treeManager.KeyboardReleased(Keyboard.Key.D))
            {
                _treeManager.drawTrees = !_treeManager.drawTrees;
                consoleText = $"Draw Tree : {_treeManager.drawTrees}";
                Console.WriteLine($"Draw Tree : {_treeManager.drawTrees}");
            }
            //move points
            else if (_treeManager.KeyboardReleased(Keyboard.Key.F))
            {
                _treeManager.movingPoints = !_treeManager.movingPoints;
                consoleText = $"Move Points : {_treeManager.movingPoints}";
                Console.WriteLine($"Move Points : {_treeManager.movingPoints}");
            }
            //point collision
            else if (_treeManager.KeyboardReleased(Keyboard.Key.C))
            {
                _treeManager.collidingPoints = !_treeManager.collidingPoints;
                foreach(Point p in pointGenerator.GetPoints())
                {
                    if (p != null)
                    {
                        p.colliding = false;
                    }
                }
                consoleText = $"Calculate Collisions : {_treeManager.collidingPoints}";
                Console.WriteLine($"Calculate Collisions : {_treeManager.collidingPoints}");
            }
            //point collision
            else if (_treeManager.KeyboardReleased(Keyboard.Key.V))
            {
                _treeManager.varyingPointSize = !_treeManager.varyingPointSize;
                consoleText = $"Varying Point Size : {_treeManager.varyingPointSize}";
                Console.WriteLine($"Varying Point Size : {_treeManager.varyingPointSize}");
            }
            //clearup
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                clearScreen();
                consoleText = "Clear";
                Console.WriteLine("Clear");
            }
            #endregion
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
                        if (kdTree != null && kdTree != null)
                            p.HandleCollision(kdTree.QueryRange(p.Boundry()));
                    }
                    break;
            }
        }
    }
}
