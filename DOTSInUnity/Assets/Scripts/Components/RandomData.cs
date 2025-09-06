using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    public struct RandomData : IComponentData
    {
        public Random Rng;
    }
}
