using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace Quad_and_KD_Trees
{
    public abstract class GameLoop
    {
        public const int TARGET_FPS = 60;
        public const float TIME_UNTIL_UPDATE = 1f / TARGET_FPS;

        public RenderWindow window { get; protected set; }

        public GameTime gameTime { get; protected set; }

        public Color windowClearColor { get; protected set; }

        protected GameLoop(uint pWindowWidth, uint pWindowHeight, string pWindowTitle, Color pWindowClearColor)
        {
            this.windowClearColor = pWindowClearColor;
            this.window = new RenderWindow(new VideoMode(pWindowWidth, pWindowHeight), pWindowTitle);
            this.gameTime = new GameTime();
            window.Closed += WindowClosed;
        }

        public void Run()
        {
            LoadContent();
            Initialize();

            float totalTimeBeforeUpdate = 0f;
            float previousTimeElapsed = 0f;
            float deltaTime = 0f;
            float totalTimeElapsed = 0f;

            //starts the clock
            Clock clock = new Clock();

            //game loop
            while(window.IsOpen)
            {
                window.DispatchEvents();

                //calculate time
                totalTimeElapsed = clock.ElapsedTime.AsSeconds();
                deltaTime = totalTimeElapsed - previousTimeElapsed;
                previousTimeElapsed = totalTimeElapsed;

                totalTimeBeforeUpdate += deltaTime;

                if(totalTimeBeforeUpdate >= TIME_UNTIL_UPDATE)
                {
                    //update game time
                    gameTime.Update(totalTimeBeforeUpdate, clock.ElapsedTime.AsSeconds());
                    totalTimeBeforeUpdate = 0;

                    Update(gameTime);

                    //render
                    window.Clear(windowClearColor);
                    Draw(gameTime);
                    window.Display();
                }
            }
        }

        public abstract void LoadContent();
        public abstract void Initialize();
        public abstract void Update(GameTime pGameTime);
        public abstract void Draw(GameTime pGameTime);

        private void WindowClosed(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}
