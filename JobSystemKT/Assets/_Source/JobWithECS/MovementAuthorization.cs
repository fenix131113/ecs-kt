using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace JobWithECS
{
    public class MovementAuthorization : MonoBehaviour
    {
        public float MovementSpeed;
        public float Radius;
    }

    public struct MoveSpeed : IComponentData
    {
        public float Value;
    }
    
    public struct Radius : IComponentData
    {
        public float Value;
    }

    public struct Center : IComponentData
    {
        public float3 Value;
    }

    public class MovementBaker : Baker<MovementAuthorization>
    {
        public override void Bake(MovementAuthorization authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic | TransformUsageFlags.Renderable);
            
            AddComponent(entity, new MoveSpeed {Value = authoring.MovementSpeed});
            AddComponent(entity, new Radius {Value = authoring.Radius});
            AddComponent(entity, new Center());
        }
    }
}