using UnityEngine;

public class Grab : State
{
    private ControlState _state = ControlState.none;

    private bool _touchStarted;
    private bool _firtPos;
    private GameObject _grabbed;
    private Rigidbody rb;

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
        if (rb != null)
        {
            rb.isKinematic = false;
            rb = null;
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

                if (!clicked.CompareTag("Attrapable"))
                    return;
                
                _grabbed = clicked;
                rb = _grabbed.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                _firtPos = true;
            }
        }
        
        if(_grabbed == null)
            return;

        float distance = -Camera.main.transform.position.z;

        Vector3 worldPos3 = Camera.main.ScreenToWorldPoint(
            new Vector3(pPosition.x, pPosition.y, distance)
        );


        _grabbed.transform.position = Vector3.Lerp(_grabbed.transform.position, new Vector3(worldPos3.x, worldPos3.y, -4f),
            Time.deltaTime * 10);
    }
}