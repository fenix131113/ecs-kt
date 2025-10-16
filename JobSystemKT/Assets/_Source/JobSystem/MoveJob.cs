using UnityEngine;
using UnityEngine.Jobs;

namespace JobSystem
{
    public struct MoveJob: IJobParallelForTransform
    {
        public float Time;
        public float Radius;
        public float Speed;
    
        public void Execute(int index, TransformAccess transform)
        {
            var angle = Speed * Time * index;
            var x = Radius * Mathf.Cos(angle);
            var z = Radius * Mathf.Sin(angle);
            transform.position = new Vector3(x, transform.position.y, z);
        }
    }
}