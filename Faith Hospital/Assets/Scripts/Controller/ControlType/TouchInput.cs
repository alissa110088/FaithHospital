using UnityEngine;

public class TouchInput : State
{
    private ControlState _state = ControlState.touch;

    private bool _touchStarted;
    private bool _firtPos;

    private void OnEnable()
    {
        unlockState += OnStart;
        
        if (_drawShape == null)
            return;

        foreach (SpriteRenderer i in _drawShape.visualFeedBack)
        {
            i.enabled = true;
        }
    }

    private void OnDisable()
    {
        unlockState -= OnStart;
        if (inputManager != null)
        {
            inputManager.onToucheStart -= TouchStarted;
            inputManager.onTouchingPos -= GetFirstPos;
            inputManager.onToucheEnd -= TouchEnded;
            
            inputManager = null;
        }
        
        DisableImage(this);
    }

    private void Start()
    {
        Controller.OnInputValidate += DisableImage;
    }

    private void OnStart(InputManager pInputManager, ControlState controlState)
    {
        if (controlState == _state)
        {
            if (inputManager != null)
            {
                return;
            }

            inputManager = pInputManager;
            inputManager.onToucheStart += TouchStarted;
            inputManager.onTouchingPos += GetFirstPos;
            inputManager.onToucheEnd += TouchEnded;
            if (!_drawShape.isActiveAndEnabled)
                return;

            _points = _drawShape.points;
        }
        else if (inputManager != null)
        {
            inputManager.onToucheStart -= TouchStarted;
            inputManager.onTouchingPos -= GetFirstPos;
            inputManager.onToucheEnd -= TouchEnded;
            _points = null;
            inputManager = null;
        }
    }

    private void TouchStarted()
    {
        _touchStarted = true;
    }

    private void TouchEnded()
    {
        _touchStarted = false;
        _firtPos = false;
    }

    private void GetFirstPos(Vector2 pPosition)
    {
        if (!_touchStarted || _firtPos || float.IsInfinity(pPosition.x) || float.IsInfinity(pPosition.y) ||
            !_drawShape.isActiveAndEnabled || _points.Count < 1)
            return;

        float distance = -Camera.main.transform.position.z;

        Vector3 worldPos3 = Camera.main.ScreenToWorldPoint(
            new Vector3(pPosition.x, pPosition.y, distance)
        );

        _firtPos = true;

        foreach (Vector2 pos in _points)
        {
            if (CheckIfInRange(worldPos3, pos))
            {
                Controller.OnInputStepFinished.Invoke(this, pPosition);
                _points.Remove(pos);
                if (_points.Count == 0)
                {
                }

                return;
            }
        }
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