using UnityEngine;

public static class RandomValue
{
    public static Vector2 RandomPosAround(Vector2 center, float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        return center + new Vector2(x, y);
    }
}
