using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    [SerializeField] private ToolsEnum _currentTool;

    public InputManager inputManager;

    public static Action<bool,State> OnInputValidate; 
    public static Action<int> OnInputValidateOrgan; 
    public static Action OnInputNotValidate;
    public static Action<State, Vector2, float> OnInputStepFinished;
    public static Controller Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        inputManager = new InputManager();

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);
    }

    public void SwitchState(ToolsEnum pTool)
    {
        Debug.Log(pTool);
        _currentTool = pTool;
        State.unlockState.Invoke(inputManager, pTool);
        
    }
}