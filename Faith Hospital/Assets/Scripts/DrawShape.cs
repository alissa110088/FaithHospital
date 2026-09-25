using System.Collections.Generic;
using UnityEngine;

public class DrawShape : MonoBehaviour
{
    [SerializeField] ControlState _stateControl;
    public List<Vector2> points;

    private void OnEnable()
    {
        switch (_stateControl)
        {
            case ControlState.SwipeFollow:
                gameObject.AddComponent<SwipeFollow>();
                break;
            case ControlState.trace:
                gameObject.AddComponent<Trace>();
                break;
        }
    }
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