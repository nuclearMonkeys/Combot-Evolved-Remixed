using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TankSelectionManager : MonoBehaviour
{
    public static TankSelectionManager instance = null;

    public List<GameObject> players = new List<GameObject>();

    public List<GameObject> promptCubes = new List<GameObject>();
    [HideInInspector] public List<GameObject> readyLines = new List<GameObject>();

    public GameObject promptCubeContainer;
    public GameObject referencePromptCube;
    public GameObject readyLineContainer;
    public GameObject spawnpoints;
    public GameObject gameCanvas;

    private void Awake() 
    {
        if (!instance){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
            return;
        }
    

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
    }

    private void Start() {
        AudioManager.instance.PlaySound(name = "soundtrack00", null, false, 1f, true);
        promptCubeContainer = GameObject.Find("PromptCubeContainer");
        readyLineContainer = GameObject.Find("ReadyPlayerContainer");
        referencePromptCube = GameObject.Find("ReferencePromptCube");
        spawnpoints = GameObject.Find("SpawnPoints");

        Transform[] promptCubeArr = promptCubeContainer.GetComponentsInChildren<Transform>(true);
        Transform[] readyLineArr = readyLineContainer.GetComponentsInChildren<Transform>(true);

        for(int i = 0; i < promptCubeArr.Length; i++) 
        {
            if (promptCubeArr[i].parent != promptCubeContainer.transform)
                continue;
            promptCubes.Add(promptCubeArr[i].gameObject);
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
        playerInput.GetComponentInChildren<PlayerStamina>().maxStamina = 999;
        // remove prompt cube
        promptCubes[tankID].SetActive(false);
        // set controller logo and show emblem
        // controllerEmblems[tankID].SetActive(true);
        readyLines[tankID].GetComponent<SpriteRenderer>().color = Color.red;
        // reposition to spawn point
        playerInput.transform.Find("Player").transform.position = spawnpoints.transform.GetChild(tankID).position;
        // add to player list
        players.Add(playerInput.transform.Find("Player").gameObject);
        // disable bars
        playerInput.transform.Find("Player").Find("Bars").gameObject.SetActive(false);
    }

    public void PlayerLeft(PlayerInput playerInput) 
    {
        AudioManager.instance.PlaySound("Unready");
        int tankID = playerInput.GetComponentInChildren<PlayerController>().tankID;
        // if left inside of lobby
        if(SceneManager.GetActiveScene().name == "Lobby")
        {
            print(tankID);
            promptCubes[tankID].SetActive(true);
            promptCubes[tankID].transform.rotation = Quaternion.identity;
            // controllerEmblems[tankID].SetActive(false);
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
            for(int i = 0; i < players.Count; i++) { 
                PlayerStamina playerStamina = players[i].GetComponentInChildren<PlayerStamina>();
                playerStamina.maxStamina = 5;
                playerStamina.staminaSlider.maxValue = playerStamina.maxStamina;
                playerStamina.currentStamina = playerStamina.maxStamina;
            }
            AudioManager.instance.StopSound("soundtrack00");
            // activate canvas
            gameCanvas.SetActive(true);
            // move to next scene
            sceneManager.Instance.nextScene();
        }
    }
}
