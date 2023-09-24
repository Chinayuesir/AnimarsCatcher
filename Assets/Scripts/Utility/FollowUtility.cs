using UnityEngine;

public class FollowUtility
{
    /// <summary>
    /// ╬ьпнееап
    /// </summary>
    /// <param name="target"></param>
    /// <param name="index"></param>
    /// <param name="columns"></param>
    /// <param name="spacingX"></param>
    /// <param name="spacingY"></param>
    /// <returns></returns>
    public static Vector3 RectArrange(Transform target, int index, int columns = 5, float spacingX = 2f, float spacingY = 2f)
    {
        int row = index / 5;
        int col = index % 5;
        Vector3 center = target.position - (row + 1) * spacingY * target.forward;
        return center + (col + 1) / 2 * (col % 2 == 1 ? -1 : 1) * spacingX * target.right;
    }
}
