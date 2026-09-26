using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class SwipeFollow : State
{
    [Header("GD")] [SerializeField] private float marginHowCloseeToPointToMoveOn = 0.3f;
    [SerializeField] private float marginHowFarCanGoFromLine = 1.5f;

    private bool _touching = false;

    private ControlState _state = ControlState.SwipeFollow;

    private Vector2 _lastPos = Vector2.zero;
    private Vector2 _pos;


    public Vector2 pos
    {
        get => _pos;
        set
        {
            _lastPos = _pos;
            _pos = value;
        }
    }

    private void OnEnable()
    {
        unlockState += OnStart;
    }

    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.onToucheStart -= TouchStarted;
            inputManager.onToucheEnd -= TouchEnded;
            inputManager.onTouchingPos -= OnSwipeStarted;
        }
    }

    private void OnStart(InputManager pInputManager, ControlState controlState)
    {
        if (controlState == _state)
        {
            inputManager = pInputManager;
            inputManager.onToucheStart += TouchStarted;
            inputManager.onToucheEnd += TouchEnded;
            inputManager.onTouchingPos += OnSwipeStarted;
            _points = GetComponent<DrawShape>().points;
            ;
        }
        else if (inputManager != null)
        {
            inputManager.onToucheStart -= TouchStarted;
            inputManager.onToucheEnd -= TouchEnded;
            inputManager.onTouchingPos -= OnSwipeStarted;
            _points = null;
        }
    }


    public void TouchStarted()
    {
        _currentPoint = _points[0];
        _touching = true;
    }

    private void TouchEnded()
    {
        _touching = false;
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

        float distance = -Camera.main.transform.position.z;

        //Converte to world point 
        Vector3 worldPos3 = Camera.main.ScreenToWorldPoint(
            new Vector3(pPos.x, pPos.y, distance)
        );

        Vector2 worldPos = new Vector2(worldPos3.x, worldPos3.y);

        if (!_canStart && CheckIfInRange(worldPos3, _points[0]))
        {
            _canStart = true;
        }
        else if (!_canStart)
            return;

        pos = worldPos;
        Vector2 dir = (_lastPos - pos).normalized;
        Vector2 dirPoints = (_points[_points.IndexOf(_currentPoint) + 1] - _currentPoint).normalized;


        //Turned positive to allow both side swipe
        dir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
        dirPoints = new Vector2(Mathf.Abs(dirPoints.x), Mathf.Abs(dirPoints.y));

        float dot = Vector2.Dot(dir, dirPoints);
        bool sameDirection = dot > 0.9f;
        float distancePointToSegment =
            DistancePointToSegment(_points[_points.IndexOf(_currentPoint) + 1], _currentPoint, pos);

        //Checks if went to first point
        if (sameDirection && (worldPos - _points[1]).magnitude < marginHowCloseeToPointToMoveOn &&
            distancePointToSegment < 10f)
        {
            if (_currentPoint == _points[^2])
            {
                Debug.Log("VALIDATED");
                Controller.OnInputValidate.Invoke();
                _patternValidated = true;
            }
            else
            {
                Debug.Log("VALIDATED GOING NEXT POINT");
                _currentPoint = _points[_points.IndexOf(_currentPoint) + 1];
            }
        }
        else if (sameDirection && distancePointToSegment < marginHowFarCanGoFromLine)
        {
            Debug.Log("CONTINUE");
        }
        //Checks if goes to the wrong direction 
        else if (_currentErrorMargin == _errorMargin)
        {
            Controller.OnInputNotValidate.Invoke();
            Debug.Log("NOT VALIDATED RESET");
            _currentPoint = _points[0];
            _currentErrorMargin = 0;
        }
        else
        {
            Controller.OnInputNotValidate.Invoke();
            Debug.Log("NOT VALIDATED " + sameDirection);
            _currentErrorMargin++;
        }
    }
}