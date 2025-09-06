using Unity.Burst;
using Unity.Physics;

namespace Jobs
{
    [BurstCompile]
    public struct PersonCollisionJob : ITriggerEventsJob
    {
        public void Execute(TriggerEvent triggerEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}