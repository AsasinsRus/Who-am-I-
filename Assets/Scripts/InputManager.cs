using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public bool isInteracting;

    public static InputManager Instance { get; private set; }
    private void Awake()
    {
        isInteracting = false;
        Instance = this;
    }

    public void IsInteracting(InputAction.CallbackContext context)
    {
        if (context.performed)
            isInteracting = true;
        else if (context.canceled)
            isInteracting = false;
    }
}
