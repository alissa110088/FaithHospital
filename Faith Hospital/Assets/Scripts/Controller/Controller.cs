using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    [SerializeField] private ControlState _currentState = ControlState.SwipeFollow;

    private SwipeFollow _swipeFollow;

    private InputManager _inputManager;

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
    }

    public void SwitchState(ControlState pState, GameObject pTool = null)
    {
        _currentState = pState;
        if (_currentState == ControlState.SwipeFollow)
        {
            Debug.Log("SCALPEL SELECTED");
            _swipeFollow.follow = pTool;
            _swipeFollow.enabled = true;
        }
        else if (_currentState == ControlState.none)
        {
            _swipeFollow.enabled = false;
            _swipeFollow.follow = null;
        }
    }
}