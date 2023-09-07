using UnityEngine;

public class FollowUtility
{
    public static float spacingY = 2f;
    public static float spacingX = 2f;

    public static Vector3 GetFollowerDestination(Transform target, int index)
    {
        int line = index / 5;
        int indexInLine = index % 5;
        Vector3 center = target.position - (line + 1) * spacingY * target.forward;
        return indexInLine switch
        {
            1 => center - target.right * spacingX,
            2 => center + target.right * spacingX,
            3 => center - spacingX * 2 * target.right,
            4 => center + spacingX * 2 * target.right,
            _ => center
        };
    }
}
