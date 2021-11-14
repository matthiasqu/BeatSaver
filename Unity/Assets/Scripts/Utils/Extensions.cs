using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public static class Extensions
    {
        public static async Task DestroyDelayed(this GameObject go, int delay)
        {
            new Action(async () =>
            {
                await Task.Delay(new TimeSpan(0, 0, 0, 0, delay));
                Object.Destroy(go);
            }).Invoke();
        }

        public static IEnumerable<Vector3> OrderByDistance(this IEnumerable<Vector3> vectors, Vector3 reference)
        {
            return vectors.OrderBy(v => Vector3.Distance(reference, v));
        }

        public static IEnumerable<Vector3> OrderByDistance(this FixedQueue<Vector3> vectors, Vector3 reference)
        {
            return vectors.ToArray().OrderBy(v => Vector3.Distance(reference, v));
        }


        public static bool Approximates(this Vector3 v1, Vector3 v2)
        {
            if (Math.Abs(v1.x - v2.x) > float.Epsilon) return false;
            if (Math.Abs(v1.y - v2.y) > float.Epsilon) return false;
            if (Math.Abs(v1.z - v2.z) > float.Epsilon) return false;
            return true;
        }

        public static T FindComponentUpwards<T>(this Transform t)
        {
            var trans = t;
            T component = default;

            var i = 0;
            while (trans != null)
            {
                component = trans.gameObject.GetComponentInParent<T>(true);
                Debug.Log($"[FindComponentUpwards] {trans.name} on {i} has {typeof(T)}: {component != null}");
                if (component != null) return component;
                trans = trans.parent;
                i++;
            }

            return component;
        }
    }
}