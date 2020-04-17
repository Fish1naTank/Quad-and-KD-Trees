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
        public TreeManager treeManager = new TreeManager();
        public int testTime = 1;
        public string consoleText = "";

        public static int NumOfTests = 2;
        public enum TestMode { CollisionTest, CapacityTest }
        //starting test
        public TestMode testMode = TestMode.CollisionTest;

        //these are in the order of the tests above
        private string[] _testDataFilePaths = new string[2] { "CollisionTests.txt", "CapacityTest.txt" };

        //test paramaters
        public PointGenerator _pointGenerator;
        public Vector2i _pointSpawnArea;

        //Test Values
        private int[] _pointAmmountToTest = new int[] { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600 };
        //smol test
        //private int[] _pointAmmountToTest = new int[] { 100 };
        private int _pointTestNumber = -1;

        private int[] _treeCapacitySizes = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
        private int _treeCapacityTestNumber = -1;

        private RenderWindow _window;


        public TestTreeManager(RenderWindow pWindow, TreeManager.TreeMode pTreeToStart)
        {
            _window = pWindow;
            treeManager.treeMode = pTreeToStart;

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
                SetTreeMode(treeManager.treeMode);

                //title dataset
                using (StreamWriter sw = File.AppendText(GetTestDataFilePath()))
                {
                    sw.WriteLine($"{treeManager.treeMode.ToString()} {testMode.ToString()}");
                }
            }
        }

        private void SetTreeMode(TreeManager.TreeMode pTreemode)
        {
            //check if we 

            switch (testMode)
            {
                case TestMode.CollisionTest:
                    _pointTestNumber = 0;

                    //set test settings
                    treeManager = new TreeManager();
                    treeManager.treeMode = pTreemode;
                    treeManager.collidingPoints = true;
                    treeManager.movingPoints = true;
                    consoleText = "Collision Test";
                    break;
                case TestMode.CapacityTest:
                    _treeCapacityTestNumber = 0;

                    //set test settings
                    treeManager = new TreeManager();
                    treeManager.treeMode = pTreemode;
                    treeManager.collidingPoints = true;
                    treeManager.movingPoints = true;
                    consoleText = "Capacity Test";
                    break;
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

                        //END TESTS 
                        //IMPLEMENT CAPACITYTEST
                        if (testMode == TestMode.CapacityTest)
                        { 
                            _window.Close();
                        }
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

        public PointGenerator GetPointGenerator()
        {
            switch (testMode)
            {
                case TestMode.CollisionTest:
                    int pPointCount = GetTestValue();
                    Vector2i pSpawnArea = getSpawnArea();
                    _pointGenerator = new PointGenerator(pPointCount);
                    _pointGenerator.GenerateRandomPoints(pSpawnArea);
                    _pointGenerator.UpdateUserData(Color.Red, treeManager.varyingPointSize);
                    return _pointGenerator;
                case TestMode.CapacityTest:
                    break;
                default:
                    break;
            }
            return null;
        }

        public string GetTestDataFilePath()
        {
            return _testDataFilePaths[(int)testMode];
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
