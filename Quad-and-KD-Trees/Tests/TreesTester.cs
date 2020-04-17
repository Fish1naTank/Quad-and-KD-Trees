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

        private TestTreeManager _testTreeManager;

        private FPSTracker fpsTracker = new FPSTracker();

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
            _testTreeManager = new TestTreeManager(window, TreeManager.TreeMode.NoTree);

            //TestTrees
            _noTreeTest = new NoTreeTest();
            _quadTreeTest = new QuadTreeTest((Vector2f)window.Size);
            _kdTreeTest = new KDTreeTest();
        }

        public override void Update(GameTime pGameTime)
        {
            _testTreeManager.UpdateTests(pGameTime);

            switch (_testTreeManager.treeManager.treeMode)
            {
                case TreeManager.TreeMode.NoTree:
                    testTreeUpdate(pGameTime, _noTreeTest);
                    break;

                case TreeManager.TreeMode.QuadTree:
                    testTreeUpdate(pGameTime, _quadTreeTest);
                    break;

                case TreeManager.TreeMode.KDTree:
                    testTreeUpdate(pGameTime, _kdTreeTest);
                    break;
            }
        }

        private void testTreeUpdate(GameTime pGameTime, TreeTest pTreeTest)
        {
            switch (_testTreeManager.testMode)
            {
                case TestTreeManager.TestMode.CollisionTest:
                    //update test number
                    if (pTreeTest.TestID != _testTreeManager.GetTestNumber())
                    {
                        //start new test
                        pTreeTest.InitializeTest(_testTreeManager.GetTestNumber(), _testTreeManager.testTime, _testTreeManager.treeManager, _testTreeManager.GetPointGenerator());
                    }

                    //update current test
                    pTreeTest.Update(pGameTime, window);

                    //check test status
                    if (pTreeTest.TestCompleate)
                    {
                        //write results to file
                        pTreeTest.WriteTestStatsToFile(_testTreeManager.GetTestDataFilePath(), _testTreeManager.GetTestValue());

                        //tell the testTreeManager we cave compleated a test
                        _testTreeManager.TestCompleate();
                    }
                    break;
                case TestTreeManager.TestMode.CapacityTest:
                    break;
                default:
                    return;
            }
        }

        public override void Draw(GameTime pGameTime)
        {
            _noTreeTest.Draw(window);
            _quadTreeTest.Draw(window);
            _kdTreeTest.Draw(window);

            if (_testTreeManager.GetPointGenerator() != null)
            {
                if (_testTreeManager.GetPointGenerator().pointCount == -1)
                {

                }
                DebugUtility.DrawPreformanceData(this, window, _testTreeManager.consoleText, _testTreeManager.GetPointGenerator().pointCount, Color.Red);
            }
        }
    }
}
