using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TankSelectionManager : MonoBehaviour
{
    public static TankSelectionManager instance = null;

    public List<GameObject> players = new List<GameObject>();

    //[HideInInspector] 
    public List<GameObject> tanks = new List<GameObject>();
    [HideInInspector] public List<GameObject> promptCubes = new List<GameObject>();
    [HideInInspector] public List<GameObject> controllerEmblems = new List<GameObject>();

    [SerializeField] public Dictionary<string, string> controllersToPlayers =
        new Dictionary<string, string>();

    public GameObject tankContainer;
    public GameObject promptCubeContainer;
    public GameObject controllerEmblemContainer;
    public GameObject referencePromptCube;

    private void Awake() 
    {
        if (!instance)
            instance = this;
        else {
            Destroy(this);
            return;
        }

        Transform[] tankArr = tankContainer.GetComponentsInChildren<Transform>(true);
        Transform[] promptCubeArr = promptCubeContainer.GetComponentsInChildren<Transform>(true);
        Transform[] controllerEmblemArr = controllerEmblemContainer.GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < tankArr.Length; i++) {
            if (tankArr[i].parent != tankContainer.transform)
                continue;
            tanks.Add(tankArr[i].gameObject);
        }

        for(int i = 0; i < promptCubeArr.Length; i++) 
        {
            if (promptCubeArr[i].parent != promptCubeContainer.transform)
                continue;
            promptCubes.Add(promptCubeArr[i].gameObject);
        }

        for(int i = 0; i < controllerEmblemArr.Length; i++) 
        {
            if (controllerEmblemArr[i].parent != controllerEmblemContainer.transform)
                continue;
            controllerEmblems.Add(controllerEmblemArr[i].gameObject);
        }

        
    }

    private void Update() 
    {

    }

    void ConsoleOutput()
    {
        foreach (KeyValuePair<string, string> kvp in controllersToPlayers)
            Debug.Log ("Key = " + kvp.Key + ", Value = " + kvp.Value);
    }

    public void AssignControllerToPlayer(GameObject tps, string controllerType) 
    {
        string value = string.Empty;

        // Look in the dictionary `controllersToPlayers` if there
        // exists a player corressponding to an existing controller.
        // Add the entry to dictionary if entry doesn't exist

        if(!controllersToPlayers.TryGetValue(controllerType + "/" + tps.name, out value)) 
        {
            for (int i = 0; i < tanks.Count; i++) 
            {
                if(!tanks[i].activeSelf) {
                    MapControls(tps, tanks[i]);
                    controllersToPlayers.Add(controllerType + "/" + tps.name, tanks[i].name);
                    tanks[i].SetActive(true);
                    promptCubes[i].SetActive(false);
                    controllerEmblems[i].SetActive(true);
                    break;
                }
            }
        }
        else if (controllersToPlayers.TryGetValue(controllerType + "/" + tps.name, out value)) 
        {
            for (int i = 0; i < tanks.Count; i++) 
            {
                if(controllersToPlayers[controllerType + "/" + tps.name] == tanks[i].name) 
                {
                    controllersToPlayers.Remove(controllerType + "/" + tps.name);
                    tanks[i].SetActive(false);
                    promptCubes[i].SetActive(true);
                    controllerEmblems[i].SetActive(false);
                    break;
                }
            }
        }
    }

    public void MapControls (GameObject tps, GameObject tank) 
    {
        PlayerInput playerInput = tps.GetComponent<PlayerInput>();
        
        PlayerController playerController = tank.GetComponent<PlayerController>();
        InputManager inputManager = tps.GetComponent<TankPlayerSelection>().inputManager;

        inputManager.Player.Rotate.performed += ctx => {print(ctx); playerController.ControllerRotate(ctx);};
        inputManager.Player.Move.performed += ctx => playerController.ControllerMove(ctx);
        inputManager.Player.Fire.performed += ctx => playerController.Fire();
    }

    public void UnmapControls(GameObject tps) {}

    void OnEnable() 
    {

    }
}
