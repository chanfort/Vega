using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Vega
{
    public class GameOverScreen : MonoBehaviour
    {
        public static string resultsToShow;
        public Text resultsText;

        void Start()
        {
            if(!string.IsNullOrEmpty(resultsToShow))
            {
                resultsText.text = resultsToShow;
            }
            else
            {
                resultsText.text = string.Empty;
            }
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }
}
