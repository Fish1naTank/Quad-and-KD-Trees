using System;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;

namespace Quad_and_KD_Trees
{
    abstract class TreeTest
    {
        public bool TestCompleate { get; protected set; }
        public int TestID { get; protected set; }
        public FPSTracker fpsTracker { get; protected set; }

        protected float _testDuration;
        protected double _testStartTime;
        protected PointGenerator _pointGenerator;

        public abstract void InitializeTest(int pTestNumber, float pTestTime, PointGenerator pPointGenerator);
        public abstract void Update(GameTime pGameTime);
        public abstract void WriteTestStatsToFile(string pFilename);
    }
}
