using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Trebuchet
{
    public void FireInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            ReleaseArm();
        }
    }
    public void RearmInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            RearmTrebuchet();
        }
    }
    public void MovementInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
        {
            Vector2 input = context.ReadValue<Vector2>();
            currentRotationValue = input.x;
        }
    }

    public void AdjustReleaseAngleInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
        {
            Vector2 input = context.ReadValue<Vector2>();

            UpdateReleaseAngle(input.y);
        }
    }
}
