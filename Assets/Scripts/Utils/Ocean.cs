using UnityEngine;

public static class Ocean
{
    public static float depth = 599f;

    public static float GetSamplePointDepth(Vector3 position)
    {
        float _depth = depth * 0.5f - position.y;
        return Mathf.Min(_depth, depth);
    }
}
