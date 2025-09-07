using Components;
using Unity.Burst;
using Unity.Entities;

namespace Jobs
{
    [BurstCompile]
    public partial struct LifetimeJob : IJobEntity
    {
        
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter Ecb;

        private void Execute([EntityIndexInQuery] int sortKey, Entity entity, ref LifetimeData lifetime)
        {
            lifetime.Value -= DeltaTime;
            if (lifetime.Value <= 0f)
            {
                Ecb.DestroyEntity(sortKey, entity);
            }
        }
    }
}
