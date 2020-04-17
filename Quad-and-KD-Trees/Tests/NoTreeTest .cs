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
            _testStartTime = -1;
        }

        public override void InitializeTest(int pTestID, float pTestDurration, PointGenerator pPointGenerator)
        {
            TestCompleate = false;
            fpsTracker = new FPSTracker();

            TestID = pTestID;
            _testDuration = pTestDurration;
            _pointGenerator = pPointGenerator;
        }

        public override void Update(GameTime pGameTime)
        {
            //stop if the test is over
            if (TestCompleate) return;

            //start timer
            if (_testStartTime < -1) _testStartTime = pGameTime.TotalTimeElapesd;

            //run test

            //collision test
            List<Point> pointList = _pointGenerator.GetPoints();
            foreach (Point p in pointList)
            {
                p.HandleCollision(pointList);
            }

            //trackFPS
            fpsTracker.UpdateCumulativeMovingAverageFPS(1 /pGameTime.DeltaTime);

            //check if test is over
            if (pGameTime.TotalTimeElapesd - _testStartTime > _testDuration)
            {
                //mark test as compleate
                TestCompleate = true;
            }
        }

        public override void WriteTestStatsToFile(string pFilepath)
        {
            //TreeMode, PointCount, AverageFrames
            string data = $"NoTree, {_pointGenerator.pointCount}, {fpsTracker.currentAvgFPS},";
            using (StreamWriter sw = File.AppendText(pFilepath))
            {
                sw.WriteLine(data);
            }
        }
    }
}
