using System;
using System.Collections.Generic;
using UnityEngine;

public class Trace : State
{
    [SerializeField] private float speedTrace = 0.75f;
    
    private Vector2 _currentPos = Vector2.zero;
    
    private ControlState _state = ControlState.trace;
    
    private bool _start;

    private void OnEnable()
    {
        unlockState += OnStart;
        _points = GetComponent<DrawShape>().points;
        transform.position = new Vector3(_points[0].x, _points[0].y, -1); 
    }
    
    private void OnStart(InputManager pInputManager, ControlState controlState)
    {
        _currentPoint = _points[1];

        if (controlState == _state)
        {
            if (inputManager != null)
            {
                return;
            }
            inputManager = pInputManager;

            inputManager.onTouchingPos += OnTouchingScreen;
            inputManager.onToucheStart += OnTouchedScreen;

            transform.position = new Vector3(_points[0].x, _points[0].y, -1);
        }
        else if(inputManager!=null)
        {
            inputManager.onTouchingPos -= OnTouchingScreen;
            inputManager.onToucheStart -= OnTouchedScreen;   
            
            _points = null;
            _currentPoint = Vector2.zero;
            inputManager = null;
        }
    }
    
    
    private void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.onTouchingPos -= OnTouchingScnreen;
            inputManager.onToucheStart -= OnTouchedScreen;
        }
    }

    private void Update()
    {
        if(_start)
            OnTouchingScnreen(_currentPos);
    }

    private void OnTouchedScreen()=> _start = true;   
    

    private void OnTouchingScreen(Vector2 pCurrentPos)
    {
        _currentPos = pCurrentPos;
    }
    
    private void OnTouchingScnreen(Vector2 pPosition)
    {
        
        if (_patternValidated || float.IsInfinity(pPosition.x) || float.IsInfinity(pPosition.y))
            return;
        
        float distance = -Camera.main.transform.position.z;
    
        //Converte to world point 
        Vector3 worldPos3 = Camera.main.ScreenToWorldPoint(
            new Vector3(pPosition.x, pPosition.y, distance)
        );
    
        Vector2 worldPos = new Vector2(worldPos3.x, worldPos3.y);
        
        if (!_canStart && CheckIfInRange(worldPos, transform.position) &&
            new Vector2(transform.position.x, transform.position.y) == _points[0])
        {
            _canStart = true;
        }
    
        if (!_canStart)
            return;
        
        
        if (!CheckIfInRange(worldPos, transform.position))
        {
            if (_currentErrorMargin != _errorMargin)
            {
                _currentErrorMargin++;
                Controller.OnInputNotValidate.Invoke();
                Debug.Log("NOT VALIDATED ");
            }
            else
            {
                Controller.OnInputNotValidate.Invoke();
                Debug.Log("NOT VALIDATED RESET");
                _currentPoint = _points[1];
                transform.position = new Vector3( _points[0].x, _points[0].y, -1);
                _currentErrorMargin = 0;
            }
        }
        if (Vector3.Distance(transform.position, _currentPoint) < 1.1f)
        {
            if (_currentPoint == _points[^1])
            {
                Controller.OnInputValidate.Invoke();
                _patternValidated = true;
                Debug.Log("finishhhh");
            }
            else
            {
                _currentPoint = _points[_points.IndexOf(_currentPoint) + 1];
                Debug.Log("NEXT POINT");
            }
        }
        Debug.Log("MOVING");
        Vector2 direction = new Vector2(_currentPoint.x - transform.position.x, _currentPoint.y - transform.position.y).normalized;
        Vector3 dirV3 = new Vector3(direction.x, direction.y, 0f);
        transform.position += dirV3 * Time.deltaTime * speedTrace;
    }
}

