using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class sceneManager : MonoBehaviour
{
    //the 4 indexes are the current player indexes
    private int[] kills = {0, 0, 0, 0};
    private int maxKills = 15;
    private int players = 2;
    private List<string> scenes;
    public int currentLiving = 2;
    float countdownLength = 3f;
    List<string> levels = new List<string>();

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
        print(chooseRandomLevel());
    }

    // Update is called once per frame
    void Update()
    {
        /*
        currentLiving = GameObject.FindGameObjectsWithTag("Player").Length;
        if (currentLiving <= 1)
        {
            countdownLength -= Time.deltaTime;
        }
        if (countdownLength <= 0)
        {
            countdownLength = 3f;
            //nextScene();
        }*/
        

    }

    public void nextScene(string sceneName = "")
    {
        if (sceneName == "")
        {
            sceneName = chooseRandomLevel();
        }
        SceneManager.LoadScene(sceneName);
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
        return players;
    }
}
