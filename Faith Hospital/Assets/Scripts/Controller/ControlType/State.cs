using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class State : MonoBehaviour
{
    public static Action<InputManager, ToolsEnum> unlockState;

    protected ToolsEnum _tool = ToolsEnum.none;
    
    protected static int _countGrouped = 0;

    public InputManager inputManager;
    public DrawShape _drawShape;

    protected bool _patternValidated;
    protected bool _canStart;
    private bool _registered;

    protected int _errorMargin = 25;
    protected int _currentErrorMargin = 0;

    protected Vector2 _currentPoint;

    protected List<Vector2> _points;

    protected float _range = 1f;
    
    protected InputSettings _inputSettings;

    protected virtual void Start()
    {
        if (_drawShape != null)
        {
            _tool = _drawShape._toolUsed;
            _inputSettings = _drawShape.inputSettings;
            _range = _inputSettings.MaxDistanceFromPoint;
        }
    }

    //TODO jsp
    protected virtual void Update()
    {
    }

    protected bool CheckIfInRange(Vector3 pPosition, Vector3 pPoint)
    {
        if (Mathf.Abs((pPosition.x - pPoint.x)) < _range && Mathf.Abs((pPosition.y - pPoint.y)) < _range)
        {
            return true;
        }

        return false;
    }

    //A faire un truc
    protected virtual void OnStart(InputManager pInputManager, ToolsEnum pTool)
    {
       
    }
}