using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager 
{
    private InputSystem_Actions action;
    public Action onToucheStart;
    public Action onToucheEnd;
    public Action<Vector2> onTouchingPos;

    public InputManager()
    {
        action = new InputSystem_Actions();
        action.Enable();

        action.Touch.PrimaryContact.started += StartTouchPrimary;
        action.Touch.PrimaryContact.canceled += EndTouchPrimary;
        action.Touch.PrimaryPosition.performed += Position;
    }

    ~ InputManager()
    {
        action.Touch.PrimaryContact.started -= StartTouchPrimary;
        action.Touch.PrimaryContact.canceled -= EndTouchPrimary;
        action.Touch.PrimaryPosition.performed -= Position;

        action.Disable();
    }

    private void StartTouchPrimary(InputAction.CallbackContext ctx)
    {
        onToucheStart?.Invoke();
    }

    private void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        onToucheEnd?.Invoke();
    }

    private void Position(InputAction.CallbackContext ctx)
    {
        onTouchingPos?.Invoke(ctx.ReadValue<Vector2>());
        // drawShape.pos = ctx.ReadValue<Vector2>();
    }
}