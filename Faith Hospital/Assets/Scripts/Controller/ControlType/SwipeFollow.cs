using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using Debug = UnityEngine.Debug;

public class SwipeFollow : State
{

    private bool _touching = false;

    private Vector3 _lastPos;
    private Vector3 _pos;

    private SwipeSettings _swipeSettings;

    public Vector3 pos { get => _pos; set { _lastPos = _pos; _pos = value; } }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        unlockState += OnStart;
        if (_drawShape == null)
            return;

        if (_drawShape.visualFeedBack.Length < 1)
            return;
        

        if (_drawShape._groupped)
            _countGrouped++;
    }

    private void OnDisable()
    {
        unlockState -= OnStart;
        _canStart = false;
        if (inputManager != null)
        {
            inputManager.onToucheStart -= TouchStarted;
            inputManager.onToucheEnd -= TouchEnded;
            inputManager.onTouchingPos -= OnSwipeStarted;

            inputManager = null;
        }

        DisableImage(false, this);
    }

    protected override void Start()
    {
        base.Start();
        Controller.OnInputValidate += DisableImage;
        
        _swipeSettings = _inputSettings as SwipeSettings;
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

            _patternValidated = false;

            inputManager = pInputManager;
            inputManager.onToucheStart += TouchStarted;
            inputManager.onToucheEnd += TouchEnded;
            inputManager.onTouchingPos += OnSwipeStarted;

            if (_drawShape.isActiveAndEnabled)
                _points = _drawShape.points;


            TouchStarted();
        }
        else if (inputManager != null)
        {
            _canStart = false;
            inputManager.onToucheStart -= TouchStarted;
            inputManager.onToucheEnd -= TouchEnded;
            inputManager.onTouchingPos -= OnSwipeStarted;
            inputManager = null;
            _points = null;
        }
    }


    public void TouchStarted()
    {
        if (!_drawShape.isActiveAndEnabled)
            return;

        _currentPoint = _points[0];
        _touching = true;
    }

    private void TouchEnded()
    {
        if (!_drawShape.isActiveAndEnabled)
            return;
        _touching = false;
        _canStart = false;
        _currentPoint = _points[0];
    }

    private static float DistancePointToSegment(Vector3 a, Vector3 b, Vector3 p)
    {
        Vector3 ab = b - a;
        Vector3 ap = p - a;

        float abLengthSq = ab.sqrMagnitude;

        if (abLengthSq < Mathf.Epsilon)
            return ap.magnitude;

        float t = Vector3.Dot(ap, ab) / abLengthSq;

        if (t < 0f)
        {
            return ap.magnitude;
        }

        if (t > 1f)
        {
            return (p - b).magnitude;
        }

        Vector3 closestPoint = a + t * ab;
        return (p - closestPoint).magnitude;
    }

    private void OnSwipeStarted(Vector2 pPos)
    {
        if (!_touching || float.IsInfinity(pPos.x) || float.IsInfinity(pPos.y) || _patternValidated)
            return;

        Ray ray = Camera.main.ScreenPointToRay(pPos);
        Plane plane = new Plane(Vector3.up, _drawShape.transform.position);
        if (!plane.Raycast(ray, out float enter))
            return;

        Vector3 worldPos3 = ray.GetPoint(enter);

        if (!_canStart && CheckIfInRange(worldPos3, _points[0]))
        {
            _canStart = true;
        }
        else if (!_canStart)
            return;

        pos = worldPos3;
        Vector3 dir = Flat(_lastPos - pos).normalized;
        Vector3 dirPoints = Flat(_points[_points.IndexOf(_currentPoint) + 1] - _currentPoint).normalized;

        dir = new Vector3(Mathf.Abs(dir.x), 0f, Mathf.Abs(dir.z));
        dirPoints = new Vector3(Mathf.Abs(dirPoints.x), 0f, Mathf.Abs(dirPoints.z));
        
        float dot = Vector3.Dot(dir, dirPoints);
        bool sameDirection = dot > _swipeSettings.DirectionRange;
        float distancePointToSegment =
            DistancePointToSegment(Flat(_points[_points.IndexOf(_currentPoint) + 1]), Flat(_currentPoint), Flat(pos));

        //Checks if went to first point
        if (sameDirection && (Flat(worldPos3) - Flat(_points[_points.IndexOf(_currentPoint) + 1])).magnitude < 0.3f &&
            distancePointToSegment < _swipeSettings.howFarFromSegmentMax)
        {
            if (_currentPoint == _points[^2])
            {
                
                if (CheckOperationEnded())
                {
                    _patternValidated = true;
                    return;
                }
                if (_drawShape._groupped)
                {
                    _countGrouped--;
                    if (_countGrouped > 0)
                    {
                        Controller.OnInputValidate.Invoke(true, this);
                        _patternValidated = true;
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
        else if (sameDirection && distancePointToSegment < _swipeSettings.howFarFromSegmentMax)
        {
            return;
        }
        //Checks if goes to the wrong direction 
        else if (_currentErrorMargin == _errorMargin)
        {
            Controller.OnInputNotValidate.Invoke();
            _currentPoint = _points[0];
            _currentErrorMargin = 0;
        }
        else
        {
            Controller.OnInputNotValidate.Invoke();
            _currentErrorMargin++;
        }
    }

    private void DisableImage(bool pBool, State state)
    {
        if (state != this)
            return;
    
        foreach (SpriteRenderer i in _drawShape.visualFeedBack)
        {
            if (i != null)
                i.enabled = false;
        }
    }
}