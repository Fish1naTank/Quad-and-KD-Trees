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

        public TreesTester() : base(DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, WINDOW_TITLE, Color.Black)
        {

        }

        public override void LoadContent()
        {
            DebugUtility.LoadContent();
        }

        public override void Initialize()
        {
            _testTreeManager = new TestTreeManager(window, TreeManager.TreeMode.NoTree, TestTreeManager.TestMode.CollisionTest);
        }

        public override void Update(GameTime pGameTime)
        {
            _testTreeManager.UpdateTests(pGameTime);
        }

        public override void Draw(GameTime pGameTime)
        {
            _testTreeManager.DrawTrees(pGameTime);

            if (_testTreeManager.GetPointGenerator() != null)
            {
                DebugUtility.DrawPreformanceData(this, window, _testTreeManager.consoleText, _testTreeManager.GetPointGenerator().pointCount, Color.Red);
            }
        }
    }
}
