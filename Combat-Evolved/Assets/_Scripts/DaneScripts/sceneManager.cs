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
        currentLiving = GameObject.FindGameObjectsWithTag("Player").Length;
        if (currentLiving <= 1)
        {
            countdownLength -= Time.deltaTime;
        }
        if (countdownLength <= 0)
        {
            countdownLength = 3f;
            //nextScene();
        }

    }

    void nextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void updateKills(int index)
    {
        kills[index] += 1;
    }

    string chooseRandomLevel()
    {
        int i = Random.Range(0, levels.Count);
        return levels[i];
    }
}
