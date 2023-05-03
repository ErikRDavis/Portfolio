using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player
{
    public delegate void OnChangeCameraView(int value);
    public event OnChangeCameraView onChangeCameraView;
    public delegate void OnRotateCamera(Vector2 value);
    public event OnRotateCamera onRotateCamera;
    public delegate void OnToggleMenu();
    public event OnToggleMenu onToggleMenu;

    public void CycleCameraView(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            onChangeCameraView?.Invoke((int)input.x);
        }
    }

    public void RotateCameraInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
        {
            onRotateCamera?.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void UIToggleMenuInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onToggleMenu?.Invoke();
        }
    }
}
