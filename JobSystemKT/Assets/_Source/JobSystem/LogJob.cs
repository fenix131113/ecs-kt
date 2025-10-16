using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace JobSystem
{
    public struct LogJob : IJobParallelFor
    {
        public NativeArray<int> RndArray;

        public void Execute(int index)
        {
            Debug.Log(Mathf.Log(RndArray[index]));
        }
    }
}