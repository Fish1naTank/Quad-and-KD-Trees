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

        public static void DrawPreformanceData(GameLoop pGameLoop, RenderWindow pWindow, String pConsoleText, int pPointcount, Color pFontColor)
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

            Text pointCountText = new Text($"{pPointcount}", consoleFont, 14);
            pointCountText.Position = new Vector2f(4, 68);
            pointCountText.Color = pFontColor;

            Text consoleMessageText = new Text(pConsoleText, consoleFont, 16);
            consoleMessageText.Position = new Vector2f(4, pWindow.Size.Y - 20);
            consoleMessageText.Color = pFontColor;

            pGameLoop.window.Draw(timeText);
            pGameLoop.window.Draw(deltaTimeText);
            pGameLoop.window.Draw(fpsText);
            pGameLoop.window.Draw(pointCountText);
            pGameLoop.window.Draw(consoleMessageText);
        }
    }
}
