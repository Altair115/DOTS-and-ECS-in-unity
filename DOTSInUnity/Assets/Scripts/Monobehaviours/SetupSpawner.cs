using Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Monobehaviours
{
    public class SetupSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject personPrefab;
        [SerializeField] private int gridSize = 5;
        [SerializeField] private int spread = 3;
        [SerializeField] private Vector2 speedRange = new Vector2(4f, 7f);
        [SerializeField] private Vector2 lifetimeRange = new Vector2(10f, 60f);
        
        // Public getters for the baker
        public GameObject PersonPrefab => personPrefab;
        public int GridSize => gridSize;
        public int Spread => spread;
        public float2 SpeedRange => new float2(speedRange.x, speedRange.y);
        public float2 LifetimeRange => new float2(lifetimeRange.x, lifetimeRange.y);
    }
    
    /// <summary>Bakes authoring → ECS runtime data (runs during Baking).</summary>
    public class SetupSpawnerBaker : Baker<SetupSpawner>
    {
        public override void Bake(SetupSpawner authoring)
        {
            // This entity represents the spawner “controller”
            var spawnerEntity = GetEntity(TransformUsageFlags.None);

            // Convert the referenced GameObject prefab into an *Entity* prefab
            var prefabEntity = GetEntity(authoring.PersonPrefab, TransformUsageFlags.Dynamic);

            AddComponent(spawnerEntity, new SpawnerData
            {
                Prefab = prefabEntity,
                GridSize = authoring.GridSize,
                Spread = authoring.Spread,
                SpeedRange = authoring.SpeedRange,
                LifetimeRange = authoring.LifetimeRange
            });
            // Don't add SpawnerInitialized here — the system will tag it after it spawns.
        }
    }
}