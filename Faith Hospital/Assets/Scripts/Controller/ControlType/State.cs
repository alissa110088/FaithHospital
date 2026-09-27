using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class State : MonoBehaviour
{
    [SerializeField] private Image[] visualFeedBack;

    public static Action<InputManager, ControlState> unlockState;

    public InputManager inputManager;

    protected bool _patternValidated;
    protected bool _canStart;

    protected int _errorMargin = 25;
    protected int _currentErrorMargin = 0;
    public int _nextLayer;

    protected Vector2 _currentPoint;

    protected List<Vector2> _points;

    protected float _range = 0.5f;

    
    //TODO AVOIR UNE INSTANCE DE DRAWSHAPE QUE TU DESACTIVFE BITCH
    protected virtual void Start()
    {
        foreach (Image i in visualFeedBack)
        {
            i.enabled = true;
        }
    }

    private void OnDisable()
    {
        foreach (Image i in visualFeedBack)
        {
            i.enabled = false;
        }
    }
    
    protected bool CheckIfInRange(Vector3 pPosition, Vector3 pPoint)
    {
        if (Mathf.Abs((pPosition.x - pPoint.x)) < _range && Mathf.Abs((pPosition.y - pPoint.y)) < _range)
        {
            return true;
        }

        return false;
    }
}