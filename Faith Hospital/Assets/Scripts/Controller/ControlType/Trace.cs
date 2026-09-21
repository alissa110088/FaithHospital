using System;
using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour
{
    
    private float _range = 1f;
    private Vector2 _currentPoint;
    private List<Vector2> _points;
    
    private int _errorMargin = 25;
    private int _currentErrorMargin = 0;

    public InputManager inputManager;

    private bool _canStart;
    private bool _patternValidated;

    private void Start()
    {
        _points = GetComponent<DrawShape>().points;
        _currentPoint = _points[1];
        inputManager = Controller.Instance.inputManager;
        inputManager.onTouchingPos += OnTouchingScnreen;
        transform.position = new Vector3( _points[0].x, _points[0].y, -1);
    }

    private void OnDestroy()
    {
        inputManager.onTouchingPos -= OnTouchingScnreen;
    }

    private void OnTouchingScnreen(Vector2 pPosition)
    {
        if (!_canStart && CheckIfInRange(pPosition) &&
            new Vector2(transform.position.x, transform.position.y) == _points[0])
        {
            _canStart = true;
        }

        if (!_canStart)
            return;
        
        if (!CheckIfInRange(pPosition))
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
                transform.position = _points[0];
                _currentErrorMargin = 0;
            }
        }

        if (Vector3.Distance(transform.position, _currentPoint) < 0.1f)
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
        Vector2 direction = new Vector2(_currentPoint.x - transform.position.x, _currentPoint.y - transform.position.y).normalized;
        Vector3 dirV3 = new Vector3(direction.x, direction.y, 0f);
        transform.position += dirV3 * Time.deltaTime * 1.5f;
    }
    

    private bool CheckIfInRange(Vector3 pPosition)
    {
        if (Mathf.Abs((pPosition.x - transform.position.x)) < _range && Mathf.Abs((pPosition.y - transform.position.y)) < _range)
        {
            return true;
        }

        return false;
    }
}
