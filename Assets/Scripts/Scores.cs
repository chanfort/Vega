using UnityEngine;
using UnityEngine.UI;

namespace Vega
{
    public class Scores : MonoBehaviour
    {
        public static Scores instance;

        public Text text;
        int score;

        void Awake()
        {
            instance = this;
        }

        public void AddToScore()
        {
            score++;
            text.text = score.ToString();
        }
    }
}
