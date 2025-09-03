using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    partial struct MoveToDestinationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>())
            {
                // Forward direction in local space (z axis)
                float3 forward = math.mul(transform.ValueRO.Rotation, new float3(0, 0, 1));

                // Move forward in world space
                transform.ValueRW.Position += forward * deltaTime;
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        
        }
    }
}
