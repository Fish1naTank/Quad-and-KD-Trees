using System;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;
using System.IO;

namespace Quad_and_KD_Trees
{
    class QuadTreeTest : TreeTest
    {
        private QuadTree _quadTree;
        private RectBoundry _quadTreeBoundry;
        public QuadTreeTest(Vector2f pWindowSize)
        {
            TestCompleate = true;
            TestID = -1;
            _testStartTime = -1;

            _quadTreeBoundry = new RectBoundry(pWindowSize.X / 2, pWindowSize.Y / 2, pWindowSize.X, pWindowSize.Y);
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

                //rebuild tree
                _quadTree = new QuadTree(_quadTreeBoundry, _treeManager.treeCapacity);
                _quadTree.Insert(_pointGenerator.GetPoints());
            }
            else
            {
                //build the tree once
                if(_quadTree == null)
                {
                    _quadTree = new QuadTree(_quadTreeBoundry, _treeManager.treeCapacity);
                    _quadTree.Insert(_pointGenerator.GetPoints());
                }
            }

            //collisions
            if (_treeManager.collidingPoints)
            {
                foreach (Point p in _pointGenerator.GetPoints())
                {
                    if (_quadTree != null && p.userData != null)
                        p.HandleCollision(_quadTree.QueryRange(p.Boundry()));
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
            string data = $"QuadTree, {pTestValue}, {fpsTracker.currentAvgFPS},";
            using (StreamWriter sw = File.AppendText(pFilepath))
            {
                sw.WriteLine(data);
            }
        }

        public override void Draw(RenderWindow pWindow)
        {
            if (_pointGenerator == null || _quadTree == null) return;
            _pointGenerator.DrawPoints(pWindow, Color.Red, _treeManager.varyingPointSize);
            _quadTree.DrawTree(pWindow, Color.White);
        }

        protected override void resetTestTree()
        {
            _quadTree = null;

            TestCompleate = true;
            TestID = -1;
            _testStartTime = -1;
            _testDuration = -1;
            _treeManager = null;
            _pointGenerator = null;
        }
    }
}
