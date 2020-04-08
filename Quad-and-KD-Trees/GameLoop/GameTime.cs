using System;

namespace Quad_and_KD_Trees
{
    public class GameTime
    {
        private float _deltaTime = 0f;
        private float _timeScale = 1f;

        public float TimeScale
        {
            get { return _timeScale; }
            set { _timeScale = value; }
        }

        public float DeltaTime
        {
            get { return _deltaTime * _timeScale; }
            set { _deltaTime = value; }
        }

        public float DeltaTimeUnscaled
        {
            get { return _deltaTime; }
        }

        public float TotalTimeElapesd
        {
            get;
            private set;
        }

        public GameTime()
        {
        }

        public void Update(float pDeltaTime, float pTotalTimeElapesd)
        {
            _deltaTime = pDeltaTime;
            TotalTimeElapesd = pTotalTimeElapesd;
        }
    }
}
