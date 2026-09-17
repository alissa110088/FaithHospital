using UnityEngine;
using System.Collections.Generic;
    
public class SwipeFollow : MonoBehaviour
{ 
    [SerializeField] private GameObject _follow;
    [SerializeField] private DrawShape _drawShape;

    private bool _touching = false;
    private Vector2 _lastPos = Vector2.zero;
    private Vector2 _pos;
    private List<Vector2> _points;

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
    

    private void TouchStarted() => _touching = true;
    private void TouchEnded() => _touching = false;
    
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
        Vector2 dirPoints = (_points[1] - _points[0]).normalized;

        //Turned positive to allow both side swipe
        dir = new Vector2(Mathf.Abs( dir.x),  Mathf.Abs(dir.y));
        dirPoints = new Vector2(Mathf.Abs(dirPoints.x), Mathf.Abs(dirPoints.y));
        

        float dot = Vector2.Dot(dir, dirPoints);
        bool sameDirection = dot > 0.9f;
        
        Debug.Log(dot);

        if (sameDirection && (worldPos - _points[1]).magnitude < 0.1f)
        {
            Debug.Log("VALIDATED");
            Controller.OnInputValidate.Invoke();
        }
            
    }
}
