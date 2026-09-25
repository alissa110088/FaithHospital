using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    [SerializeField] private ControlState _currentState = ControlState.SwipeFollow;

    private SwipeFollow _swipeFollow;

    public InputManager inputManager;

    public static Action OnInputValidate; //TODO ENLEVER
    public static Action OnInputNotValidate;
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
        _swipeFollow = GetComponent<SwipeFollow>();
        inputManager = new InputManager();

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);
    }

    public void SwitchState(ControlState pState)
    {
        _currentState = pState;


        State.unlockState.Invoke(inputManager, pState);
        
    }
}