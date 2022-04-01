using UnityEngine;

namespace Vega
{
    public class AiController : MonoBehaviour
    {
        public float starCreationCooldown = 0.5f;
        float currentCooldown;
        public int maxStars = 1000;

        void Start()
        {

        }

        void Update()
        {
            CreateStar(Time.deltaTime);
        }

        void CreateStar(float dt)
        {
            currentCooldown -= dt;

            if (currentCooldown < 0f && AnnihilationSystem.instance.antimatterStars.Count <= maxStars)
            {
                currentCooldown = starCreationCooldown;
                Vector2 pos = new Vector2(Random.Range(GameBounds.instance.minX, GameBounds.instance.maxX), GameBounds.instance.maxY);
                StarSystem.instance.Add(1, pos);
            }
        }
    }
}
