using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    //the 4 indexes are the current player indexes
    private int[] kills = {0, 0, 0, 0};
    private int maxKills = 15;
    private int players = 2;
    private List<string> scenes;
    public int currentLiving = 2;
    float countdownLength = 3f;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
            nextScene();
        }

    }

    void nextScene()
    {
        print("next scene");
        SceneManager.LoadScene("PlayerTest");
    }

    public void updateKills(int index)
    {
        kills[index] += 1;
    }
}
