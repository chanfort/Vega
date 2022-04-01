using UnityEngine;

namespace Vega
{
    public class GameOver : MonoBehaviour
    {
        public static GameOver instance;
        public GameObject gameOverPanel;

        void Awake()
        {
            instance = this;
        }

        public void TriggerGameOver()
        {
            gameOverPanel.SetActive(true);
        }
    }
}
