using UnityEngine;

public class Cote : Organs
{
    [SerializeField] private DrawShape _touch;
    
    private MeshRenderer _meshRenderer;
    private Rigidbody _rb;
    protected override void Start()
    {
        base.Start();
        if(_touch != null)
            _animsStep[_touch._state] = (Vector2 pPos) => BreakCotes(_touch._state, pPos);
        _meshRenderer = GetComponent<MeshRenderer>();
        _rb =  GetComponent<Rigidbody>();
    }

    private void BreakCotes(State pState, Vector2 pPosition)
    {
        Camera cam = Camera.main;

        Vector3 bounds = _meshRenderer.bounds.extents;

        Ray ray = cam.ScreenPointToRay(pPosition);

        if (_meshRenderer.bounds.IntersectRay(ray))
        {
            gameObject.layer = LayerMask.NameToLayer("Attrapable");
            _rb.useGravity = false;
            _meshRenderer.material.color = Color.red;
        }
        
    }
}
