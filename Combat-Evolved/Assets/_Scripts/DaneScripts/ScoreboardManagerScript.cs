using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScoreboardManagerScript : MonoBehaviour
{
    public static ScoreboardManagerScript instance;

    
    float maxSeconds = 300;
    public int displaySeconds;
    private string secondString;
    public Text timer;
    public Transform scoreboardManager;
    private int[] playerKills = { 0, 0, 0, 0 };
    public int killThreshold = 15;
    public int numPlayers = 2;

    public GameObject dataHolder;

    private GameObject spot0;
    private GameObject spot1;
    private GameObject spot2;
    private GameObject spot3;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        updateScores(-1);
    }

    void Update()
    {
        if (numPlayers > 4)
        {
            numPlayers = 4;
        }
        maxSeconds -= Time.smoothDeltaTime;
        displaySeconds = (int)maxSeconds;
        timer.text = ((int) displaySeconds / 60 )+ ":" + displaySeconds % 60 ;
        
        if (Input.GetKeyDown("o"))
        {
            updateScores(0);
        }

        if (displaySeconds <= 0 || killThresholdReached())
        {
            dataHolder.GetComponent<gameData>().setData(getData());
            SceneManager.LoadScene("resultsScreen");
        }
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
    public void updateScores(int playerIndex)
    {
        if (playerIndex != -1)
        {
            playerKills[playerIndex]++;
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

}
