using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Vega
{
    public class GravitySystem : MonoBehaviour
    {
        public bool canSwitchToRegular;
        bool useRegular;
        public float forceCoeficient = 0.02f;

        void Start()
        {

        }

        void Update()
        {
            if (canSwitchToRegular && Input.GetKeyDown(KeyCode.G))
            {
                useRegular = !useRegular;
            }

            float dt = Time.deltaTime;

            if (useRegular)
            {
                UpdateRegular(dt);
            }
            else
            {
                UpdateJobified(dt);
            }
        }

        void UpdateJobified(float dt)
        {
            List<Vector2> instancePositions = StarSystem.instance.starPositions;
            List<StarController> instances = StarSystem.instance.starInstances;

            int n = instancePositions.Count;

            NativeArray<float2> positions = new NativeArray<float2>(n, Allocator.Persistent);
            NativeArray<float2> velocities = new NativeArray<float2>(n, Allocator.Persistent);

            for (int i = 0; i < n; i++)
            {
                positions[i] = instancePositions[i];
                velocities[i] = instances[i].velocity;
            }

            new CalculateStep
            {
                positions = positions,
                velocities = velocities,
                nTotal = n,
                forceCoeficient = forceCoeficient,
                dt = dt
            }.Schedule(n, System.Environment.ProcessorCount).Complete();

            for (int i = 0; i < n; i++)
            {
                instances[i].velocity = velocities[i];
            }

            positions.Dispose();
            velocities.Dispose();
        }

        void UpdateRegular(float dt)
        {
            List<Vector2> instancePositions = StarSystem.instance.starPositions;
            List<StarController> instances = StarSystem.instance.starInstances;

            int n = instancePositions.Count;

            for (int i = 0; i < n; i++)
            {
                Vector2 dv = Vector3.zero;
                Vector2 posi = instancePositions[i];

                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        Vector2 posj = instancePositions[j];
                        Vector2 r = posj - posi;

                        float rSq = r.sqrMagnitude + 0.01f;
                        float rSqrt = Mathf.Sqrt(rSq);

                        dv += 1f * r / (rSq * rSqrt);
                    }
                }

                instances[i].velocity += dt * forceCoeficient * dv;
            }
        }

        [BurstCompile]
        struct CalculateStep : IJobParallelFor
        {
            [NativeDisableParallelForRestriction][ReadOnly] public NativeArray<float2> positions;
            [NativeDisableParallelForRestriction] public NativeArray<float2> velocities;
            [ReadOnly] public int nTotal;
            [ReadOnly] public float forceCoeficient;
            [ReadOnly] public float dt;

            public void Execute(int i)
            {
                float2 dv = 0f;
                float2 posi = positions[i];

                for (int j = 0; j < nTotal; j++)
                {
                    if (i != j)
                    {
                        float2 r = positions[j] - posi;

                        float rSq = math.lengthsq(r) + 0.01f;
                        float rSqrt = math.sqrt(rSq);

                        dv += r / (rSq * rSqrt);
                    }
                }

                velocities[i] += dt * forceCoeficient * dv;
            }
        }
    }
}
