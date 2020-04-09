using System;
using SFML.System;
using SFML.Graphics;

namespace Quad_and_KD_Trees
{
    public static class DebugUtility
    {
        public const string CONSOLE_FONT_PATH = "./fonts/arial.ttf";

        public static Font consoleFont;

        public static void LoadContent()
        {
            consoleFont = new Font(CONSOLE_FONT_PATH);
        }

        public static void DrawPreformanceData(GameLoop pGameLoop, Color pFontColor)
        {
            if (consoleFont == null) return;

            string totalTimeElapsedStr = pGameLoop.gameTime.TotalTimeElapesd.ToString("0.000");
            string deltaTimeStr = pGameLoop.gameTime.DeltaTime.ToString("0.00000");

            float fps = 1 / pGameLoop.gameTime.DeltaTime;
            string fpsStr = fps.ToString("0.00");

            Text timeText = new Text(totalTimeElapsedStr, consoleFont, 14);
            timeText.Position = new Vector2f(4, 8);
            timeText.Color = pFontColor;

            Text deltaTimeText = new Text(deltaTimeStr, consoleFont, 14);
            deltaTimeText.Position = new Vector2f(4, 28);
            deltaTimeText.Color = pFontColor;

            Text fpsText = new Text(fpsStr, consoleFont, 14);
            fpsText.Position = new Vector2f(4, 48);
            fpsText.Color = pFontColor;

            pGameLoop.window.Draw(timeText);
            pGameLoop.window.Draw(deltaTimeText);
            pGameLoop.window.Draw(fpsText);
        }
    }
}
