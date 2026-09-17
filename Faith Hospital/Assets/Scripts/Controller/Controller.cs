using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private ControlState _currentState = ControlState.SwipeFollow;
    
    private SwipeFollow _swipeFollow;

    private InputManager _inputManager;

    private void OnEnable()
    {
        _swipeFollow = GetComponent<SwipeFollow>();
    }

    private void Start()
    {
        SwitchState(ControlState.SwipeFollow);
    }

    private void SwitchState(ControlState  pState)
    {
        _currentState = pState;
        if (_currentState == ControlState.SwipeFollow)
        {
            _swipeFollow.enabled = true;
        }
    }
}
