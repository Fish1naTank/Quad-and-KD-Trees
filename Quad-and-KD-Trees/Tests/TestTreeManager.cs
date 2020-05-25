using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;

namespace Quad_and_KD_Trees
{
    class TestTreeManager
    {
        public int seed = 849;

        public TreeManager treeManager = new TreeManager();
        public int testTime = 60;
        public string consoleText = "";

        public static int NumOfTests = 2;
        public enum TestMode { CollisionTest, CapacityTest }
        public TestMode testMode = TestMode.CollisionTest;

        //these are in the order of the tests above
        private string[] _testDataFilePaths = new string[2] { "CollisionTests.txt", "CapacityTest.txt" };

        //test paramaters
        public PointGenerator _pointGenerator;
        public Vector2i _pointSpawnArea;

        //Test Values
        private int[] _pointAmmountToTest = new int[] { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600 };
        //private int[] _pointAmmountToTest = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        //smol test
        //private int[] _pointAmmountToTest = new int[] { 100 };
        private int _pointTestNumber = -1;

        private int[] _treeCapacitySizes = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
        private int _treeCapacityTestNumber = -1;
        private int _treeCapacityPointCount = 1000;
        //private int _treeCapacityPointCount = 10;

        private RenderWindow _window;

        //Tree tests
        private NoTreeTest _noTreeTest;
        private QuadTreeTest _quadTreeTest;
        private KDTreeTest _kdTreeTest;


        public TestTreeManager(RenderWindow pWindow, TreeManager.TreeMode pTreeToStart, TestMode pTestToStart)
        {
            _window = pWindow;
            treeManager.treeMode = pTreeToStart;
            testMode = pTestToStart;

            //TestTrees
            _noTreeTest = new NoTreeTest();
            _quadTreeTest = new QuadTreeTest((Vector2f)_window.Size);
            _kdTreeTest = new KDTreeTest();

            //overite / create data files
            string fileTilte = "TreeTester Data\n";
            for (int i = 0; i < _testDataFilePaths.Length; i++)
            {
                File.WriteAllText(GetTestDataFilePath(), fileTilte);
            }
        }

        public void UpdateTests(GameTime pGameTime)
        {
            //initialize test set
            if (GetTestNumber() == -1)
            {
                setTreeMode(treeManager.treeMode);

                //title dataset
                using (StreamWriter sw = File.AppendText(GetTestDataFilePath()))
                {
                    sw.WriteLine($"{treeManager.treeMode.ToString()} {testMode.ToString()}");
                }
            }

            switch (treeManager.treeMode)
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
            if (pTreeTest.TestID != GetTestNumber())
            {
                updateTreeManager();

                //update the point generator
                SetPointGenerator();

                //start new test
                pTreeTest.InitializeTest(GetTestNumber(), testTime, treeManager, GetPointGenerator());
            }

            //update current test
            pTreeTest.Update(pGameTime, _window);

            //check test status
            if (pTreeTest.TestCompleate)
            {
                //write results to file
                pTreeTest.WriteTestStatsToFile(GetTestDataFilePath(), GetTestValue());

                //tell the testTreeManager we cave compleated a test
                TestCompleate();
            }
        }

        //call if you compleated a test
        public void TestCompleate()
        {
            //check if we have more of the same test
            if (GetTestNumber() < GetNumberOfTests() - 1)
            {
                setTestNumber(GetTestNumber() + 1);
            }
            else
            {
                //reset test number
                setTestNumber(-1);
                //check if we are at last tree
                if((int)treeManager.treeMode == TreeManager.NumOfTrees - 1)
                {
                    //check if there are more tests to run
                    if((int)testMode == NumOfTests - 1)
                    {
                        //END TESTS
                        _window.Close();
                    }
                    else
                    {
                        //move to next test
                        testMode += 1;

                        //resete tree
                        treeManager.treeMode = 0;
                    }
                }
                else
                {
                    //move to next tree
                    treeManager.treeMode += 1;
                }
            }
        }

        public int GetTestNumber()
        {
            switch (testMode)
            {
                case TestMode.CollisionTest:
                    return _pointTestNumber;
                case TestMode.CapacityTest:
                    return _treeCapacityTestNumber;
            }
            return 0;
        }

