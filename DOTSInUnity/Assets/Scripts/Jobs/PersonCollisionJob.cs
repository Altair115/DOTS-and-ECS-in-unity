using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;

namespace Jobs
{
    [BurstCompile]
    public struct PersonCollisionJob : ITriggerEventsJob
    {
        // Read-only tag lookup: is this entity a "person"?
        [ReadOnly] public ComponentLookup<PersonTagData> PersonLookup; // set ReadOnly = true when you fetch it

        // Read-write color lookup: per-entity URP base color
        public ComponentLookup<URPMaterialPropertyBaseColor> ColorLookup; // RW
        
        // A per-frame seed to vary colors over time (optional)
        public uint FrameSeed;
        
        public void Execute(TriggerEvent triggerEvent)
        {
            var a = triggerEvent.EntityA;
            var b = triggerEvent.EntityB;

            // Only react if BOTH are persons (same logic as the tutorial)
            if (!PersonLookup.HasComponent(a) || !PersonLookup.HasComponent(b))
                return;
            
            // Build a deterministic seed from the pair + frame
            uint pairSeed = math.hash(new int2(a.Index, b.Index)) ^ (FrameSeed | 1u);
            var rng = Random.CreateFromIndex(pairSeed);
            
            // Recolor both (if they have the URP color component)
            RecolorIfPresent(a, ref rng);
            RecolorIfPresent(b, ref rng);
        }
        
        private void RecolorIfPresent(Entity e, ref Unity.Mathematics.Random rng)
        {
            if (!ColorLookup.HasComponent(e)) return;

            var c = ColorLookup[e];
            c.Value = new float4(
                rng.NextFloat(0f, 1f),
                rng.NextFloat(0f, 1f),
                rng.NextFloat(0f, 1f),
                1f);
            ColorLookup[e] = c; // write back
        }
    }
}