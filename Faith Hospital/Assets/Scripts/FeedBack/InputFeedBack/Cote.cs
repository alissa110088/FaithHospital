using UnityEngine;

public class Cote : Organs
{
    [SerializeField] private DrawShape _touch;
    
    private MeshRenderer _meshRenderer;
    private float _range = 1f;
    private Rigidbody _rb;
    [SerializeField] private int _hitIndex;
    protected override void Start()
    {
        base.Start();
        if(_touch != null)
            _animsStep[_touch._state] = (Vector2 pPos, int _hitIndex) => BreakCotes(_touch._state, pPos, _hitIndex);
        _meshRenderer = GetComponent<MeshRenderer>();
        _rb =  GetComponent<Rigidbody>();
    }
    
    private void BreakCotes(State pState, Vector2 pPosition, int pHitIndex)
    {
        if (pHitIndex != _hitIndex)
            return;
        
        // Vector3 world = ScreenToWorldOnPlane(pPosition);
        //
        // if (!CheckIfInRange(world, transform.position)) return;

        gameObject.layer = LayerMask.NameToLayer("Attrapable");
        _rb.useGravity = false;
        _meshRenderer.material.color = Color.red;
    }
    private Vector3 ScreenToWorldOnPlane(Vector2 pPos)
    {
        float distance = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(new Vector3(pPos.x, pPos.y, distance));
    }
    
    private bool CheckIfInRange(Vector3 pPosition, Vector3 pPoint)
    {
        if (Mathf.Abs((pPosition.x - pPoint.x)) < _range && Mathf.Abs((pPosition.y - pPoint.y)) < _range)
        {
            return true;
        }

        return false;
    }
    
}
