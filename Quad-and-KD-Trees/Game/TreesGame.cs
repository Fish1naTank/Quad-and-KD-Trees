using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;

namespace Quad_and_KD_Trees
{
    class TreesGame : GameLoop
    {
        public const uint DEFAULT_WINDOW_WIDTH = 1080;
        public const uint DEFAULT_WINDOW_HEIGHT = 720;
        public const string WINDOW_TITLE = "Quad and KD Trees";

        public TreesGame() : base(DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, WINDOW_TITLE, Color.Black)
        {

        }

        public override void LoadContent()
        {
            DebugUtility.LoadContent();
        }

        public override void Initialize()
        {
        }

        public override void Update(GameTime pGameTime)
        {
        }

        public override void Draw(GameTime pGameTime)
        {
            DebugUtility.DrawPreformanceData(this, Color.White);
        }
    }
}
