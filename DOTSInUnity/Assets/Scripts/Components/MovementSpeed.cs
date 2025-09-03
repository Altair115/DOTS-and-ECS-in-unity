using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Components
{
    /// <summary>
    /// ECS component
    /// </summary>
    public struct MovementSpeedData : IComponentData
    {
        public float Value;
    }

    /// <summary>
    /// Authoring MonoBehaviour (shows in inspector)
    /// </summary>
    public class MovementSpeed : MonoBehaviour
    {
        public float Value;
    }
    
    /// <summary>
    /// Baker converts the authoring MonoBehaviour to ECS component
    /// </summary>
    public class MovementSpeedBaker : Baker<MovementSpeed>
    {
        public override void Bake(MovementSpeed authoring)
        {
            // Get the entity associated with this GameObject
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // Add ECS component to that entity
            AddComponent(entity, new MovementSpeedData
            {
                Value = authoring.Value
            });
        }
    }
}