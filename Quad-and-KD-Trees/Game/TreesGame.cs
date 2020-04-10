using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using System.Collections.Generic;

namespace Quad_and_KD_Trees
{
    class TreesGame : GameLoop
    {
        public const uint DEFAULT_WINDOW_WIDTH = 1080;
        public const uint DEFAULT_WINDOW_HEIGHT = 720;
        public const string WINDOW_TITLE = "Quad and KD Trees";

        public PointGenerator pointGenerator;
        public QuadTree quadTree;

        MouseBox mouseBox;

        public TreesGame() : base(DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, WINDOW_TITLE, Color.Black)
        {

        }

        public override void LoadContent()
        {
            DebugUtility.LoadContent();
        }

        public override void Initialize()
        {
            pointGenerator = new PointGenerator(500);
            pointGenerator.Generate((Vector2i)window.Size);

            Boundry quadTreeBoundry = new Boundry(window.Size.X / 2, window.Size.Y / 2, window.Size.X, window.Size.Y);
            quadTree = new QuadTree(quadTreeBoundry, 1);

            foreach(Point p in pointGenerator.GetPoints())
            {
                quadTree.Insert(p);
            }

            mouseBox = new MouseBox(new Vector2f(0, 0), new Vector2f(100, 100));
        }

        public override void Update(GameTime pGameTime)
        {
            mouseBox.Update(window);

            mouseBox.pointsFound = quadTree.QueryRectangelRange(mouseBox);
        }

        public override void Draw(GameTime pGameTime)
        {
            DebugUtility.DrawPreformanceData(this, Color.White);
            quadTree.DrawTree(window, Color.White);
            quadTree.DrawPoints(window, Color.White);
            mouseBox.Draw(window, Color.Green);
        }
    }
}