        public int GetTestValue()
        {
            switch (testMode)
            {
                case TestMode.CollisionTest:
                    if (_pointTestNumber < 0) return _pointTestNumber;
                    return _pointAmmountToTest[_pointTestNumber];
                case TestMode.CapacityTest:
                    if (_treeCapacityTestNumber < 0) return _treeCapacityTestNumber;
                    return _treeCapacitySizes[_treeCapacityTestNumber];
            }
            return -1;
        }

        public int GetNumberOfTests()
        {
            switch (testMode)
            {
                case TestMode.CollisionTest:
                    return _pointAmmountToTest.Length;
                case TestMode.CapacityTest:
                    return _treeCapacitySizes.Length;
            }
            return 0;
        }

        public void SetPointGenerator()
        {
            int pPointCount;
            Vector2i pSpawnArea;
            switch (testMode)
            {
                case TestMode.CollisionTest:
                    pPointCount = GetTestValue();
                    pSpawnArea = getSpawnArea();
                    _pointGenerator = new PointGenerator(pPointCount);
                    _pointGenerator.GenerateRandomPoints(pSpawnArea, seed);
                    _pointGenerator.UpdateUserData(Color.Red, treeManager.varyingPointSize);
                    break;

                case TestMode.CapacityTest:
                    pPointCount = _treeCapacityPointCount;
                    pSpawnArea = getSpawnArea();
                    _pointGenerator = new PointGenerator(pPointCount);
                    _pointGenerator.GenerateRandomPoints(pSpawnArea, seed);
                    _pointGenerator.UpdateUserData(Color.Red, treeManager.varyingPointSize);
                    break;
                default:
                    break;
            }
        }

        public PointGenerator GetPointGenerator()
        {
            return _pointGenerator;
        }

        public string GetTestDataFilePath()
        {
            return _testDataFilePaths[(int)testMode];
        }

        public void DrawTrees(GameTime pGameTime)
        {
            _noTreeTest.Draw(_window);
            _quadTreeTest.Draw(_window);
            _kdTreeTest.Draw(_window);
        }

        private void setTreeMode(TreeManager.TreeMode pTreemode)
        {
            switch (testMode)
            {
                case TestMode.CollisionTest:
                    _pointTestNumber = 0;

                    //set test settings
                    treeManager = new TreeManager();
                    treeManager.treeMode = pTreemode;
                    treeManager.collidingPoints = true;
                    treeManager.movingPoints = true;
                    treeManager.enableWindowBoundry = true;
                    consoleText = "Collision Test";
                    break;
                case TestMode.CapacityTest:
                    _treeCapacityTestNumber = 0;

                    //set test settings
                    treeManager = new TreeManager();
                    treeManager.treeMode = pTreemode;
                    treeManager.collidingPoints = true;
                    treeManager.movingPoints = true;
                    treeManager.enableWindowBoundry = true;
                    consoleText = "Capacity Test";
                    break;
            }
        }

        private void updateTreeManager()
        {
            switch (testMode)
            {
                case TestMode.CollisionTest:
                    break;
                case TestMode.CapacityTest:
                    if (treeManager.treeMode == TreeManager.TreeMode.NoTree)
                    {
                        //if we are on the no tree we do not need to go though all the capacity tests
                        setTestNumber(GetNumberOfTests() - 1);
                    }
                    else
                    {
                        treeManager.treeCapacity = (uint)GetTestValue();
                    }
                    break;
            }
        }

        private void setTestNumber(int pNewTestNumber)
        {
            switch (testMode)
            {
                case TestMode.CollisionTest:
                    _pointTestNumber = pNewTestNumber;
                    break;
                case TestMode.CapacityTest:
                    _treeCapacityTestNumber = pNewTestNumber;
                    break;
            }
        }

        private Vector2i getSpawnArea()
        {
            switch (testMode)
            {
                case TestMode.CollisionTest:
                    _pointSpawnArea = (Vector2i)_window.Size;
                    break;
                case TestMode.CapacityTest:
                    _pointSpawnArea = (Vector2i)_window.Size;
                    break;
            }
            return _pointSpawnArea;
        }
    }
}
