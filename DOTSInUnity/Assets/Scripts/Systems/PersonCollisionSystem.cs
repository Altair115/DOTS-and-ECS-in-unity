using Jobs;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Systems
{
    partial struct PersonCollisionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            // Make sure the simulation exist before we run.
            state.RequireForUpdate<SimulationSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var sim = SystemAPI.GetSingleton<SimulationSingleton>();
            
            var job = new PersonCollisionJob();
            state.Dependency = job.Schedule(sim, state.Dependency);

        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        
        }
    }
}
