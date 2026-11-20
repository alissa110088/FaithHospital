using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FailedInputFeedback
    : MonoBehaviour
{
    [FormerlySerializedAs("_failedSlider")] [SerializeField] private Slider failedSlider;
    [SerializeField] private int numEventDuringGame;
    [SerializeField] private DrawShape[] afflictions;
    
    private List<float> afflictionStage = new List<float>();
    private CameraShake cam;
    private bool isInAfflictionStage = false;
    private int energyLostFromStart = 0;
    private PatientBloodLevel currentPatientBlood;

    private void Start()
    {
        PatientManager.Instance.OnOperationFinished += OnOperationEnded;
        Controller.OnInputValidate += OnAfflictionFinished;
        
        for (int i = 0; i < numEventDuringGame; i++)
        {
            float num = Random.Range(0, failedSlider.maxValue);
            afflictionStage.Add(num);
        }

        for (int i = 0; i < afflictions.Length; i++)
        {
            afflictions[i]._state.enabled = false;
        }
        
        Controller.OnInputNotValidate += OnFailed;
        cam = Camera.main.GetComponent<CameraShake>();
        currentPatientBlood = GameManager.Instance.currentPatientBlood;
        
        if(cam == null)
            Debug.Log("NO CAMERE SHAKE FOUND");
    }

    private void OnDestroy()
    {
        PatientManager.Instance.OnOperationFinished -= OnOperationEnded;
        Controller.OnInputValidate -= OnAfflictionFinished;
        Controller.OnInputNotValidate -= OnFailed;
    }

    private void Update()
    {
        if(currentPatientBlood != null)
         failedSlider.value = currentPatientBlood.currentBloodLevel;
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
        
        currentPatientBlood.OnFail();
        cam.StartShake();

        if (afflictionStage.Contains(failedSlider.value))
        {
            DrawShape affliction = afflictions[Random.Range(0, afflictions.Length)];
            PatientManager.Instance.AfflictionInterruption(affliction);
            isInAfflictionStage = true;
        }
    }

    private int OnOperationEnded()
    {
        currentPatientBlood.BlockDownfall();
        return energyLostFromStart;
    }
}
