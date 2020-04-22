using System;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;
using System.IO;

namespace Quad_and_KD_Trees
{
    class NoTreeTest : TreeTest
    {
        public NoTreeTest()
        {
            TestCompleate = true;
            TestID = -1;
            _testTime = -1;
        }

        public override void InitializeTest(int pTestID, float pTestDurration, TreeManager pTreeManager, PointGenerator pPointGenerator)
        {
            TestCompleate = false;
            fpsTracker = new FPSTracker();

            TestID = pTestID;
            _testDuration = pTestDurration;
            _treeManager = pTreeManager;
            _pointGenerator = pPointGenerator;
        }

        public override void Update(GameTime pGameTime, RenderWindow pWindow)
        {
            //stop if the test is over
            if (TestCompleate) return;

            //run test
            #region //////////TESTS///////////
            //movePoints
            if (_treeManager.movingPoints)
            {
                _pointGenerator.MovePoints(pGameTime, pWindow, _treeManager.enableWindowBoundry);
            }

            //collision
            if (_treeManager.collidingPoints)
            {
                List<Point> pointList = _pointGenerator.GetPoints();
                foreach (Point p in pointList)
                {
                    p.HandleCollision(pointList);
                }
            }
            #endregion

            //trackFPS
            fpsTracker.UpdateCumulativeMovingAverageFPS(1 / pGameTime.DeltaTime);

            //count updates
            _testTime += 1;
            //check if test is over
            if (_testTime > _testDuration)
            {
                //mark test as compleate
                resetTestTree();
            }
        }

        public override void WriteTestStatsToFile(string pFilepath, int pTestValue)
        {
            //TreeMode, PointCount, AverageFrames
            string data = $"NoTree, {pTestValue}, {fpsTracker.currentAvgFPS}";
            using (StreamWriter sw = File.AppendText(pFilepath))
            {
                sw.WriteLine(data);
            }
        }

        public override void Draw(RenderWindow pWindow)
        {
            if (_pointGenerator == null) return;
            _pointGenerator.DrawPoints(pWindow, Color.Red);
        }

        protected override void resetTestTree()
        {
            TestCompleate = true;
            TestID = -1;
            _testTime = -1;
            _testDuration = -1;
            _treeManager = null;
            _pointGenerator = null;
        }
    }
}
