using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TankSelectionManager : MonoBehaviour
{
    public static TankSelectionManager instance = null;

    public List<GameObject> players = new List<GameObject>();

    [HideInInspector] public List<GameObject> promptCubes = new List<GameObject>();
    [HideInInspector] public List<GameObject> controllerEmblems = new List<GameObject>();
    [HideInInspector] public List<GameObject> readyLines = new List<GameObject>();

    // [SerializeField] public Dictionary<string, string> controllersToPlayers =
    //     new Dictionary<string, string>();

    public GameObject promptCubeContainer;
    public GameObject controllerEmblemContainer;
    public GameObject referencePromptCube;
    public GameObject readyLineContainer;
    public GameObject spawnpoints;
    public GameObject gameCanvas;

    private void Awake() 
    {
        if (!instance)
            instance = this;
        else {
            Destroy(this);
            return;
        }
    
        DontDestroyOnLoad(this.gameObject);

        Transform[] promptCubeArr = promptCubeContainer.GetComponentsInChildren<Transform>(true);
        Transform[] controllerEmblemArr = controllerEmblemContainer.GetComponentsInChildren<Transform>(true);
        Transform[] readyLineArr = readyLineContainer.GetComponentsInChildren<Transform>(true);

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

        for(int i = 0; i < readyLineArr.Length; i++) 
        {
            if (readyLineArr[i].parent != readyLineContainer.transform)
                continue;
            readyLines.Add(readyLineArr[i].gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            CreatePlayer();
        }
    }

    // should only be called by keyboard player
    public void CreatePlayer()
    {
        GameObject newPlayer = Instantiate(GetComponent<PlayerInputManager>().playerPrefab);
        newPlayer.GetComponent<PlayerController>().enableKeyboard = true;
    }

    public void PlayerJoin(PlayerInput playerInput) 
    {
        print("PLAYER JOINED");
        playerInput.gameObject.transform.parent = this.transform;
        int tankID = players.Count;
        playerInput.name = "Player" + tankID;
        // assign tank id and change color
        playerInput.GetComponent<PlayerController>().AssignTankID(tankID);
        // remove prompt cube
        promptCubes[tankID].SetActive(false);
        // set controller logo and show emblem
        controllerEmblems[tankID].SetActive(true);
        // reposition to spawn point
        playerInput.transform.position = spawnpoints.transform.GetChild(tankID).position;
        // add to player list
        players.Add(playerInput.gameObject);
    }

    public void PlayerLeft(PlayerInput playerInput) 
    {
        print("PLAYER LEFT");
        //int tankID = players.Count;
        //// playerInput.GetComponent<PlayerController>().AssignTankID(tankID);

        //promptCubes[tankID].SetActive(true);
        //controllerEmblems[tankID].SetActive(false);
        //players.Remove(playerInput.gameObject);
        //Destroy(playerInput.gameObject);
    }

    public void CheckAllPlayerStatus() {
        int numOfPlayerReady = 0;

        for(int i = 0; i < readyLines.Count; i++) {
            if (readyLines[i].GetComponent<SpriteRenderer>().color == Color.yellow)
                numOfPlayerReady++;
        }

        print("NUMBER PLAYERS READY: " + numOfPlayerReady);
        if (numOfPlayerReady == 1)
            return;

        if (numOfPlayerReady == players.Count)
        {
            gameCanvas.SetActive(true);
            sceneManager.Instance.nextScene();
        }
    }
}
