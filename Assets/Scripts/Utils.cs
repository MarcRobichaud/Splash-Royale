using UnityEngine;

public static class Utils
{
    public static bool IsDistanceFromTargetInRange(this Vector3 position, Vector3 targetPosition, float range) =>
        Vector3.SqrMagnitude(position - targetPosition) <= range * range;

}