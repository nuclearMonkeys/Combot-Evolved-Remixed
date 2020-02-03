using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankPlayerSelection : MonoBehaviour
{
    public string currentController = string.Empty;
    public bool isReady;
    public GameObject promptCube;
    public int cooldown = 0;
    public int maxCooldown = 60;

    private PlayerInput playerInput;
    public InputManager inputManager;

    [HideInInspector] public Vector3 promptCubePos;

    private void Awake() 
    {
        inputManager = new InputManager();
    }

    private void Start()
    {
        promptCubePos = promptCube.transform.position;
    }

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
        TankSelectionManager.instance.AssignControllerToPlayer(this.gameObject, controllerType);
        
        cooldown = maxCooldown;

        isReady = !isReady;
        
        if(isReady)
            currentController = controllerType;
        else
            currentController = string.Empty;
    }
}
