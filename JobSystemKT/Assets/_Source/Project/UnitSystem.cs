using Project;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using static Project.ProjectSpawnAuthorization;
using Random = UnityEngine.Random;

[BurstCompile]
public partial struct UnitSystem : ISystem
{
    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // var query = SystemAPI.QueryBuilder().WithAll<MoveTarget>().Build();
        // var entities = query.ToEntityArray(Allocator.Temp);
        var spawner = SystemAPI.GetSingleton<Spawner>();

        var entities = state.EntityManager.CreateEntityQuery(typeof(LocalTransform), typeof(MoveSpeed)).ToEntityArray(Allocator.Temp);
        Debug.Log(entities.Length);
        foreach (var entity in entities)
        {
            var speed = state.EntityManager.GetComponentData<MoveSpeed>(entity);
            var target = state.EntityManager.GetComponentData<MoveTarget>(entity);
            var transform = state.EntityManager.GetComponentData<LocalTransform>(entity);
            
            Debug.Log($"{target.Value} - {transform.Position}");
            if (Vector2.Distance(new Vector2(transform.Position.x, transform.Position.y),
                    new Vector2(target.Value.x, target.Value.y)) < 0.1f)
            {
                target.Value.x = Random.Range(spawner.MinX, spawner.MaxX);
                target.Value.y = Random.Range(spawner.MinY, spawner.MaxY);
                state.EntityManager.SetComponentData(entity, target);
            }

            state.EntityManager.SetComponentData(entity,
                LocalTransform.FromPosition(transform.Position + (target.Value - transform.Position) * speed.Value));
        }

        entities.Dispose();
    }
}