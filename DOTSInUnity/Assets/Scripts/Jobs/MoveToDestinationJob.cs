using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
    public partial struct MoveToDestinationJob : IJobEntity
    {
        public float DeltaTime;

        private void Execute(ref LocalTransform transform, in DestinationData destination, in MovementSpeedData speed)
        {
            float3 currentPos = transform.Position;
            float3 targetPos  = destination.Value;
            float3 direction = targetPos - currentPos;

            // Already there (or extremely close)
            if(math.all(targetPos == currentPos)){return;}

            float3 movement = math.normalize(direction) * speed.Value *  DeltaTime;

            // Face the destination
            transform.Rotation = quaternion.LookRotationSafe(direction, math.up());

            // Step toward destination (snap if overshooting)
            if (math.length(movement) >= math.length(direction))
                transform.Position = targetPos;
            else
                transform.Position = currentPos + movement;
        }
    }
}
