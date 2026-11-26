using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FailedInputFeedback
    : MonoBehaviour
{
    [FormerlySerializedAs("_failedSlider")] [SerializeField]
    private Slider failedSlider;

    [SerializeField] private int numEventDuringGame;
    [SerializeField] private DrawShape[] afflictions;
    [SerializeField] private List<ListObject> visualAffliction;
    
    private List<float> afflictionStage = new List<float>();
    private int afflictionIndex = 0;
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

        foreach (DrawShape affliction in afflictions)
        {
            affliction.enabled = false;
        }

        Controller.OnInputNotValidate += OnFailed;
        cam = Camera.main.GetComponent<CameraShake>();

        if (GameManager.Instance != null)
            currentPatientBlood = GameManager.Instance.currentPatientBlood;

        if (cam == null)
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
        if (currentPatientBlood != null)
        {
            failedSlider.value = currentPatientBlood.currentBloodLevel;

            if (isInAfflictionStage)
                return;
            
            for (int i = 0; i < afflictionStage.Count; i++)
            {
                if (afflictionStage[i] >= failedSlider.value - 0.05f &&
                    afflictionStage[i] <= failedSlider.value + 0.05f)
                {
                    afflictionIndex = Random.Range(0, afflictions.Length);
                    
                    DrawShape affliction = afflictions[afflictionIndex];
                    affliction.enabled = true;
                    affliction.gameObject.SetActive(true);

                    if (visualAffliction[afflictionIndex].afflictionsVisual[0] != null)
                    {
                        foreach (GameObject visual in visualAffliction[afflictionIndex].afflictionsVisual)
                        {
                            visual.SetActive(true);
                        }
                    }
                        
                    
                    afflictionStage.RemoveAt(i);
                    PatientManager.Instance.AfflictionInterruption(affliction);
                    isInAfflictionStage = true;
                    Controller.Instance.SwitchState(ToolsEnum.all);
                }
            }
        }
    }

    private void OnAfflictionFinished(bool pIsGrouped, State _state = null)
    {
        if (_state._drawShape._nextLayer == -1)
        {
            isInAfflictionStage = false;
            
            if (visualAffliction[afflictionIndex].afflictionsVisual[0] != null)
            {
                foreach (GameObject visual in visualAffliction[afflictionIndex].afflictionsVisual)
                {
                    visual.SetActive(false);
                }
            }
        }
    }

    private void OnFailed()
    {
        // if (isInAfflictionStage)
        //     return;

        if (currentPatientBlood != null)
            currentPatientBlood.OnFail();
        cam.StartShake();
    }

    private int OnOperationEnded()
    {
        currentPatientBlood.BlockDownfall();
        return energyLostFromStart;
    }
}