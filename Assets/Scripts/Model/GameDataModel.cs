using UnityEngine;

namespace AppsULove.TestGame
{
    public class GameDataModel
    {
        private const string DistanceKey = "DistanceKey";
        private const string ScoreKey = "ScoreKey";
        public float Distance { get; private set; }

        public int Score { get; private set; }

        public GameDataModel() 
        {
            LoadData();
        }

        public void UpdateScore()
        {
            Score++;
        }

        public void PlayerMoved(float distance)
        {
            Distance += distance;
        }

        public void LoadData()
        {
            Distance = PlayerPrefs.GetFloat(DistanceKey);
            Score = PlayerPrefs.GetInt(ScoreKey);
        }

        public void SaveData()
        {
            PlayerPrefs.SetFloat(DistanceKey, Distance);
            PlayerPrefs.SetInt(ScoreKey, Score);
        }
    }
}
