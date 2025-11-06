using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project
{
    public class TemperatureZone : MonoBehaviour
    {
        [SerializeField] private float affectDistance;
        [SerializeField] private float rate;
        [SerializeField] private bool leftMouse;
        
        private EntityManager _em;

        private void Start() => _em = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        private void Update()
        {
            if ((leftMouse && Mouse.current.leftButton.isPressed) ||
                (!leftMouse && Mouse.current.rightButton.isPressed))
            {
                var mousePos = Mouse.current.position.ReadValue();
                var pos = Camera.main!.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
                pos.z = 0;
                transform.position = pos;
            }
            
            var query = _em.CreateEntityQuery(typeof(Temperature), typeof(LocalTransform));
            using var entities = query.ToEntityArray(Unity.Collections.Allocator.Temp);
            using var transforms = query.ToComponentDataArray<LocalTransform>(Unity.Collections.Allocator.Temp);

            for (var i = 0; i < entities.Length; i++)
            {
                var t = transforms[i];
                var dist = Vector3.Distance(t.Position, transform.position);
                
                if (dist >= affectDistance)
                    continue;
                
                var temp = _em.GetComponentData<Temperature>(entities[i]);
                temp.Value += rate * Time.deltaTime;
                _em.SetComponentData(entities[i], temp);
            }
        }
    }
}