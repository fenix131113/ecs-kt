using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace JobWithECS
{
    public class EntityGizmosDrawer : MonoBehaviour
    {
        [SerializeField] private int maxDraw;
        [SerializeField] private Color color;

        private void OnDrawGizmos()
        {
            if(!Application.isPlaying)
                return;
            
            var world = World.DefaultGameObjectInjectionWorld;
            if (world == null)
                return;
            
            var manager = world.EntityManager;
            var query = manager.CreateEntityQuery(typeof(LocalTransform));
            var count = query.CalculateEntityCount();
            if(count == 0)
                return;
            
            Gizmos.color = color;
            using var t = query.ToComponentDataArray<LocalTransform>(Allocator.Temp);
            var sample = Mathf.Min(maxDraw, t.Length);
            var step = Mathf.Max(1, t.Length / sample);
            var drawn = 0;

            for (var i = 0; i < t.Length && drawn < sample; i += step)
            {
                var pos = t[i].Position;
                Gizmos.DrawWireCube(pos, Vector3.one);
                drawn++;
            }
        }
    }
}