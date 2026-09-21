using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private bool isCameraShaking;
    private Vector3 posOrigin;
    [SerializeField] private float shakeIntensity = 1f;

    public void StartShake()
    {
        posOrigin = Camera.main.transform.position;
        StartCoroutine(ScreenShake());
    }

    private IEnumerator ScreenShake()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 offset = Random.insideUnitCircle * shakeIntensity;
            Camera.main.transform.position = new Vector3(
                posOrigin.x + offset.x,
                posOrigin.y + offset.y,
                posOrigin.z
            );
            yield return new WaitForSeconds(0.05f);
        }

        Camera.main.transform.position = posOrigin;
    }
}