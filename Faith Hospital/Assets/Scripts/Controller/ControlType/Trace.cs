using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trace : State
{
    private Vector2 _currentPos = Vector2.zero;

    private bool _start;

    private TraceSettings _traceSettings;
    private void OnEnable()
    {
        unlockState += OnStart;

        if (_drawShape == null)
            return;

        foreach (SpriteRenderer i in _drawShape.visualFeedBack)
        {
            i.enabled = true;
        }
        
        if (_drawShape._groupped)
            _countGrouped++;
    }

    private void OnDisable()
    {
        DisableImage(this);
        unlockState -= OnStart;
        _canStart = false;
        if (inputManager != null)
        {
            inputManager.onTouchingPos -= OnTouchingScreen;
            inputManager.onToucheStart -= OnTouchedScreen;

            _currentPoint = Vector2.zero;
            inputManager = null;
        }
    }

    protected override void Start()
    {
        base.Start();
        if (!_drawShape.isActiveAndEnabled)
            return;
        _points = _drawShape.points;
        transform.position = new Vector3(_points[0].x, _points[0].y, -1);
        _traceSettings = _inputSettings as TraceSettings;
    }


    protected override void OnStart(InputManager pInputManager, ToolsEnum pTool)
    {
        base.OnStart(pInputManager, pTool);
        if (pTool == _tool)
        {
            if (inputManager != null)
            {
                return;
            }

            inputManager = pInputManager;
            inputManager.onTouchingPos += OnTouchingScreen;
            inputManager.onToucheStart += OnTouchedScreen;
            inputManager.onToucheEnd += OnEndTouch;
                
            if (!_drawShape.isActiveAndEnabled)
                return;

            if (_points.Count > 1)
                _currentPoint = _points[1];
            transform.position = new Vector3(_points[0].x, _points[0].y, -1);
        }
        else if (inputManager != null)
        {
            _canStart = false;
            inputManager.onTouchingPos -= OnTouchingScreen;
            inputManager.onToucheStart -= OnTouchedScreen;
            inputManager.onToucheEnd -= OnEndTouch;

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
            inputManager = null;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (_start)
            OnTouchingScnreen(_currentPos);
    }

    private void OnTouchedScreen() => _start = true;

    private void OnTouchingScreen(Vector2 pCurrentPos)
    {
        _currentPos = pCurrentPos;
    }

    private void OnEndTouch()
    {
        _currentPoint = _points[1];
        transform.position = new Vector3(_points[0].x, _points[0].y, -1);
        _currentErrorMargin = 0;
        _currentPos = Vector3.positiveInfinity;
    }

    private void OnTouchingScnreen(Vector2 pPosition)
    {
        if (_patternValidated || float.IsInfinity(pPosition.x) || float.IsInfinity(pPosition.y) ||
            !_drawShape.isActiveAndEnabled)
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
                transform.position = new Vector3(_points[0].x, _points[0].y, -1);
                _currentErrorMargin = 0;
            }
        }

        if (Vector3.Distance(transform.position, _currentPoint) < 1.1f)
        {
            if (_currentPoint == _points[^1])
            {
                if (_drawShape._groupped)
                {
                    _countGrouped--;
                    if (_countGrouped > 0)
                    {
                        Controller.OnInputValidate.Invoke(true,this);
                        _patternValidated = true;
                        Debug.Log("finishhhh");
                        return;
                    }
                }
                
                Controller.OnInputValidate.Invoke(false, this);
                _patternValidated = true;
            }
            else
            {
                _currentPoint = _points[_points.IndexOf(_currentPoint) + 1];
            }
        }

        Vector2 direction = new Vector2(_currentPoint.x - transform.position.x, _currentPoint.y - transform.position.y)
            .normalized;
        Vector3 dirV3 = new Vector3(direction.x, direction.y, 0f);
        transform.position += dirV3 * Time.deltaTime * _traceSettings.speedMoving;
    }

    private void DisableImage(State state)
    {
        if (state != this)
            return;

        foreach (SpriteRenderer i in _drawShape.visualFeedBack)
        {
            i.enabled = false;
        }
    }
}