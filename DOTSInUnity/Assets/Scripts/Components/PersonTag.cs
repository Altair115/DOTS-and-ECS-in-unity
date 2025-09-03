using Unity.Entities;
using UnityEngine;

namespace Components
{
    /// <summary>
    /// ECS component
    /// </summary>
    public struct PersonTagData : IComponentData
    {
        
    }
    
    /// <summary>
    /// Authoring MonoBehaviour (shows in inspector)
    /// </summary>
    public class PersonTag : MonoBehaviour
    {
        
    }
    
    /// <summary>
    /// Baker converts the authoring MonoBehaviour to ECS component
    /// </summary>
    public class PersonTagBaker : Baker<PersonTag>
    {
        public override void Bake(PersonTag authoring)
        {
            // Get the entity associated with this GameObject
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            // Add ECS component to that entity
            AddComponent(entity, new PersonTagData
            {
                
            });
        }
    }
}