using UnityEngine;
using UnityEngine.UI;

public class FailedInputFeedback
    : MonoBehaviour
{
    [SerializeField] private Slider _failedSlider;

    private void Start()
    {
        Controller.OnInputNotValidate += OnFailed;
    }
    
    private void OnFailed()
    {
        if(_failedSlider.value != _failedSlider.maxValue)
            _failedSlider.value ++;

        else
        {
            _failedSlider.value = 0;
        }
    }
}
