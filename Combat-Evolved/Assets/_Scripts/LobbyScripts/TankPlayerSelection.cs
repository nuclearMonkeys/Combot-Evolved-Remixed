﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankPlayerSelection : MonoBehaviour
{
    // Start is called before the first frame update
    public string currentController = string.Empty;
    public bool isReady;
    public GameObject promptCube;
    public int cooldown = 0;
    public int maxCooldown = 60;

    private void Update() 
    {
        if (cooldown > 0) 
            cooldown--;
    }

    public void Ready(InputAction.CallbackContext context) 
    {
        if (cooldown > 0)
            return;

        string controllerType = context.control.ToString().Split('/')[1];
        int index = 0;
        TankSelectionManager.instance.AssignControllerToPlayer(ref index, controllerType);
        
        cooldown = maxCooldown;
    }
}
