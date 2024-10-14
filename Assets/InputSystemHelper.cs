using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemHelper : MonoBehaviour
{
    public GunSystem gunSystem;


    public void OnFire(InputAction.CallbackContext callbackContext)
    {
        gunSystem.MyInput();
    }


    public void OnReload(InputAction.CallbackContext callbackContext)
    {
        gunSystem.Reload();
    }
}
