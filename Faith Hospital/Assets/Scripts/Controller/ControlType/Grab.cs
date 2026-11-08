using UnityEngine;

public class Grab : State
{
    [SerializeField] private OrganCollector _organCollector;

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

    private void OnStart(InputManager pInputManager, ToolsEnum pTool)
    {
        if (pTool == _tool)
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

                //hardcoder c moche
                if (clicked.layer != LayerMask.NameToLayer("Attrapable") || clicked.tag != "Layer" + PatientManager.Instance._currentLayer)
                    return;

                if (_organCollector.isNew)
                {
                    _organCollector.Close();
                }
                else
                {
                    _organCollector.Appear();
                }
                
                
                OrganMagnet.ReleaseOrgan.Invoke(clicked);
                _grabbed = clicked;
                _rb = _grabbed.GetComponent<Rigidbody>();
                _rb.isKinematic = false;
                _rb.useGravity = false;
                _rb.linearDamping = 10f;
                _firtPos = true;
            }
        }

        if (_grabbed == null)
            return;

        Ray planeRay = Camera.main.ScreenPointToRay(pPosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0f, 5f, 0f));
        if (!plane.Raycast(planeRay, out float enter))
            return;

        Vector3 worldPos3 = planeRay.GetPoint(enter);


        _targetPos = new Vector3(worldPos3.x,5f , worldPos3.z);
        _rb.MovePosition(Vector3.Lerp(_grabbed.transform.position, _targetPos,
            Time.deltaTime * 5));
    }
}