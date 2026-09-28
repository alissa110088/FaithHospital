using UnityEngine;

public class Grab : State
{
    private ControlState _state = ControlState.none;

    private bool _touchStarted;
    private bool _firtPos;
    private GameObject _grabbed;
    private Rigidbody _rb;
    private Vector3 _targetPos;

    private void OnEnable()
    {
        unlockState += OnStart;
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
    }

    private void FixedUpdate()
    {
        if (_rb == null) return;

        Vector3 toTarget = _targetPos - _rb.position;
        _rb.linearVelocity = toTarget * 4f;  
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
        }
        else if (inputManager != null)
        {
            inputManager.onToucheStart -= TouchStarted;
            inputManager.onTouchingPos -= GetFirstPos;
            inputManager.onToucheEnd -= TouchEnded;
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
        _grabbed = null;
        if (_rb != null)
        {
            _rb.useGravity = true;
            _rb.linearDamping = 0f;          
            _rb = null;
        }
        
    }

    private void GetFirstPos(Vector2 pPosition)
    {
        if (!_touchStarted || float.IsInfinity(pPosition.x) || float.IsInfinity(pPosition.y))
            return;

        if (!_firtPos)
        {
            Ray ray = Camera.main.ScreenPointToRay(pPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clicked = hit.collider.gameObject;

                if (clicked.layer != LayerMask.NameToLayer("Attrapable"))
                    return;
                
                _grabbed = clicked;
                _rb = _grabbed.GetComponent<Rigidbody>();
                _rb.useGravity = false;       
                _rb.linearDamping = 10f;    
                _firtPos = true; }
        }
        
        if(_grabbed == null)
            return;

        float distance = -Camera.main.transform.position.z;

        Vector3 worldPos3 = Camera.main.ScreenToWorldPoint(
            new Vector3(pPosition.x, pPosition.y, distance)
        );

        _targetPos = new Vector3(worldPos3.x, worldPos3.y, -4f);
        _rb.MovePosition( Vector3.Lerp(_grabbed.transform.position, new Vector3(worldPos3.x, worldPos3.y, -4f), Time.deltaTime * 5));
    }
}