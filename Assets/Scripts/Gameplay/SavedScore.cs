namespace URacing
{
    public class SavedScore
    {
        public int StarsCount;
        public float Time;

        public SavedScore(int starsCount, float time)
        {
            StarsCount = starsCount;
            Time = time;
        }

        public SavedScore()
        {
            StarsCount = 0;
            Time = float.MaxValue;
        }
    }
}