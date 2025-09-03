using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Components
{
    /// <summary>
    /// ECS component
    /// </summary>
    public struct DestinationStruct : IComponentData
    {
        public float3 Value;
    }

    /// <summary>
    /// Authoring MonoBehaviour (shows in inspector)
    /// </summary>
    public class Destination : MonoBehaviour
    {
        public float3 Value;
    }
    
    /// <summary>
    /// Baker converts the authoring MonoBehaviour to ECS component
    /// </summary>
    public class DestinationBaker : Baker<Destination>
    {
        public override void Bake(Destination authoring)
        {
            // Get the entity associated with this GameObject
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // Add ECS component to that entity
            AddComponent(entity, new DestinationStruct
            {
                Value = authoring.Value
            });
        }
    }
}