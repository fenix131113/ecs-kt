using Leopotam.EcsLite;
using UnityEngine;

namespace LeoEcs
{
    public class CounterSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<CounterComponent>().End();
            var pool = world.GetPool<CounterComponent>();

            foreach (var entIndex in filter)
            {
                ref var c = ref pool.Get(entIndex);
                c.Value++;
                Debug.Log(c.Value);
            }
        }
    }
}