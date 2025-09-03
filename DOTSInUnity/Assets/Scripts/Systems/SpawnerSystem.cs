using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    /// <summary>
    /// Spawns a grid of entity instances from the baked prefab, once.
    /// Runs in Initialization so entities exist before your Simulation systems.
    /// </summary>
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [BurstCompile]
    partial struct SpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SpawnerData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // Record structural changes (instantiation, adds/sets) safely
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            // Simple (non-deterministic) seed for a one-shot spawn
            var rng = new Unity.Mathematics.Random(
                (uint)(0x9E3779B9u ^ (int)(SystemAPI.Time.ElapsedTime * 1000.0)));

            foreach (var (spawnerRO, spawnerEntity) in
                     SystemAPI.Query<RefRO<SpawnerData>>()
                         .WithNone<SpawnerInitialized>()
                         .WithEntityAccess())
            {
                var spawner = spawnerRO.ValueRO;

                // Pull fields into readable locals
                Entity prefab = spawner.Prefab;
                int gridSize = spawner.GridSize;
                int spread = spawner.Spread;
                float speedMin = spawner.SpeedRange.x;
                float speedMax = spawner.SpeedRange.y;

                for (int x = 0; x < gridSize; x++)
                for (int z = 0; z < gridSize; z++)
                {
                    // Create an instance of the *entity prefab*
                    Entity instance = ecb.Instantiate(prefab);

                    // Place it in a grid
                    float3 position = new float3(x * spread, 0, z * spread);
                    ecb.SetComponent(instance, LocalTransform.FromPositionRotationScale(
                        position, quaternion.identity, 1f));

                    // Keep the prefab's DestinationData as-is (DO NOT set it here)

                    // Override ONLY the speed the prefab already has
                    // (Use SetComponent because MovementSpeedData exists on the prefab)
                    float randomSpeed = rng.NextFloat(speedMin, speedMax);
                    ecb.SetComponent(instance, new MovementSpeedData { Value = randomSpeed });
                }

                // Mark this spawner so it doesn't run again
                ecb.AddComponent<SpawnerInitialized>(spawnerEntity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        
        }
    }
}
