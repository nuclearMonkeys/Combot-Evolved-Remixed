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
    private int playerCount = 2;
    private List<string> scenes;
    public int currentLiving = 2;
    public float countdownLength = 3f;
    List<string> levels = new List<string>();
    List<GameObject> players = new List<GameObject>();

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
        foreach (PlayerController pc in playerControllers)
        {
            players.Add(pc.gameObject);
        }
        //players = GameObject.FindObjectsOfType<PlayerController>();
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
        print("next scene called");
        if (sceneName == "")
        {
            sceneName = chooseRandomLevel();
        }
        currentLiving = GameObject.FindGameObjectsWithTag("Player").Length;
        if (currentLiving <= 1 || SceneManager.GetActiveScene().name == "LyndonScene")
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

    public int getPlayerCount()
    {
        return playerCount;
    }

    IEnumerator startTimer(float seconds, string sceneName)
    {
        print("entered timer");
        yield return new WaitForSeconds(seconds);
        print("loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("spawnPoint");
        List<GameObject> spawnPoints = new List<GameObject>(spawns);
        foreach (GameObject player in players)
        {
            player.SetActive(true);
            GameObject spawnPos = spawnPoints[Random.Range(0, spawnPoints.Count)];
            spawnPoints.Remove(spawnPos);
            player.GetComponent<Transform>().position = spawnPos.GetComponent<Transform>().position;
        }
    }

    
}
