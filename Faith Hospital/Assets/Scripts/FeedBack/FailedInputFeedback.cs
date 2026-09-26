using UnityEngine;
using UnityEngine.UI;

public class FailedInputFeedback
    : MonoBehaviour
{
    [SerializeField] private Slider _failedSlider;
    private CameraShake cam;

    private void Start()
    {
        Controller.OnInputNotValidate += OnFailed;
        cam = Camera.main.GetComponent<CameraShake>();
        if(cam == null)
            Debug.Log("NO CAMERE SHAKE FOUND");
    }
    
    private void OnFailed()
    {
        if(_failedSlider.value != _failedSlider.maxValue)
            _failedSlider.value ++;

        else
        {
            cam.StartShake();
            _failedSlider.value = 0;
        }
    }
}
