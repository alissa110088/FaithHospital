using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DrawShape : MonoBehaviour
{
    public ToolsEnum _toolUsed;
    [SerializeField] private ControlState _controlState;
    public InputSettings inputSettings;
    public bool _groupped;
    
    [HideInInspector] public State _state;
    
    public SpriteRenderer[] visualFeedBack;
    public List<Vector2> points;
    public int _nextLayer;
    
    private void OnEnable()
    {
        switch (_controlState)
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

    private void Start()
    {
        const string prefix = "Layer";
        string tag = gameObject.tag;

        if (tag.StartsWith(prefix) && int.TryParse(tag.Substring(prefix.Length), out int layer))
        {
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