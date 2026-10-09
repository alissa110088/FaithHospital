using UnityEngine;

public class TouchInput : State
{
    private ControlState _state = ControlState.touch;

    private bool _touchStarted;
    private bool _firtPos;
    public int[] countPoints;
    
    
    protected override void OnEnable()
    {
        base.OnEnable();
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
        unlockState -= OnStart;
        if (inputManager != null)
        {
            inputManager.onToucheStart -= TouchStarted;
            inputManager.onTouchingPos -= GetFirstPos;
            inputManager.onToucheEnd -= TouchEnded;
            
            inputManager = null;
        }
        
        DisableImage(false, this);
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
            countPoints = new int[_points.Count];
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
        
        int hitIndex = -1;
        for (int i = 0; i < _points.Count; i++)
        {
            if (!CheckIfInRange(worldPos3, _points[i])) continue;

            if (countPoints[i] == 0) { hitIndex = i; break; } 
            if (hitIndex == -1) hitIndex = i;                 
        }

        if (hitIndex == -1) return;

        Controller.OnInputStepFinished.Invoke(this, pPosition);
        countPoints[hitIndex]++;

        foreach (int value in countPoints)
            if (value == 0) return;

        if (_drawShape._groupped)
        {
            _countGrouped--;
            if (_countGrouped > 0)
            {
                Controller.OnInputValidate.Invoke(true,this);
                _patternValidated = true;
                return;
            }
        }
        
        Controller.OnInputValidate.Invoke(false, this);
    }

    private void DisableImage(bool pBool, State state)
    {
        if (state != this)
            return;

        foreach (SpriteRenderer i in _drawShape.visualFeedBack)
        {
            i.enabled = false;
        }
    }
}