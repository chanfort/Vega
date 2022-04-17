using System.Collections.Generic;
using UnityEngine;

namespace Vega
{
    public class StarSystem : MonoBehaviour
    {
        public static StarSystem instance;

        [HideInInspector] public List<StarController> starInstances = new List<StarController>();
        [HideInInspector] public List<Vector2> starPositions = new List<Vector2>();

        void Awake()
        {
            instance = this;
        }

        void Update()
        {
            float dt = Time.deltaTime;
            Move(dt);
        }

        void Move(float dt)
        {
            for (int i = 0; i < starInstances.Count; i++)
            {
                StarController st = starInstances[i];

                Vector2 v = st.velocity;
                Vector2 pos = starPositions[i];
                Vector2 newPos = pos + v * dt;

                starPositions[i] = newPos;
                st.transform.position = newPos;
            }

            for (int i = 0; i < starInstances.Count; i++)
            {
                Vector2 pos = starPositions[i];
                StarController st = starInstances[i];

                if (
                    pos.x < GameBounds.instance.minX - 10f ||
                    pos.x > GameBounds.instance.maxX + 10f ||
                    pos.y < GameBounds.instance.minY - 10f ||
                    pos.y > GameBounds.instance.maxY + 10f
                )
                {
                    Remove(i);
                    Destroy(st.gameObject);
                }
            }
        }

        public void Add(Vector3 pos, StarController starPrefab, Vector2 directionalVelocity, float turbulence, bool isAntimatter, int playerId)
        {
            StarController star = Instantiate(starPrefab, pos, Quaternion.identity);
            star.starSystemIndex = starInstances.Count;
            star.velocity = directionalVelocity + Random.insideUnitCircle * turbulence;
            star.isAntimatter = isAntimatter;
            star.playerId = playerId;

            starInstances.Add(star);
            starPositions.Add(pos);

            AnnihilationSystem.instance.Add(star);
        }

        public void Remove(int i)
        {
            StarController star = starInstances[i];

            starInstances[i] = starInstances[starInstances.Count - 1];
            starPositions[i] = starPositions[starInstances.Count - 1];
            starInstances[i].starSystemIndex = i;

            starInstances.RemoveAt(starInstances.Count - 1);
            starPositions.RemoveAt(starPositions.Count - 1);

            AnnihilationSystem.instance.Remove(star);
        }

        public void RemoveAnnihilated()
        {
            for (int i = 0; i < starInstances.Count; i++)
            {
                StarController star = starInstances[i];

                if(star.markAnnihilated)
                {
                    Remove(i);
                    i--;

                    Destroy(star.gameObject);
                }
            }
        }
    }
}
