using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    /// <summary>Runtime ECS data for spawning.</summary>
    public struct SpawnerData : IComponentData
    {
        /// <summary>Entity prefab baked from a GameObject prefab.</summary>
        public Entity Prefab;

        public int    GridSize;
        public int    Spread;
        /// <summary>x = min speed, y = max speed.</summary>
        public float2 SpeedRange;
    }

    /// <summary>Tag to ensure we spawn only once.</summary>
    public struct SpawnerInitialized : IComponentData {}
}