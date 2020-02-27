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

        if(!SceneManager.GetActiveScene().name.Equals("Lobby"))
        {
            // for test scenes
            PlayerController[] pcs = FindObjectsOfType<PlayerController>();
            foreach (PlayerController pc in pcs)
            {
                players.Add(pc.gameObject);
                pc.enableKeyboard = true;
            }
            return;
        }

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
        // Spawning players with keyboard
        if(Input.GetKeyDown(KeyCode.Return) && players.Count < GetComponent<PlayerInputManager>().maxPlayerCount && SceneManager.GetActiveScene().name == "Lobby")
        {
            GameObject newPlayer = Instantiate(GetComponent<PlayerInputManager>().playerPrefab);
            newPlayer.GetComponentInChildren<PlayerController>().enableKeyboard = true;
        }
    }

    private int FindOpenPlayerSlot()
    {
        for (int i = 0; i < promptCubes.Count; i++)
        {
            if(promptCubes[i].activeSelf)
            {
                return i;
            }
        }
        return -1;
    }

    public void PlayerJoin(PlayerInput playerInput) 
    {
        AudioManager.instance.PlaySound("Ready");
        // places player under this object to carry over scenes
        playerInput.gameObject.transform.parent = this.transform;
        int tankID = FindOpenPlayerSlot();
        playerInput.name = "Player" + tankID;
        // assign tank id and change color
        playerInput.GetComponentInChildren<PlayerController>().AssignTankID(tankID);
        // remove prompt cube
        promptCubes[tankID].SetActive(false);
        // set controller logo and show emblem
        controllerEmblems[tankID].SetActive(true);
        // reposition to spawn point
        playerInput.transform.Find("Player").transform.position = spawnpoints.transform.GetChild(tankID).position;
        // add to player list
        players.Add(playerInput.transform.Find("Player").gameObject);
        // disable bars
        playerInput.transform.Find("Player").Find("Bars").gameObject.SetActive(false);
    }

    public void PlayerLeft(PlayerInput playerInput) 
    {
        int tankID = playerInput.GetComponentInChildren<PlayerController>().tankID;
        // if left inside of lobby
        if(SceneManager.GetActiveScene().name == "Lobby")
        {
            promptCubes[tankID].SetActive(true);
            promptCubes[tankID].transform.rotation = Quaternion.identity;
            controllerEmblems[tankID].SetActive(false);
        }
        players.Remove(playerInput.transform.Find("Player").gameObject);
        Destroy(playerInput.gameObject);
    }

    public void CheckAllPlayerStatus() {
        int numOfPlayerReady = 0;

        for(int i = 0; i < readyLines.Count; i++) {
            if (readyLines[i].GetComponent<SpriteRenderer>().color == Color.yellow)
                numOfPlayerReady++;
        }

        //print("NUMBER PLAYERS READY: " + numOfPlayerReady);
        if (numOfPlayerReady == 1)
            return;

        if (numOfPlayerReady == players.Count)
        {
            // activate canvas
            gameCanvas.SetActive(true);
            // move to next scene
            sceneManager.Instance.nextScene();
        }
    }
}
