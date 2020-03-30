using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScoreboardManagerScript : MonoBehaviour
{
    public static ScoreboardManagerScript instance;

    public float totalSeconds = 300f;
    private float currentTime;
    public int displaySeconds;
    private string secondString;
    public Text timer;
    public Transform scoreboardManager;
    private int[] playerKills = { 0, 0, 0, 0 };
    public int killThreshold = 15;
    public int numPlayers = 2;

    public int suicidePenalty = 1;

    string oldScene;

    public GameObject dataHolder;

    private GameObject spot0;
    private GameObject spot1;
    private GameObject spot2;
    private GameObject spot3;

    bool finalSceneCalled = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        numPlayers = TankSelectionManager.instance.players.Count;
        updateScores(-1);
        currentTime = totalSeconds;
        oldScene = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        //print(oldScene + " " + SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name != oldScene)
        {
            currentTime = totalSeconds;
        }
        if (numPlayers < 2)
        {
            numPlayers = TankSelectionManager.instance.players.Count;
            updateScores(-1);
        }
        
        if (numPlayers > 4)
        {
            numPlayers = 4;
        }
        currentTime -= Time.smoothDeltaTime;
        displaySeconds = (int)currentTime;
        if (displaySeconds % 60 >= 10)
        {
            timer.text = ((int) displaySeconds / 60 )+ ":" + displaySeconds % 60 ;
        }
        else
        {
            timer.text = ((int)displaySeconds / 60) + ":0" + displaySeconds % 60;
        }
        
        if (currentTime <= 61f)
        {
            if ((int)currentTime % 10 == 0)
            {
                EncapsulatingWall.instance.Encapsulate();
            }
        }

        if ((displaySeconds <= 0 || killThresholdReached()) && (finalSceneCalled == false))//
        {
            finalSceneCalled = true;
            dataHolder.GetComponent<gameData>().setData(getData());
            SceneManager.LoadScene("resultsScreen");
        }
        oldScene = SceneManager.GetActiveScene().name;
    }

    //returns true if any player has reached the kill threshold
    bool killThresholdReached()
    {
        for (int i = 0; i < 4; ++i)
        {
            if (playerKills[i] >= killThreshold)
            {
                return true;
            }
        }
        return false;

    }

    //updates the score of the player by index, with 0 representing player 1, etc. Also updates the canvas
    //Pass -1 to not update any scores and just make sure the scoreboard is updated
    public void updateScores(int playerIndex, bool isSuicide = false)
    {
        if (playerIndex != -1 && isSuicide == false)
        {
            playerKills[playerIndex]++;
        }

        if (isSuicide)
        {
            playerKills[playerIndex] -= suicidePenalty;
        }
        

        if (numPlayers == 2)
        {
            spot0 = scoreboardManager.transform.Find("Spot0Background").gameObject;
            spot0.SetActive(false);
            spot3 = scoreboardManager.transform.Find("Spot3Background").gameObject;
            spot3.SetActive(false);

            spot1 = scoreboardManager.transform.Find("Spot1Background").gameObject;
            spot1.transform.Find("killCount").gameObject.GetComponent<Text>().text = "Player 1\n" + playerKills[0];

            spot2 = scoreboardManager.transform.Find("Spot2Background").gameObject;
            spot2.transform.Find("killCount").gameObject.GetComponent<Text>().text = "Player 2\n" + playerKills[1];
        }
        else if (numPlayers > 2)
        {
            spot0 = scoreboardManager.transform.Find("Spot0Background").gameObject;
            spot0.transform.Find("killCount").gameObject.GetComponent<Text>().text = "Player 1\n" + playerKills[0];
            spot0.SetActive(true);

            spot1 = scoreboardManager.transform.Find("Spot1Background").gameObject;
            spot1.transform.Find("killCount").gameObject.GetComponent<Text>().text = "Player 2\n" + playerKills[1];

            spot2 = scoreboardManager.transform.Find("Spot2Background").gameObject;
            spot2.transform.Find("killCount").gameObject.GetComponent<Text>().text = "Player 3\n" + playerKills[2];

            spot3 = scoreboardManager.transform.Find("Spot3Background").gameObject;
            spot3.SetActive((numPlayers == 4));
            spot3.transform.Find("killCount").gameObject.GetComponent<Text>().text = "Player 4\n" + playerKills[3];
        }

    }

    public int[] getData()
    {
        int[] data = new int[6];
        for (int i = 0; i < 4; ++i)
        {
            data[i] = playerKills[i];
        }
        data[4] = killThreshold;
        data[5] = numPlayers;
        return data;
    }

    public void resetTime() { currentTime = totalSeconds; }

}
