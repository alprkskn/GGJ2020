using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ggj20
{
    public static class PhysicsUtils
    {
        private static ContactFilter2D _flammableContactFilter;

        private static Collider2D[] _overlapBuffer = new Collider2D[100];

        static PhysicsUtils()
        {
            _flammableContactFilter = new ContactFilter2D();
            _flammableContactFilter.useLayerMask = true;
            _flammableContactFilter.layerMask = LayerMask.GetMask("Flammable");
        }

        public static List<Flammable> GetOverlappingFlammables(Collider2D collider)
        {
            var count = collider.OverlapCollider(_flammableContactFilter, _overlapBuffer);

            var result = new List<Flammable>();

            for (int i = 0; i < count; i++)
            {
                var o = _overlapBuffer[i];

                var flammable = o.GetComponentInParent<Flammable>();

                if(flammable)
                {
                    result.Add(flammable);
                }
            }

            return result;
        }
    }
}