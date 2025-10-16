using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace JobWithECS
{
    [BurstCompile]
    public partial struct MoveJob : IJobEntity
    {
        public float Time;
        
        public void Execute(ref LocalTransform transform, in MoveSpeed speed, in Radius radius, in Center center)
        {
            var time = Time * speed.Value;

            var cos = math.cos(time);
            var sin = math.sin(time);

            var offset = new float3(cos * radius.Value, 0f, sin * radius.Value);
            
            var newPos = center.Value + offset;
            newPos.y = 0f;

            transform = transform.WithPosition(newPos);
        }
    }
}