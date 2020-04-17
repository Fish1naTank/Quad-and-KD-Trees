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

        private FPSTracker fpsTracker = new FPSTracker();

        private int _testTime = 5;

        private int[] _pointAmmountToTest = new int[] { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600 };
        private int _pointTestNumber = -1;

        //Tree tests
        private NoTreeTest _noTreeTest;

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
            _pointGenerator = new PointGenerator(0);

            _treeManager.treeMode = TreeManager.TreeMode.NoTree;
            _treeManager.collidingPoints = true;

            _noTreeTest = new NoTreeTest();

            //overite / create data file
            string fileTilte = "TreeTester Data\n";
            File.WriteAllText(_treeTestDataFilepath, fileTilte);
        }

        public override void Update(GameTime pGameTime)
        {
            double totalTimeElapsedStr = pGameTime.TotalTimeElapesd;
            double deltaTimeStr = pGameTime.DeltaTime;
            float fps = 1 / pGameTime.DeltaTime;

            switch (_treeManager.treeMode)
            {
                case TreeManager.TreeMode.NoTree:
                    //initialize test number
                    if(_pointTestNumber == -1)
                    {
                        _pointTestNumber = 0;
                        //title dataset
                        using (StreamWriter sw = File.AppendText(_treeTestDataFilepath))
                        {
                            sw.WriteLine("NoTree collision tests");
                        }
                    }
                    //run tests
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
            //update test number
            if (_noTreeTest.TestID != _pointTestNumber)
            {
                _pointGenerator = new PointGenerator(_pointAmmountToTest[_pointTestNumber]);
                _pointGenerator.GenerateRandomPoints((Vector2i)window.Size);
                _pointGenerator.UpdateUserData(Color.Red, _treeManager.varyingPointSize);

                //start new test
                _noTreeTest.InitializeTest(_pointTestNumber, _testTime, _pointGenerator);
            }

            //update current test
            _noTreeTest.Update(pGameTime);

            //check test status
            if(_noTreeTest.TestCompleate)
            {
                //write results to file
                _noTreeTest.WriteTestStatsToFile(pTreeTestDataFile);

                //check if we have more tests
                if (_pointTestNumber<_pointAmmountToTest.Length - 1)
                {
                    _pointTestNumber += 1;
                }
                else
                {
                    //change mode
                    _pointTestNumber = 0;
                    _treeManager.treeMode += 1;

                    //TEMP
                    window.Close();
                }
            }
        }

        private void QuadTreeUpdate(GameTime pGameTime)
        {
            throw new NotImplementedException();
        }

        private void KDTreeUpdate(GameTime pGameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime pGameTime)
        {
        }
    }
}
