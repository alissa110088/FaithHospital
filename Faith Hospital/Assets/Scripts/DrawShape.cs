using System.Collections.Generic;
using UnityEngine;

public class DrawShape : MonoBehaviour
{
    public List<Vector2> points;

    private void OnDrawGizmos()
    {
        if (!(points.Count > 1))
            return;

        for (int i = 1; i < points.Count; i++)
        {
            Gizmos.DrawLine(points[i - 1], points[i]);

        }
    }
}