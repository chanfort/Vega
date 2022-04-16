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
    }
}
