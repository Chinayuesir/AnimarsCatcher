using UnityEngine;

namespace AnimarsCatcher
{
    public static class TerrainUtility
    {
        public static Vector3 GetPositionOnTerrain(this Transform trans)
        {
            Physics.Raycast(trans.position + Vector3.up,
                -Vector3.up, out var hit, 10, LayerMask.GetMask("Terrain"));
            return hit.point;
        }
    }
}