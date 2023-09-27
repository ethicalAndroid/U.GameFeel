using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static T RandomPop<T>(this List<T> list)
    {
        int index = Random.Range(0, list.Count);
        T get = list[index];
        list.RemoveAt(index);
        return get;
    }
    public static int OneLayer(this LayerMask mask)
    {
        return Mathf.RoundToInt(Mathf.Log(mask.value, 2));
    }
    public static LayerMask ToMask(this int layer)
    {
        return 1 << layer;
    }
    public static Vector2 UnitDirection(this float angle)
    {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
    public static float Angle(this Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x);
    }
    /// <summary>
    /// Rotates a vector using radians.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="delta"></param>
    /// <returns>The rotated vector.</returns>
    public static Vector2 Rotate(this Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
    public static void GetChildren(this Transform transform, List<Transform> emptyList)
    {
        int cCount = transform.childCount;
        for (int i = 0; i < cCount; i++)
        {
            emptyList.Add(transform.GetChild(i));
        }
    }
    /// <summary>
    /// Returns true when the bool is first true.
    /// </summary>
    /// <param name="edge"></param>
    /// <returns></returns>
    public static bool FallingEdge(this ref bool edge)
    {
        bool state = edge;
        edge = false;
        return state;
    }
    /// <summary>
    /// Returns true when the bool is first false.
    /// </summary>
    /// <param name="edge"></param>
    /// <returns></returns>
    public static bool RisingEdge(this ref bool edge)
    {
        bool state = !edge;
        edge = true;
        return state;
    }
}