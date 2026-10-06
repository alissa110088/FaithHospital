using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class State : MonoBehaviour
{
    public static Action<InputManager, ControlState> unlockState;

    protected static int _countGrouped = 0;

    public InputManager inputManager;
    public DrawShape _drawShape;
    
    protected bool _patternValidated;
    protected bool _canStart;

    protected int _errorMargin = 25;
    protected int _currentErrorMargin = 0;

    protected Vector2 _currentPoint;

    protected List<Vector2> _points;

    protected float _range = 1f; 
    
    
    protected bool CheckIfInRange(Vector3 pPosition, Vector3 pPoint)
    {
        if (Mathf.Abs((pPosition.x - pPoint.x)) < _range && Mathf.Abs((pPosition.y - pPoint.y)) < _range)
        {
            return true;
        }

        return false;
    }
}