using Components;
using Jobs;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;

namespace Systems
{
    partial struct PersonCollisionSystem : ISystem
    {
        // Cache the lookups; update them each frame before scheduling
        private ComponentLookup<PersonTagData> _personLk;
        private ComponentLookup<URPMaterialPropertyBaseColor> _colorLk;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            // Make sure the simulation exist before we run.
            state.RequireForUpdate<SimulationSingleton>();
            
            // Get lookups (true = ReadOnly)
            _personLk = state.GetComponentLookup<PersonTagData>(true);
            _colorLk  = state.GetComponentLookup<URPMaterialPropertyBaseColor>(false);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // Refresh lookups to this frame’s data
            _personLk.Update(ref state);
            _colorLk.Update(ref state);
            
            var sim = SystemAPI.GetSingleton<SimulationSingleton>();
            
            var job = new PersonCollisionJob
            {
                PersonLookup = _personLk,
                ColorLookup  = _colorLk,
                FrameSeed    = (uint)(SystemAPI.Time.ElapsedTime * 1000.0f) + 1u
            };
            state.Dependency = job.Schedule(sim, state.Dependency);

        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        
        }
    }
}
