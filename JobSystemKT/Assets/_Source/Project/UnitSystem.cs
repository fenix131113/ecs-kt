using System.Linq;
using Project;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using static Project.ProjectSpawnAuthorization;
using Random = UnityEngine.Random;

[BurstCompile]
public partial struct UnitSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var spawner = SystemAPI.GetSingleton<Spawner>();

        var entities = state.EntityManager.CreateEntityQuery(typeof(LocalTransform), typeof(UnitData))
            .ToEntityArray(Allocator.Temp);
        
        foreach (var entity in entities)
        {
            var speed = state.EntityManager.GetComponentData<UnitData>(entity);
            var target = state.EntityManager.GetComponentData<MoveTarget>(entity);
            var transform = state.EntityManager.GetComponentData<LocalTransform>(entity);
            var temp = state.EntityManager.GetComponentData<Temperature>(entity);

            if (Vector2.Distance(new Vector2(transform.Position.x, transform.Position.y),
                    new Vector2(target.Value.x, target.Value.y)) < 0.1f)
            {
                target.Value.x = Random.Range(spawner.MinX, spawner.MaxX);
                target.Value.y = Random.Range(spawner.MinY, spawner.MaxY);
                state.EntityManager.SetComponentData(entity, target);
            }

            state.EntityManager.SetComponentData(entity,
                LocalTransform.FromPosition(transform.Position +
                                            math.normalize(target.Value - transform.Position) * speed.Speed));

            // Temperature

            var pos = transform.Position;
            var warmSum = 0f;
            var coldSum = 0f;
            var warmRateSum = 0f;
            var coldRateSum = 0f;

            foreach (var otherEntity in entities.Where(otherEntity => entity != otherEntity))
            {
                var otherTransform = state.EntityManager.GetComponentData<LocalTransform>(otherEntity);
                var otherTemp = state.EntityManager.GetComponentData<Temperature>(otherEntity);
                var otherData = state.EntityManager.GetComponentData<UnitData>(otherEntity);

                if (math.distance(pos, otherTransform.Position) >= otherData.AffectDistance)
                    continue;

                if (otherTemp.Value >= 0.5f)
                {
                    warmRateSum += otherData.HeatRate;
                    warmSum++;
                }
                else
                {
                    coldRateSum += otherData.HeatRate;
                    coldSum++;
                }
            }
            
            temp.Value += (warmSum >= coldSum ? warmRateSum : -coldRateSum) * SystemAPI.Time.DeltaTime;
            temp.Value = math.clamp(temp.Value, 0f, 1f);
            
            var color = state.EntityManager.GetComponentObject<SpriteRenderer>(entity);
            var lerp = math.lerp(new float4(0.012f, 0.75f, 0.91f, 1f), new float4(0.91f, 0.18f, 0.012f, 1f), temp.Value);
            color.color = new Color(lerp.x, lerp.y, lerp.z, lerp.w);
            
            state.EntityManager.SetComponentData(entity, temp);
        }

        entities.Dispose();
    }
}