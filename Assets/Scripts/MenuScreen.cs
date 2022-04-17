using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vega
{
    public class MenuScreen : MonoBehaviour
    {
        public void OpenSinglePlayer()
        {
            SceneManager.LoadScene("SinglePlayer", LoadSceneMode.Single);
        }

        public void Open2vAI()
        {
            SceneManager.LoadScene("2vAI", LoadSceneMode.Single);
        }

        public void OpenPvP()
        {
            SceneManager.LoadScene("PvP", LoadSceneMode.Single);
        }
    }
}
