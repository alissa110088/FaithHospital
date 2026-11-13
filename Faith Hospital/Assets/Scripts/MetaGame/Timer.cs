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
    }
    
    public void StartTimer()
    {
        Debug.Log("Starting Timer");
        _currentTime = 0;
        _image.fillAmount = 0;
        _divider = 1 / _timerDuration;
        _timerStarted = true;
    }

    private void Update()
    {
        if (!_timerStarted)
            return;
        
        _currentTime += Time.deltaTime;
        _image.fillAmount = _currentTime *  _divider;
        if (_image.fillAmount == 1)
        {
            Debug.Log("Ending Timer");
            _currentTime = 0;
            _image.fillAmount = 0;
            _timerStarted = false;
            GameManager.Instance.OnTimerEnded.Invoke();
        }

    }
}
