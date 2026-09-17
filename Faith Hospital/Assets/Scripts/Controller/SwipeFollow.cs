using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class SwipeFollow : MonoBehaviour
{
    [SerializeField] private GameObject _follow;
    [SerializeField] private DrawShape _drawShape;

    [Header("GD")] 
    [SerializeField] private float marginHowCloseeToPointToMoveOn = 0.1f;

    [SerializeField] private float marginHowFarCanGoFromLine = 0.3f;
    private bool _touching = false;

    private Vector2 _lastPos = Vector2.zero;
    private Vector2 _pos;
    private Vector2 _currentPoint;

    private List<Vector2> _points;

    private int _errorMargin = 25;
    private int _currentErrorMargin = 0;

    private InputManager inputManager;

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
        inputManager = new InputManager();
        inputManager.onToucheStart += TouchStarted;
        inputManager.onToucheEnd += TouchEnded;
        inputManager.onTouchingPos += OnSwipeStarted;

        _points = _drawShape.points;
    }

    private void OnDisable()
    {
        inputManager.onToucheStart -= TouchStarted;
        inputManager.onToucheEnd -= TouchEnded;
        inputManager.onTouchingPos -= OnSwipeStarted;
    }


    private void TouchStarted()
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
        if (!_touching || float.IsInfinity(pPos.x) || float.IsInfinity(pPos.y))
            return;

        float distance = -Camera.main.transform.position.z;

        //Converte to world point 
        Vector3 worldPos3 = Camera.main.ScreenToWorldPoint(
            new Vector3(pPos.x, pPos.y, distance)
        );

        Vector2 worldPos = new Vector2(worldPos3.x, worldPos3.y);


        _follow.transform.position = worldPos;

        pos = worldPos;
        Vector2 dir = (_lastPos - pos).normalized;
        Vector2 dirPoints = (_points[_points.IndexOf(_currentPoint) + 1] - _currentPoint).normalized;


        //Turned positive to allow both side swipe
        dir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
        dirPoints = new Vector2(Mathf.Abs(dirPoints.x), Mathf.Abs(dirPoints.y));


        float dot = Vector2.Dot(dir, dirPoints);
        bool sameDirection = dot > 0.9f;
        float distancePointToSegment = DistancePointToSegment(_points[_points.IndexOf(_currentPoint) + 1], _currentPoint, pos);
        
        //Checks if went to first point
        if (sameDirection && (worldPos - _points[1]).magnitude < marginHowCloseeToPointToMoveOn && distancePointToSegment < 0.3f)
        {
            if (_currentPoint == _points[^2])
            {
                Debug.Log("VALIDATED");
                Controller.OnInputValidate.Invoke();
                gameObject.SetActive(false);
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
            Debug.Log("NOT VALIDATED RESET");
            _currentPoint = _points[0];
            _touching = false;
        }
        else
        {
            Debug.Log("NOT VALIDATED " + sameDirection);
            _currentErrorMargin++;
        }
    }
}