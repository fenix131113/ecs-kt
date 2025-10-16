using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace JobWithECS
{
    [BurstCompile]
    public partial struct SpawnSystem : ISystem
    {
        public struct SpawnedTag : IComponentData
        {
        }

        [BurstCompile]
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

            var maxRawLenght = Mathf.Sqrt(spawner.Count);
            var startX = -(maxRawLenght / 2 + (maxRawLenght - 1) * spawner.Offset);
            var x = startX;
            var z = startX;
            var rawCount = 0;
            
            foreach (var t in clones)
            {
                var pos = new float3(x, 0, z);
                rawCount++;
                x += 1 + spawner.Offset;
                if (rawCount > maxRawLenght)
                {
                    x = startX;
                    z += 1 + spawner.Offset;
                    rawCount = 0;
                }

                entityManager.SetComponentData(t, new Center { Value = pos });
                entityManager.SetComponentData(t, LocalTransform.FromPosition(pos));
            }

            entityManager.AddComponent<SpawnedTag>(spawnerEntity);
            clones.Dispose();
        }
    }
}