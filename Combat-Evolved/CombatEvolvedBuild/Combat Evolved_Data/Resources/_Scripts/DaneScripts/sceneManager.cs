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
    string lastScene;
    public string testingScene = "";
    bool isPlaying = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextScene(string sceneName = "")
    {
        
        sceneName = testingScene;
        if (sceneName == "")
        {
            sceneName = chooseRandomLevel();
            while (sceneName == lastScene)
            {
                sceneName = chooseRandomLevel();
            }
            lastScene = sceneName;
        }
        if (currentLiving <= 1 || SceneManager.GetActiveScene().name == "Lobby")
        {
            StartCoroutine(startTimer(countdownLength, sceneName));
        }
    }

    public void backToMainMenu() 
    {
        SceneManager.LoadScene("Scenes/StartupScreen");
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

    public string chooseSoundtrack()
    {
        string song = "";
        List<string> songs = new List<string>();
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Audio/Resources");
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            if ((f.Name.EndsWith("wav") || f.Name.EndsWith("mp3")) && f.Name.StartsWith("soundtrack"))
            {
                songs.Add(f.Name);
            }
        }
        song = songs[Random.Range(0, songs.Count)];
        song = song.Remove(song.LastIndexOf('.'));
        return song;
    }

    IEnumerator startTimer(float seconds, string sceneName)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(sceneName);
        ScoreboardManagerScript.instance.resetTime();
        if (isPlaying == false)
        {
            isPlaying = true;
            AudioManager.instance.PlaySound(chooseSoundtrack(), null, false, 0.35f);
        }
    }
}
