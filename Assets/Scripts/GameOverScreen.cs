using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vega
{
    public class GameOverScreen : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
        }
    }
}
