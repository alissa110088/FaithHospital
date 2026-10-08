using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawShape : MonoBehaviour
{
    [SerializeField] ControlState _stateControl;
    public bool _groupped;
    
    [HideInInspector] public State _state;
    
    public SpriteRenderer[] visualFeedBack;
    public List<Vector2> points;
    public int _nextLayer;
    
    private void OnEnable()
    {
        switch (_stateControl)
        {
            case ControlState.SwipeFollow:
                Debug.Log(PatientManager.Instance);
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

        // _state.enabled = false;

    }

    private void Start()
    {
        if (char.IsDigit(gameObject.tag[^1]))
        {
            int layer;
            layer = gameObject.tag[^1] - '0';
            PatientManager.Instance.AddLayer(layer - 1, _state);
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