using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardManagerScript : MonoBehaviour
{
    public static ScoreboardManagerScript Instance { get; private set; }

    
    float maxSeconds = 300;
    public int displaySeconds;
    private string secondString;
    public Text timer;
    public Transform scoreboardManager;
    public int player1kills = 0;
    public int player2kills = 0;
    public int player3kills = 0;
    public int player4kills = 0;
    public int numPlayers = 2;

    private GameObject spot0;
    private GameObject spot1;
    private GameObject spot2;
    private GameObject spot3;



    void Update()
    {
        if (numPlayers > 4)
        {
            numPlayers = 4;
        }
        maxSeconds -= Time.smoothDeltaTime;
        displaySeconds = (int)maxSeconds;
        timer.text = ((int) displaySeconds / 60 )+ ":" + displaySeconds % 60 ;
        
        if (numPlayers == 2)
        {
            spot0 = scoreboardManager.transform.Find("Spot0Background").gameObject;
            spot0.SetActive(false);
            spot3 = scoreboardManager.transform.Find("Spot3Background").gameObject;
            spot3.SetActive(false);

            spot1 = scoreboardManager.transform.Find("Spot1Background").gameObject;
            spot1.transform.Find("killCount").gameObject.GetComponent<Text>().text = "Player 1\n" + player1kills;

            spot2 = scoreboardManager.transform.Find("Spot2Background").gameObject;
            spot2.transform.Find("killCount").gameObject.GetComponent<Text>().text = "Player 2\n" + player2kills;
        }
        else if (numPlayers > 2)
        {
            spot0 = scoreboardManager.transform.Find("Spot0Background").gameObject;
            spot0.transform.Find("killCount").gameObject.GetComponent<Text>().text = "Player 1\n" + player1kills;
            spot0.SetActive(true);

            spot1 = scoreboardManager.transform.Find("Spot1Background").gameObject;
            spot1.transform.Find("killCount").gameObject.GetComponent<Text>().text = "Player 2\n" + player2kills;

            spot2 = scoreboardManager.transform.Find("Spot2Background").gameObject;
            spot2.transform.Find("killCount").gameObject.GetComponent<Text>().text = "Player 3\n" + player3kills;

            spot3 = scoreboardManager.transform.Find("Spot3Background").gameObject;
            spot3.SetActive((numPlayers == 4));
            spot3.transform.Find("killCount").gameObject.GetComponent<Text>().text = "Player 4\n" + player4kills;



        }
    }
}
