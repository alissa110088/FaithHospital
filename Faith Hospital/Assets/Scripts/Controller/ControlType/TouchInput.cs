using UnityEngine;

public class TouchInput : State
{
    
    private ControlState _state = ControlState.touch;
    
    private bool _touchStarted;
    private bool _firtPos;
    private void OnEnable()
    {
        unlockState += OnStart;
    }

    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.onToucheStart -= TouchStarted;
            inputManager.onTouchingPos -= GetFirstPos;
            inputManager.onToucheEnd -= TouchEnded;
        }
    }

    private void OnStart(InputManager pInputManager, ControlState controlState)
    {
        if (controlState == _state)
        {
            _points = GetComponent<DrawShape>().points;
            Debug.Log("HAHA");
            inputManager = pInputManager;
            inputManager.onToucheStart += TouchStarted;
            inputManager.onTouchingPos += GetFirstPos;
            inputManager.onToucheEnd += TouchEnded;
        }
        else if (inputManager != null)
        {
            inputManager.onToucheStart -= TouchStarted;
            inputManager.onTouchingPos -= GetFirstPos;
            inputManager.onToucheEnd -= TouchEnded;
            _points = null;
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
        if(!_touchStarted || _firtPos)
            return;
        
        _firtPos = true;

        foreach (Vector2 pos in _points)
        {
            if (CheckIfInRange(pPosition, pos))
            {
                _points.Remove(pos);
                Debug.Log("TOUCHER");
                if (_points.Count == 0)
                {
                    Debug.Log("FINIIT");
                }
                return;
            }
        }
        
    }
}
