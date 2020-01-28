using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerLobby : MonoBehaviour
{
    public void Activate(InputAction.CallbackContext context) 
    {
        print(context);
        print("ACTIVATE");
    }
}
