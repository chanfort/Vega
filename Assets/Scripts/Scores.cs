using UnityEngine;
using UnityEngine.UI;

namespace Vega
{
    public class Scores : MonoBehaviour
    {
        public static Scores instance;

        public Text[] texts;
        public int[] score;

        void Awake()
        {
            instance = this;
        }

        public void AddToScore(int playerId)
        {
            if (playerId < 0 || playerId >= score.Length)
            {
                return;
            }

            score[playerId]++;
            texts[playerId].text = score[playerId].ToString();
        }

        public int GetScore()
        {
            int totalScore = 0;

            for (int i = 0; i < score.Length; i++)
            {
                totalScore += score[i];
            }

            return totalScore;
        }
    }
}
