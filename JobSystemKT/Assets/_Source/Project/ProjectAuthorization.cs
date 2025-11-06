using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project
{
    public class ProjectAuthorization : MonoBehaviour
    {
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float AffectDistance { get; private set; }
        [field: SerializeField] public float HeatRate { get; private set; }
    }

    public struct UnitData : IComponentData
    {
        public float Speed;
        public float AffectDistance;
        public float HeatRate;
    }

    public struct MoveTarget : IComponentData
    {
        public float3 Value;
    }

    public struct Temperature : IComponentData
    {
        public float Value;
    }

    public class MovementBaker : Baker<ProjectAuthorization>
    {
        public override void Bake(ProjectAuthorization authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic | TransformUsageFlags.Renderable);

            AddComponent(entity,
                new UnitData { Speed = authoring.MoveSpeed, AffectDistance = authoring.AffectDistance, HeatRate = authoring.HeatRate });
            AddComponent(entity, new MoveTarget());
            AddComponent(entity, new Temperature { Value = 0 });
        }
    }
}