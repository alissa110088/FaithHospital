using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trace : State
{
    private const float ReachThreshold = 0.1f;

    private Vector2 _currentPos = Vector2.zero; 
    private bool _start;
    private int _currentIndex = 1;

    private TraceSettings _traceSettings;
    private Camera _cam;

    private Camera Cam
    {
        get
        {
            if (_cam == null)
                _cam = Camera.main;
            return _cam;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        unlockState += OnStart;

        if (_drawShape == null)
            return;

        if (_drawShape._groupped)
            _countGrouped++;
    }

    private void OnDisable()
    {
        DisableImage(this);
        unlockState -= OnStart;
        _canStart = false;
        _start = false;
        Unsubscribe();
    }

    protected override void Start()
    {
        base.Start();
        _cam = Camera.main;

        if (!_drawShape.isActiveAndEnabled)
            return;

        _points = _drawShape.points; 
        SetPosition(_points[0]);
        _traceSettings = _inputSettings as TraceSettings;
    }

    protected override void OnStart(InputManager pInputManager, ToolsEnum pTool)
    {
        base.OnStart(pInputManager, pTool);

        if (pTool == _tool)
        {
            if (inputManager != null)
                return;

            _patternValidated = false;

            inputManager = pInputManager;
            inputManager.onTouchingPos += OnTouchingScreen;
            inputManager.onToucheStart += OnTouchedScreen;
            inputManager.onToucheEnd += OnEndTouch;

            if (!_drawShape.isActiveAndEnabled)
                return;

            ResetToStart();
        }
        else if (inputManager != null)
        {
            _canStart = false;
            _start = false;
            Unsubscribe();
        }
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Unsubscribe()
    {
        if (inputManager == null)
            return;

        inputManager.onTouchingPos -= OnTouchingScreen;
        inputManager.onToucheStart -= OnTouchedScreen;
        inputManager.onToucheEnd -= OnEndTouch;

        _currentPoint = Vector3.zero;
        inputManager = null;
    }

    protected override void Update()
    {
        base.Update();
        if (_start)
            FollowPattern(_currentPos);
    }

    private void OnTouchedScreen() => _start = true;

    private void OnTouchingScreen(Vector2 pCurrentPos) => _currentPos = pCurrentPos;

    private void OnEndTouch()
    {
        _start = false;
        _canStart = false;
        _currentPos = Vector2.positiveInfinity;
        _currentErrorMargin = 0;
        ResetToStart();
    }

    private bool ScreenToCursorPlane(Vector2 screenPos, out Vector3 result)
    {
        Ray ray = Cam.ScreenPointToRay(screenPos);
        Plane plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float enter))
        {
            result = ray.GetPoint(enter);
            return true;
        }

        result = default;
        return false;
    }

    private static float FlatDistance(Vector3 a, Vector3 b)
    {
        a.y = 0f;
        b.y = 0f;
        return Vector3.Distance(a, b);
    }

    private void FollowPattern(Vector2 pPosition)
    {
        if (_patternValidated || float.IsInfinity(pPosition.x) || float.IsInfinity(pPosition.y) ||
            !_drawShape.isActiveAndEnabled)
            return;

        if (!ScreenToCursorPlane(pPosition, out Vector3 worldPos))
            return;

        Vector3 pos = transform.position;

        if (!_canStart && CheckIfInRange(worldPos, pos) && FlatDistance(pos, _points[0]) < ReachThreshold)
            _canStart = true;

        if (!_canStart)
            return;

        if (!CheckIfInRange(worldPos, pos))
        {
            Controller.OnInputNotValidate.Invoke();

            if (_currentErrorMargin < _errorMargin)
            {
                _currentErrorMargin++;
            }
            else
            {
                _currentErrorMargin = 0;
                ResetToStart();
                return;
            }
        }

        if (FlatDistance(pos, _points[_currentIndex]) < ReachThreshold)
        {
            if (_currentIndex >= _points.Count - 1)
            {
                EndPattern();
                return;
            }

            _currentIndex++;
            _currentPoint = _points[_currentIndex];
        }

        Vector3 target = new Vector3(_points[_currentIndex].x, transform.position.y, _points[_currentIndex].z);
        transform.position = Vector3.MoveTowards(
            transform.position, target, _traceSettings.speedMoving * Time.deltaTime);
    }

    private void EndPattern()
    {
        _patternValidated = true;

        if (CheckOperationEnded())
            return;

        if (_drawShape._groupped)
        {
            _countGrouped--;
            if (_countGrouped > 0)
            {
                Controller.OnInputValidate.Invoke(true, this);
                return;
            }
        }

        Controller.OnInputValidate.Invoke(false, this);
    }

    private void ResetToStart()
    {
        _currentIndex = _points.Count > 1 ? 1 : 0;
        _currentPoint = _points[_currentIndex];
        SetPosition(_points[0]);
    }

    private void SetPosition(Vector3 p)
    {
        transform.position = new Vector3(p.x, transform.position.y, p.z);
    }

    private void DisableImage(State state)
    {
        if (state != this || _drawShape == null)
            return;

        foreach (SpriteRenderer i in _drawShape.visualFeedBack)
            i.enabled = false;
    }
}