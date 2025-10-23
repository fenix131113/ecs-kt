using Leopotam.EcsLite;
using UnityEngine;

namespace LeoEcs.SecondPart
{
    public class BallsSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<MoveComponent>().End();
            var movePool = world.GetPool<MoveComponent>();
            var viewPool = world.GetPool<ViewReference>();

            foreach (var entIndex in filter)
            {
                ref var moveComponent = ref movePool.Get(entIndex);
                ref var viewComponent = ref viewPool.Get(entIndex);
                
                var sin = Mathf.Sin(Time.time * moveComponent.Frequency) * moveComponent.Amplitude;
                
                var pos = viewComponent.Transform.position;
                pos.x = sin;
                pos.y += moveComponent.Speed * Time.deltaTime;
                viewComponent.Transform.position = pos;
            }
        }
    }
}