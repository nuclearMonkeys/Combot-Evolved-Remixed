using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerLobby : MonoBehaviour
{
    public bool isReady;
    public int cooldown = 0;
    public int maxCooldown = 60;
    public GameObject tank;
    public GameObject promptCube;

    void Start() 
    {
        tank.SetActive(isReady);
    }

    void LateUpdate() 
    {
        if (cooldown > 0)
            cooldown--;

        if (Input.GetAxis("Submit") == 1)
            Activate();
    }

    public void Activate() 
    {
        if (cooldown > 0)
            return;

        isReady = !isReady;
        print("ACTIVATE");
        cooldown = maxCooldown;
        tank.SetActive(isReady); 
        promptCube.SetActive(isReady);
    }
}
