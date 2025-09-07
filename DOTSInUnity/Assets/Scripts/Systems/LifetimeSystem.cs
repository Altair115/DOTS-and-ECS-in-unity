using Components;
using Jobs;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

namespace Systems
{
    partial struct LifetimeSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            // Only run when there's at least one Lifetime in the world.
            state.RequireForUpdate<LifetimeData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            // Managed ECB creation (hidden from Burst)
            var ecb = CreateEndSimParallelEcb(out var endSimEcbSystem, state.World);

            // Schedule the job in parallel
            state.Dependency = new LifetimeJob
            {
                DeltaTime = deltaTime,
                Ecb = ecb
            }.ScheduleParallel(state.Dependency);

            // Managed registration (hidden from Burst)
            RegisterEcbDependency(endSimEcbSystem, state.Dependency);
        
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        
        }
        
        // --------- Managed helpers (Burst ignores their bodies) --------- //
        
        [BurstDiscard]
        private static EntityCommandBuffer.ParallelWriter CreateEndSimParallelEcb(
            out EndSimulationEntityCommandBufferSystem endSimEcbSystem, World world)
        {
            endSimEcbSystem = world.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
            return endSimEcbSystem.CreateCommandBuffer().AsParallelWriter();
        }

        [BurstDiscard]
        private static void RegisterEcbDependency(EndSimulationEntityCommandBufferSystem sys, JobHandle dep)
        {
            sys.AddJobHandleForProducer(dep);
        }
    }
}
