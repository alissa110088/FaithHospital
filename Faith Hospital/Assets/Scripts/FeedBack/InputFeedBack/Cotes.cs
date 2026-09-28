using UnityEngine;

public class Cotes : Organs
{
    [SerializeField] private DrawShape _touch;
    
    protected override void Start()
    {
        base.Start();
        _animsStep[_touch._state] = (Vector2 pPos) => BreakCotes(_touch._state, pPos);
    }

    private void BreakCotes(State state, Vector2 _position)
    {
        Ray ray = Camera.main.ScreenPointToRay(_position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("Cotes"))
            {
                Debug.Log("Toucher cote");
                hit.collider.gameObject.layer = LayerMask.NameToLayer("Attrapable");
                Rigidbody rb = hit.collider.gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
                
            }
        }
    }
}
