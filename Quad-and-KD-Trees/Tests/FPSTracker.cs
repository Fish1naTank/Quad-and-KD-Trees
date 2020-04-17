namespace Quad_and_KD_Trees
{
    class FPSTracker
    {
        public float currentAvgFPS { get; private set; }
        private int _qty = 0;

        public FPSTracker()
        {
            currentAvgFPS = 0;
        }

        public float UpdateCumulativeMovingAverageFPS(float newFPS)
        {
            ++_qty;
            currentAvgFPS += (newFPS - currentAvgFPS) / _qty;

            return currentAvgFPS;
        }
    }
}
