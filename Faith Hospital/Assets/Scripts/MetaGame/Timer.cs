using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _timerDuration;
    private Image _image;
    private float _currentTime = 0f;
    private float _divider;
    private bool _timerStarted = false;

    private void OnEnable()
    {
        _image = GetComponent<Image>();
        StartTimer();
    }
    
    private void StartTimer()
    {
        _divider = 1 / _timerDuration;
        _timerStarted = true;
        _currentTime = 0;
    }

    private void Update()
    {
        if (!_timerStarted)
            return;
        
        _currentTime += Time.deltaTime;
        _image.fillAmount = _currentTime *  _divider;

    }
}
