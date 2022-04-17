using UnityEngine;

namespace Vega
{
    public class AiController : MonoBehaviour
    {
        public float minCooldown = 0.1f;
        public float maxCooldown = 0.02f;

        float currentCooldown;

        public int minStars = 400;
        public int maxStars = 4000;
        public int maxScore = 50000;

        public StarController prefab;
        public Vector2 minDirectionalVelocity = new Vector2(0f, -0.2f);
        public Vector2 maxDirectionalVelocity = new Vector2(0f, -0.5f);
        public float turbulence;

        void Start()
        {
            AnnihilationSystem.instance.aiController = this;
        }

        void Update()
        {
            CreateStar(Time.deltaTime);
        }

        void CreateStar(float dt)
        {
            currentCooldown -= dt;

            float scoreFraction = Scores.instance.GetScore() * 1f / maxScore;

            if (scoreFraction > 1f)
            {
                scoreFraction = 1f;
            }

            float starLimit = Mathf.Lerp(minStars, maxStars, scoreFraction);
            Vector2 directionalVelocity = Vector2.Lerp(minDirectionalVelocity, maxDirectionalVelocity, scoreFraction);

            if (currentCooldown < 0f && AnnihilationSystem.instance.antimatterStars.Count <= starLimit)
            {
                currentCooldown = Mathf.Lerp(minCooldown, maxCooldown, scoreFraction);
                Vector2 pos = new Vector2(Random.Range(GameBounds.instance.minX, GameBounds.instance.maxX), GameBounds.instance.maxY);
                StarSystem.instance.Add(pos, prefab, directionalVelocity, turbulence, true, -1);
            }
        }
    }
}
