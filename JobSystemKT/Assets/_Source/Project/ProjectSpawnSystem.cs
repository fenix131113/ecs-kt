using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static Project.ProjectSpawnAuthorization;
using Random = UnityEngine.Random;

namespace Project
{
    [BurstCompile]
    public partial struct ProjectSpawnSystem : ISystem
    {
        public struct SpawnedTag : IComponentData
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.HasSingleton<Spawner>())
                return;

            var spawnerEntity = SystemAPI.GetSingletonEntity<Spawner>();
            var spawner = SystemAPI.GetSingleton<Spawner>();

            if (state.EntityManager.HasComponent<SpawnedTag>(spawnerEntity))
                return;

            if (spawner.Prefab == null)
                return;

            var entityManager = state.EntityManager;
            var clones = entityManager.Instantiate(spawner.Prefab, spawner.Count, Allocator.Temp);

            var random = new Unity.Mathematics.Random((uint)(SystemAPI.Time.ElapsedTime * 1000 + 1));

            foreach (var e in clones)
            {
                var pos = LocalTransform.FromPosition(new float3(Random.Range(spawner.MinX, spawner.MaxX),
                    Random.Range(spawner.MinY, spawner.MaxY), 0));

                entityManager.SetComponentData(e, pos);
                entityManager.SetComponentData(e, new MoveTarget
                {
                    Value = new float3(
                        random.NextFloat(spawner.MinX, spawner.MaxX),
                        random.NextFloat(spawner.MinY, spawner.MaxY),
                        0f)
                });

                entityManager.SetComponentData(e, new Temperature { Value = random.NextFloat() });
            }

            entityManager.AddComponent<SpawnedTag>(spawnerEntity);
            clones.Dispose();
        }
    }
}