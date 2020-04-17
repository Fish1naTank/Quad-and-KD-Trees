using System;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;
using System.IO;

namespace Quad_and_KD_Trees
{
    class KDTreeTest : TreeTest
    {
        private KDTree _kdTree;
        public KDTreeTest()
        {
            TestCompleate = true;
            TestID = -1;
            _testStartTime = -1;
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

            //start timer
            if (_testStartTime < 0) _testStartTime = pGameTime.TotalTimeElapesd;

            //run test
            #region //////////TESTS///////////
            //moving points
            if (_treeManager.movingPoints)
            {
                _pointGenerator.MovePoints(pGameTime, pWindow);

                //rebuild tree every frame
                _kdTree = new KDTree(_pointGenerator.GetPoints(), _treeManager.treeCapacity);
                _kdTree.GenerateTree();
            }
            else
            {
                //build the tree once
                if(_kdTree == null)
                {
                    _kdTree = new KDTree(_pointGenerator.GetPoints(), _treeManager.treeCapacity);
                    _kdTree.GenerateTree();
                }
            }

            //collisions
            if (_treeManager.collidingPoints)
            {
                foreach (Point p in _pointGenerator.GetPoints())
                {
                    if (_kdTree != null && p.userData != null)
                        p.HandleCollision(_kdTree.QueryRange(p.Boundry()));
                }
            }
            #endregion

            //trackFPS
            fpsTracker.UpdateCumulativeMovingAverageFPS(1 / pGameTime.DeltaTime);

            //check if test is over
            if (pGameTime.TotalTimeElapesd - _testStartTime > _testDuration)
            {
                //mark test as compleate
                resetTestTree();
            }
        }

        public override void WriteTestStatsToFile(string pFilepath, int pTestValue)
        {
            //TreeMode, PointCount, AverageFrames
            string data = $"KDTree, {pTestValue}, {fpsTracker.currentAvgFPS}";
            using (StreamWriter sw = File.AppendText(pFilepath))
            {
                sw.WriteLine(data);
            }
        }

        public override void Draw(RenderWindow pWindow)
        {
            if (_pointGenerator == null || _kdTree == null) return;
            _pointGenerator.DrawPoints(pWindow, Color.Red, _treeManager.varyingPointSize);
            _kdTree.DrawSplitLines(pWindow, (Vector2f)pWindow.Size, Color.White, new Vector2f(0,0));
        }

        protected override void resetTestTree()
        {
            _kdTree = null;

            TestCompleate = true;
            TestID = -1;
            _testStartTime = -1;
            _testDuration = -1;
            _treeManager = null;
            _pointGenerator = null;
        }
    }
}
