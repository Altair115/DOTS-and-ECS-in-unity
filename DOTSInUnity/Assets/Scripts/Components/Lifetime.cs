using Unity.Entities;
using UnityEngine;

namespace Components
{
    public struct LifetimeData : IComponentData
    {
        public float Value;
    }

    /// <summary>
    /// Authoring MonoBehaviour (shows in inspector)
    /// </summary>
    public class Lifetime : MonoBehaviour
    {
        public float value;
    }
    
    /// <summary>
    /// Baker converts the authoring MonoBehaviour to ECS component
    /// </summary>
    public class LifetimeBaker : Baker<Lifetime>
    {
        public override void Bake(Lifetime authoring)
        {
            // Get the entity associated with this GameObject
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // Add ECS component to that entity
            AddComponent(entity, new LifetimeData
            {
                Value = authoring.value
            });
        }
    }
}