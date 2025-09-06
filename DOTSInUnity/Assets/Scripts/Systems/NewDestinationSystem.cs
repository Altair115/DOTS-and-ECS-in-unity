using Jobs;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems
{
    partial struct NewDestinationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new NewDestinationJob
            {
                Threshold = 0.1f,
                XRange    = new float2(0f, 500f),
                ZRange    = new float2(0f, 500f)
            };

            state.Dependency = job.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        
        }
    }
}
