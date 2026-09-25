using System;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public static Action<InputManager, ControlState> unlockState;
    public InputManager inputManager;
    
    protected bool _patternValidated;
    protected bool _canStart;
    
    protected int _errorMargin = 25;
    protected int _currentErrorMargin = 0;
    
    protected Vector2 _currentPoint;

    protected List<Vector2> _points;
    
    protected float _range = 0.5f;
    
    protected bool CheckIfInRange(Vector3 pPosition, Vector3 pPoint)
    {
        if (Mathf.Abs((pPosition.x - pPoint.x)) < _range && Mathf.Abs((pPosition.y - pPoint.y)) < _range)
        {
            return true;
        }

        return false;
    }
}
