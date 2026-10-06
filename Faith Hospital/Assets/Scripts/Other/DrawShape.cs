using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawShape : MonoBehaviour
{
    [SerializeField] ControlState _stateControl;
    [SerializeField] private bool _groupped;
    
    [HideInInspector] public State _state;
    
    public SpriteRenderer[] visualFeedBack;
    public List<Vector2> points;
    public int _nextLayer;
    
    private void OnEnable()
    {
        switch (_stateControl)
        {
            case ControlState.SwipeFollow:
                _state = gameObject.AddComponent<SwipeFollow>();
                _state._drawShape = this;
                break;
            case ControlState.trace:
                _state = gameObject.AddComponent<Trace>();
                _state._drawShape = this;
                break;
            case ControlState.touch:
                _state = gameObject.AddComponent<TouchInput>();
                _state._drawShape = this;
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