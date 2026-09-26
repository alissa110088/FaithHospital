using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private bool _isCameraShaking;
    private Vector3 _posOrigin;
    private Coroutine _shakeCoroutine;
    [SerializeField] private float shakeIntensity = 1f;

    
    public void StartShake()
    {
        _posOrigin = Camera.main.transform.position;
        if (_shakeCoroutine != null)
        {
            Camera.main.transform.position = _posOrigin;
            StopCoroutine(_shakeCoroutine);
            _shakeCoroutine = null;
        }
        _shakeCoroutine= StartCoroutine(ScreenShake());
    }

    private IEnumerator ScreenShake()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 offset = Random.insideUnitCircle * shakeIntensity;
            Camera.main.transform.position = new Vector3(
                _posOrigin.x + offset.x,
                _posOrigin.y + offset.y,
                _posOrigin.z
            );
            yield return new WaitForSeconds(0.05f);
        }

        Camera.main.transform.position = _posOrigin;
    }
}