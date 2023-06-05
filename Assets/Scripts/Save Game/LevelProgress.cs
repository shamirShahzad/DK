namespace DK
{
    [System.Serializable]
    public class LevelProgress
    {
        public int level;
        public int numberOfStars;
        public bool isCompleted;


        public LevelProgress(int level,int stars)
        {
            this.level = level;
            this.numberOfStars = stars;
        }

        public LevelProgress()
        {

        }
    }
}
