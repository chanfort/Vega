using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Vega
{
    public class IntroScreen : MonoBehaviour
    {
        public TextAsset textFile;
        public Text uiText;
        public float speed;

        void Start()
        {
            Application.targetFrameRate = 60;
            uiText.text = textFile.text;
        }

        void Update()
        {
            Vector3 oldPos = uiText.transform.position;
            uiText.transform.position = new Vector3(oldPos.x, oldPos.y + speed * Time.deltaTime, oldPos.z);

            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
        }
    }
}
