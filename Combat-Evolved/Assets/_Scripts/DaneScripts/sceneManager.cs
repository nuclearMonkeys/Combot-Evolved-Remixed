using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;

public class sceneManager : MonoBehaviour
{

    

    //the 4 indexes are the current player indexes
    private int[] kills = {0, 0, 0, 0};
    private int maxKills = 15;
    private List<string> scenes;
    public int currentLiving = 2;
    public float countdownLength = 3f;
    List<string> levels = new List<string>();

    private static sceneManager _instance;

    public static sceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<sceneManager>();
            }

            return _instance;
        }

    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath + "/Scenes/Levels";
        var files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.EndsWith(".unity"))
            {
                levels.Add(file);
            }
        }
        PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            nextScene();
        }

    }

    public void nextScene(string sceneName = "")
    {
        if (sceneName == "")
        {
            sceneName = chooseRandomLevel();
        }
        if (currentLiving <= 1 || SceneManager.GetActiveScene().name == "Lobby")
        {
            StartCoroutine(startTimer(countdownLength, sceneName));
        }
    }

    public void updateKills(int index)
    {
        kills[index] += 1;
    }

    private string chooseRandomLevel()
    {
        int i = Random.Range(0, levels.Count);
        string level = levels[i];
        level = Path.GetFileName(level);
        level = level.Substring(0, level.Length - 6);
        return level;
    }

    public int[] getKillCounts()
    {
        return kills;
    }

    IEnumerator startTimer(float seconds, string sceneName)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(sceneName);
    }

    //called when the new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("Loading Scene!");
        // find all players
        List<GameObject> players = TankSelectionManager.instance.players;
        //resets the current living player count to the proper max
        currentLiving = players.Count;
        // find spawn points
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("spawnPoint");
        List<GameObject> spawnPoints = new List<GameObject>(spawns);

        //clears the camera of players
        if(CameraController.instance)
            CameraController.instance.targets.Clear();
        foreach (GameObject player in players)
        {
            //adds each player to the camera
            if (CameraController.instance)
                CameraController.instance.targets.Add(player.transform);

            //resets each player's health to max and sets the player to be active
            player.GetComponentInChildren<PlayerHealth>().ResetHealth();
            player.SetActive(true);

            //spawns the player at a random spawn point
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            GameObject spawnPos = spawnPoints[spawnIndex];
            spawnPoints.Remove(spawnPos);
            player.transform.position = spawnPos.transform.position;
        }
    }
}
