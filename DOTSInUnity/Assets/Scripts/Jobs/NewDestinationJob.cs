using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Jobs
{
    public partial struct NewDestinationJob : IJobEntity
    {
        public float Threshold;     
        public float2 XRange;       
        public float2 ZRange;

        private void Execute(ref DestinationData destination, ref RandomData randomData, in  LocalTransform transform)
        {
            float3 pos   = transform.Position;
            float3 toDst = destination.Value - pos;
            
            if (math.lengthsq(toDst) <= Threshold * Threshold)
            {
                // Copy RNG locally (struct), advance it, then write back
                var rng = randomData.Rng;

                float newX = rng.NextFloat(XRange.x, XRange.y);
                float newZ = rng.NextFloat(ZRange.x, ZRange.y);

                destination.Value = new float3(newX, pos.y, newZ);
                randomData.Rng    = rng; // write back updated state
            }
        }
    }
}
