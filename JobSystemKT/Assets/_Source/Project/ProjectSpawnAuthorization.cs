using Unity.Entities;
using UnityEngine;

namespace Project
{
    public class ProjectSpawnAuthorization : MonoBehaviour
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public int Count { get; private set; }
        [field: SerializeField] public float MinX { get; private set; }
        [field: SerializeField] public float MaxX { get; private set; }
        [field: SerializeField] public float MinY { get; private set; }
        [field: SerializeField] public float MaxY { get; private set; }

        public struct Spawner : IComponentData
        {
            public Spawner(Entity prefab, int count, float minX, float maxX, float minY, float maxY)
            {
                Prefab = prefab;
                Count = count;
                MinX = minX;
                MaxX = maxX;
                MinY = minY;
                MaxY = maxY;
            }

            public Entity Prefab { get; private set; }
            public int Count { get; private set; }

            public float MinX { get; private set; }
            public float MaxX { get; private set; }
            public float MinY { get; private set; }
            public float MaxY { get; private set; }
        }

        private class SpawnBaker : Baker<ProjectSpawnAuthorization>
        {
            public override void Bake(ProjectSpawnAuthorization authoring)
            {
                var spawner = GetEntity(TransformUsageFlags.None);
                var prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic | TransformUsageFlags.Renderable);

                AddComponent(spawner,
                    new Spawner(prefab, authoring.Count, authoring.MinX, authoring.MaxX, authoring.MinY,
                        authoring.MaxY));
            }
        }
    }
}