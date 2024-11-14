namespace sonadro.FloppyBird
{
    public class SaveFile
    {
        public int HighScore { get; set; }

        public SaveFile(int highScore)
        {
            HighScore = highScore;
        }
    }
}