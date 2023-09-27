namespace AethicalTools
{
    public struct Ticker
    {
        private float resetTime;
        private float time;
        public Ticker(float maxTime, bool windUpOnStart)
        {
            resetTime = 0f;
            time = 0f;
            ChangeMaxTime(maxTime);
            if (windUpOnStart) WindUp();
        }
        /// <summary>
        /// Set the time to maxTime.
        /// </summary>
        public void WindUp()
        {
            time = resetTime;
        }
        /// <summary>
        /// Set the time to 0.
        /// </summary>
        public void WindDown()
        {
            time = 0f;
        }
        /// <summary>
        /// Set the maxTime to a new value.
        /// </summary>
        public Ticker ChangeMaxTime(float maxTime)
        {
            resetTime = maxTime;
            return this;
        }
        /// <summary>
        /// Reduce the time by deltaTime.
        /// </summary>
        /// <value> True if the time is below 0. </value>
        public bool Tick(float deltaTime)
        {
            time -= deltaTime;
            return CheckDone();
        }
        /// <summary>
        /// Reduce the time by deltaTime.
        /// Loops the time back through maxTime after returning True.
        /// </summary>
        /// <value> True if the time is below 0. </value>
        public bool LoopingTick(float deltaTime)
        {
            time -= deltaTime;
            if (CheckDone())
            {
                time += resetTime;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Check the status of the timer.
        /// </summary>
        /// <value> True if the time is below 0. </value>
        public bool CheckDone()
        {
            return time < 0f;
        }
        /// <summary>
        /// Check the status of the timer in percentage.
        /// </summary>
        /// <value> Between the range of 0f to 1f. </value>
        public float PercentStatus()
        {
            if (CheckDone()) return 0f;
            else return time / resetTime;
        }
    }
}