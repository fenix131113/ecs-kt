using Unity.Burst;
using Unity.Entities;

namespace JobWithECS
{
    
    [BurstCompile]
    public partial struct MoveSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var time = (float)SystemAPI.Time.ElapsedTime;
            var job = new MoveJob { Time = time };
            job.ScheduleParallel();
        }
    }
}