using UnityEngine;

namespace Vega
{
    public class GameBounds : MonoBehaviour
    {
        public static GameBounds instance;

        public float minX;
        public float maxX;
        public float minY;
        public float maxY;

        void Awake()
        {
            instance = this;
            float camSize = Camera.main.orthographicSize;

            if (Screen.width > Screen.height)
            {
                float ratio = (1f * Screen.width) / Screen.height;

                minX = -camSize * ratio;
                maxX = camSize * ratio;
                minY = -camSize;
                maxY = camSize;
            }
            else
            {
                float ratio = (1f * Screen.height) / Screen.width;

                minX = -camSize;
                maxX = camSize;
                minY = -camSize * ratio;
                maxY = camSize * ratio;
            }

            Application.targetFrameRate = 60;
        }
    }
}
