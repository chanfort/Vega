using UnityEngine;

namespace Vega
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 3f;
        public float starCreationCooldown = 0.3f;
        float currentCooldown;

        public KeyCode moveLeft = KeyCode.A;
        public KeyCode moveRight = KeyCode.D;
        public KeyCode shoot = KeyCode.E;

        public StarController prefab;
        public Vector2 directionalVelocity;
        public float turbulence;

        void Start()
        {
            transform.position = new Vector2(0f, GameBounds.instance.minY + 0.5f);
            AnnihilationSystem.instance.player = transform;
        }

        void Update()
        {
            Move(Time.deltaTime);
            CreateStar(Time.deltaTime);
        }

        void Move(float dt)
        {
            Vector2 v = Vector2.zero;

            if (Input.GetKey(moveLeft))
            {
                v += new Vector2(-1f, 0f) * dt * speed;
            }

            if (Input.GetKey(moveRight))
            {
                v += new Vector2(1f, 0f) * dt * speed;
            }

            Vector2 oldPos = transform.position;
            Vector2 newPos = oldPos + v;

            if (newPos.x < GameBounds.instance.minX + 0.5f)
            {
                newPos = new Vector2(GameBounds.instance.minX + 0.5f, newPos.y);
            }
            if (newPos.x > GameBounds.instance.maxX - 0.5f)
            {
                newPos = new Vector2(GameBounds.instance.maxX - 0.5f, newPos.y);
            }

            transform.position = newPos;
        }

        void CreateStar(float dt)
        {
            currentCooldown -= dt;

            if(currentCooldown < 0f && Input.GetKey(shoot))
            {
                currentCooldown = starCreationCooldown;
                StarSystem.instance.Add(transform.position, prefab, directionalVelocity, turbulence);
            }
        }
    }
}