using Unity.Entities;
using UnityEngine;

namespace JobWithECS
{
    public class SpawnAuthorization : MonoBehaviour
    {
        public GameObject spawnPrefab;
        public int ObjectsCount;
        public float Offset;
    }

    public struct Spawner : IComponentData
    {
        public Entity Prefab;
        public int Count;
        public float Offset;
    }

    public class SpawnBaker : Baker<SpawnAuthorization>
    {
        public override void Bake(SpawnAuthorization authoring)
        {
            var spawner = GetEntity(TransformUsageFlags.None);
            var prefab = GetEntity(authoring.spawnPrefab, TransformUsageFlags.Dynamic | TransformUsageFlags.Renderable);

            AddComponent(spawner,
                new Spawner { Prefab = prefab, Count = authoring.ObjectsCount, Offset = authoring.Offset });
        }
    }
}