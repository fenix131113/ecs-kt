using LeoEcs.SecondPart;
using Leopotam.EcsLite;
using UnityEngine;

namespace LeoEcs
{
    public class ECSStarter : MonoBehaviour
    {
        [SerializeField] private int ballsCount;
        [SerializeField] private float minAmplitude;
        [SerializeField] private float maxAmplitude;
        [SerializeField] private float minFrequency;
        [SerializeField] private float maxFrequency;
        [SerializeField] private float speed;
        [SerializeField] private float startY;
        [SerializeField] private float spawnOffset;
        [SerializeField] private GameObject ballPrefab;
        
        private EcsWorld _world;
        private EcsSystems _systems;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _systems.Add(new CounterSystem());
            _systems.Add(new BallsSystem());
            _systems.Init();

            CreateCounterEntity();

            var pos = new Vector3(0, startY, 0);
            
            for (var i = 0; i < ballsCount; i++)
            {
                CreateBallEntity(pos);
                pos += new Vector3(0, spawnOffset, 0);
            }
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _world?.Destroy();
        }

        private int CreateCounterEntity()
        {
            var counterEntity = _world.NewEntity();
            var pool = _world.GetPool<CounterComponent>();
            ref var c = ref pool.Add(counterEntity);

            return counterEntity;
        }

        private int CreateBallEntity(Vector3 pos)
        {
            var spawned = Instantiate(ballPrefab, pos, Quaternion.identity);
            
            var ballEntity = _world.NewEntity();
            var movePool = _world.GetPool<MoveComponent>();
            var viewPool = _world.GetPool<ViewReference>();
            ref var moveComponent = ref movePool.Add(ballEntity);
            ref var viewComponent = ref viewPool.Add(ballEntity);
            
            viewComponent.Transform = spawned.transform;
            moveComponent.Amplitude = Random.Range(minAmplitude, maxAmplitude);
            moveComponent.Frequency = Random.Range(minFrequency, maxFrequency);
            moveComponent.Speed = speed;

            return ballEntity;
        }
    }
}