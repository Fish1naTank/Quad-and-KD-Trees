using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using System.Collections.Generic;
using System.Linq;
using System.IO;

//This class should test different object states and output that data to file
namespace Quad_and_KD_Trees
{
    class TreesTester : GameLoop
    {
        public const uint DEFAULT_WINDOW_WIDTH = 1080;
        public const uint DEFAULT_WINDOW_HEIGHT = 720;
        public const string WINDOW_TITLE = "Tree Tester";

        public string consoleText = "";

        private string _treeTestDataFilepath = "TreesTestData.Txt";

        private TreeManager _treeManager;
        private PointGenerator _pointGenerator;
        private Vector2i _pointSpawnArea;

        private FPSTracker fpsTracker = new FPSTracker();

        private int _testTime = 1;

        //Test Values
        private int[] _pointAmmountToTest = new int[] { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600 };
        private int _pointTestNumber = -1;

        private int[] _treeCapacitySizes = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20};
        private int _treeCapacityTestNumber = -1;

        //Tree tests
        private NoTreeTest _noTreeTest;
        private QuadTreeTest _quadTreeTest;
        private KDTreeTest _kdTreeTest;

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
            _pointSpawnArea = (Vector2i)window.Size;

            //set test settings
            _treeManager.treeMode = TreeManager.TreeMode.NoTree;
            _treeManager.collidingPoints = true;
            _treeManager.movingPoints = true;
            consoleText = "Collision Test";

            _noTreeTest = new NoTreeTest();
            _quadTreeTest = new QuadTreeTest((Vector2f)window.Size);
            _kdTreeTest = new KDTreeTest();

            //overite / create data file
            string fileTilte = "TreeTester Data\n";
            File.WriteAllText(_treeTestDataFilepath, fileTilte);
        }

        public override void Update(GameTime pGameTime)
        {
            switch (_treeManager.treeMode)
            {
                case TreeManager.TreeMode.NoTree:
                    NoTreeUpdate(pGameTime, _treeTestDataFilepath);
                    break;

                case TreeManager.TreeMode.QuadTree:
                    QuadTreeUpdate(pGameTime);
                    break;

                case TreeManager.TreeMode.KDTree:
                    KDTreeUpdate(pGameTime);
                    break;
            }
        }

        private void NoTreeUpdate(GameTime pGameTime, string pTreeTestDataFile)
        {
            //initialize test number
            if (_pointTestNumber == -1)
            {
                _pointTestNumber = 0;
                //title dataset
                using (StreamWriter sw = File.AppendText(_treeTestDataFilepath))
                {
                    sw.WriteLine("NoTree collision tests");
                }
            }

            //update test number
            if (_noTreeTest.TestID != _pointTestNumber)
            {
                _pointGenerator = new PointGenerator(_pointAmmountToTest[_pointTestNumber]);
                _pointGenerator.GenerateRandomPoints((Vector2i)window.Size);
                _pointGenerator.UpdateUserData(Color.Red, _treeManager.varyingPointSize);

                //start new test
                _noTreeTest.InitializeTest(_pointTestNumber, _testTime, _treeManager ,_pointGenerator);
            }

            //update current test
            _noTreeTest.Update(pGameTime, window);

            //check test status
            if(_noTreeTest.TestCompleate)
            {
                //write results to file
                _noTreeTest.WriteTestStatsToFile(pTreeTestDataFile, _pointAmmountToTest[_pointTestNumber]);

                //check if we have more tests
                if (_pointTestNumber<_pointAmmountToTest.Length - 1)
                {
                    _pointTestNumber += 1;
                }
                else
                {
                    //change mode
                    _pointTestNumber = -1;
                    _treeManager.treeMode += 1;
                }
            }
        }

        private void QuadTreeUpdate(GameTime pGameTime)
        {
            //initialize test number
            if (_pointTestNumber == -1)
            {
                _pointTestNumber = 0;
                //title dataset
                using (StreamWriter sw = File.AppendText(_treeTestDataFilepath))
                {
                    sw.WriteLine("QuadTree collision tests");
                }
            }

            //update test number
            if (_quadTreeTest.TestID != _pointTestNumber)
            {
                updatePoints(_pointAmmountToTest[_pointTestNumber], _pointSpawnArea);

                //start new test
                _quadTreeTest.InitializeTest(_pointTestNumber, _testTime, _treeManager, _pointGenerator);
            }

            //update current test
            _quadTreeTest.Update(pGameTime, window);

            //check test status
            if (_quadTreeTest.TestCompleate)
            {
                //write results to file
                _quadTreeTest.WriteTestStatsToFile(_treeTestDataFilepath, _pointAmmountToTest[_pointTestNumber]);

                //check if we have more tests
                if (_pointTestNumber < _pointAmmountToTest.Length - 1)
                {
                    _pointTestNumber += 1;
                }
                else
                {
                    //change mode
                    _pointTestNumber = -1;
                    _treeManager.treeMode += 1;
                }
            }
        }

        private void KDTreeUpdate(GameTime pGameTime)
        {
            //initialize test number
            if (_pointTestNumber == -1)
            {
                _pointTestNumber = 0;
                //title dataset
                using (StreamWriter sw = File.AppendText(_treeTestDataFilepath))
                {
                    sw.WriteLine("KDTree collision tests");
                }
            }

            //update test number
            if (_kdTreeTest.TestID != _pointTestNumber)
            {
                updatePoints(_pointAmmountToTest[_pointTestNumber], _pointSpawnArea);

                //start new test
                _kdTreeTest.InitializeTest(_pointTestNumber, _testTime, _treeManager, _pointGenerator);
            }

            //update current test
            _kdTreeTest.Update(pGameTime, window);

            //check test status
            if (_kdTreeTest.TestCompleate)
            {
                //write results to file
                _kdTreeTest.WriteTestStatsToFile(_treeTestDataFilepath, _pointAmmountToTest[_pointTestNumber]);

                //check if we have more tests
                if (_pointTestNumber < _pointAmmountToTest.Length - 1)
                {
                    _pointTestNumber += 1;
                }
                else
                {
                    //change mode
                    _pointTestNumber = -1;
                    _treeManager.treeMode += 1;

                    //END OF TESTS
                    window.Close();
                }
            }
        }

        public override void Draw(GameTime pGameTime)
        {
            _noTreeTest.Draw(window);
            _quadTreeTest.Draw(window);
            _kdTreeTest.Draw(window);

            DebugUtility.DrawPreformanceData(this, window, consoleText, _pointGenerator.pointCount, Color.Red);
        }

        private void updatePoints(int pPointCount, Vector2i pSpawnArea)
        {
            _pointGenerator = new PointGenerator(pPointCount);
            _pointGenerator.GenerateRandomPoints(pSpawnArea);
            _pointGenerator.UpdateUserData(Color.Red, _treeManager.varyingPointSize);
        }
    }
}
