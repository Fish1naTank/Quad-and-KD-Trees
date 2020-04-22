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
        protected double _testTime;
        protected TreeManager _treeManager;
        protected PointGenerator _pointGenerator;

        public abstract void InitializeTest(int pTestNumber, float pTestTime, TreeManager pTreeManager, PointGenerator pPointGenerator);
        public abstract void Update(GameTime pGameTime, RenderWindow pWindow);

        public abstract void WriteTestStatsToFile(string pFilename, int pTestValue);
        public abstract void Draw(RenderWindow pWindow);
        protected abstract void resetTestTree();
    }
}
