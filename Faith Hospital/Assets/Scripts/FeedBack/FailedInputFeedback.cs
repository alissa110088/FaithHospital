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

            for (int i = 0; i < afflictionStage.Count; i++)
            {
                if (afflictionStage[i] >= failedSlider.value - 0.05f &&
                    afflictionStage[i] <= failedSlider.value + 0.05f)
                {
                    DrawShape affliction = afflictions[Random.Range(0, afflictions.Length)];
                    affliction.gameObject.SetActive(true);
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
        }
    }

    private void OnFailed()
    {
        if (isInAfflictionStage)
            return;

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