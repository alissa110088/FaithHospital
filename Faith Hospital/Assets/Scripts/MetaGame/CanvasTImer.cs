using UnityEngine;

public class CanvasTImer : MonoBehaviour
{
    private static CanvasTImer instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}