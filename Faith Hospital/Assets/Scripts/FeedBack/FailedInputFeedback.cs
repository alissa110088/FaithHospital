using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FailedInputFeedback
    : MonoBehaviour
{
    [FormerlySerializedAs("_failedSlider")] [SerializeField] private Slider failedSlider;
    [SerializeField] private int numEventDuringGame;
    [SerializeField] private DrawShape[] afflictions;
    
    public List<int> afflictionStage = new List<int>();
    private CameraShake cam;
    private bool isInAfflictionStage = false;
    private int energyLostFromStart = 0;

    private void Start()
    {
        PatientManager.Instance.OnOperationFinished += OnOperationEnded;
        Controller.OnInputValidate += OnAfflictionFinished;
        
        for (int i = 0; i < numEventDuringGame; i++)
        {
            int num = Random.Range(0, (int)failedSlider.maxValue);
            afflictionStage.Add(num);
        }

        for (int i = 0; i < afflictions.Length; i++)
        {
            afflictions[i]._state.enabled = false;
        }
        
        Controller.OnInputNotValidate += OnFailed;
        cam = Camera.main.GetComponent<CameraShake>();
        if(cam == null)
            Debug.Log("NO CAMERE SHAKE FOUND");
    }

    private void OnAfflictionFinished(bool pIsGrouped, State _state = null)
    {
        if (_state._drawShape._nextLayer == -1)
        {
            isInAfflictionStage = false;
        }
    }
    
    private void OnFailed()
    {
        if (isInAfflictionStage)
            return;

        if (failedSlider.value != failedSlider.maxValue)
        {
            failedSlider.value ++;
            energyLostFromStart++;
        }
            
        else
        {
            cam.StartShake();
            failedSlider.value = 0;
        }

        if (afflictionStage.Contains((int)failedSlider.value))
        {
            DrawShape affliction = afflictions[Random.Range(0, afflictions.Length)];
            PatientManager.Instance.AfflictionInterruption(affliction);
            isInAfflictionStage = true;
        }
    }

    private int OnOperationEnded()
    {
        return energyLostFromStart;
    }
}
