using UnityEngine;
using Vector2 = UnityEngine.Vector2;

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
        DrawDebugRays(_position);
        
        Camera cam = Camera.main;
        ;
        Ray ray = cam.ScreenPointToRay(_position);

        Vector3 right = cam.transform.right * 0.5f;
        Vector3 up = cam.transform.up * 0.5f;
        
        Vector3[] direction = new[]
        {
            Vector3.zero,
            // right,
            // -right, 
            // up, 
            // -up, 
        };

        for (int i = 0; i < direction.Length; i++)
        {
            Ray rayOffseet = new Ray(ray.origin + direction[i], ray.direction);
            if (Physics.Raycast(rayOffseet, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("Cotes") && hit.collider.gameObject.layer != LayerMask.NameToLayer("Attrapable"))
                {
                    Debug.Log("Toucher cote");
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("Attrapable");
                    Rigidbody rb = hit.collider.gameObject.AddComponent<Rigidbody>();
                    rb.useGravity = false;
                
                }
                else
                {
                    Debug.Log(hit.collider.gameObject.name);
                }
            }
        }
    }
    
    private void DrawDebugRays(Vector2 _position)
    {
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(_position);

        Vector3 right = cam.transform.right * 0.5f;
        Vector3 up = cam.transform.up * 0.5f;

        Vector3[] offsets = { Vector3.zero, right, -right, up, -up };
        Color[] colors = { Color.white, Color.red, Color.green, Color.blue, Color.yellow };

        for (int i = 0; i < offsets.Length; i++)
        {
            Vector3 origin = ray.origin + offsets[i];
            Vector3 end = Physics.Raycast(origin, ray.direction, out RaycastHit hit, 100f)
                ? hit.point
                : origin + ray.direction * 100f;

            Debug.DrawLine(origin, end, colors[i], 3f);
        }
    }
}
