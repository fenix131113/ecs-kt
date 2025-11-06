using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project
{
    public class ProjectAuthorization : MonoBehaviour
    {
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float MinX { get; private set; }
        [field: SerializeField] public float MaxX { get; private set; }
        [field: SerializeField] public float MinY { get; private set; }
        [field: SerializeField] public float MaxY { get; private set; }
    }

    public struct MoveSpeed : IComponentData
    {
        public float Value;
    }

    public struct MoveTarget : IComponentData
    {
        public float3 Value;
    }

    public class MovementBaker : Baker<ProjectAuthorization>
    {
        public override void Bake(ProjectAuthorization authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic | TransformUsageFlags.Renderable);

            AddComponent(entity, new MoveSpeed { Value = authoring.MoveSpeed });
            AddComponent(entity, new MoveTarget());
        }
    }
}