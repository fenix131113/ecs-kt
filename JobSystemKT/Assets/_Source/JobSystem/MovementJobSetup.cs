using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;

namespace JobSystem
{
    public class MovementJobSetup : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int objectsCount;
        [SerializeField] private float radius;
        [SerializeField] private float speed;
        [SerializeField] private float logPeriodTime;

        private TransformAccessArray _transformAccessArray;
        private JobHandle _moveJobHandle;
        private JobHandle _logJobHandle;

        private void Awake()
        {
            Setup();
            InvokeRepeating(nameof(PrintLog), logPeriodTime, logPeriodTime);
        }

        private void Update()
        {
            InitJob();
        }

        private void OnDestroy() => _transformAccessArray.Dispose();

        private void PrintLog()
        {
            var array = new NativeArray<int>(_transformAccessArray.length, Allocator.Persistent);

            for (var index = 0; index < array.Length; index++) array[index] = Random.Range(0, 101);

            var logJob = new LogJob { RndArray = array};
        
            _logJobHandle = logJob.Schedule(array.Length, 64);
        }

        private void InitJob()
        {
            var job = new MoveJob
            {
                Radius = radius,
                Speed = speed,
                Time = Time.deltaTime
            };
        
            _moveJobHandle = job.Schedule(_transformAccessArray);
        }

        private void LateUpdate()
        {
            _moveJobHandle.Complete();
        
            if (_logJobHandle != default)
                _logJobHandle.Complete();
        }

        private void Setup()
        {
            _transformAccessArray = new TransformAccessArray(objectsCount);

            for (var i = 0; i < objectsCount; i++)
            {
                var spawned = Instantiate(prefab, Random.insideUnitSphere * radius, Quaternion.identity);
                _transformAccessArray.Add(spawned.transform);
            }
        }
    }
}