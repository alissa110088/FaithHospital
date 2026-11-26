using System;
using System.Collections.Generic;
using UnityEngine;

public class DrawShape : MonoBehaviour
{
    public ToolsEnum _toolUsed;
    [SerializeField] private ControlState _controlState;
    public InputSettings inputSettings;
    
    [HideInInspector] public State _state;
    
    public SpriteRenderer[] visualFeedBack;
    public List<Vector3> points = new List<Vector3>();
    public bool isAffliction;
    [Header("ne remplir que si ce n'est pas une affliction")]
    public bool _groupped;
    public int _nextLayer;
    public bool isLast;
    
    
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
        _state._tool = _toolUsed;
        if(isAffliction)
            _nextLayer = -1;
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