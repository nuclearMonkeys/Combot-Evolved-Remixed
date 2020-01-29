using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using System;

public class deathMessages : MonoBehaviour
{
    string killerColor = "RED";
    string victimColor = "BLUE";
    string killerName;
    string victimName;
    List<string> killLines;
    List<string> suicideLines;
    string[] players = { "PLAYER 1", "PLAYER 2", "PLAYER 3", "PLAYER 4" };

    bool messageSet = false;
    public float messageDuration = 3f;
    float timeLeft;

    public Text displayText;
    public TextAsset rawText;
    public TextAsset suicideText;
    
    

    //gradually increase transparency to 0;

    // Start is called before the first frame update
    void Start()
    {
        killLines = rawText.text.Split('\n').ToList();
        suicideLines = suicideText.text.Split('\n').ToList();
        displayText.text = "";
    }
    private void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            setMessage(0, 1);
        }

        if (messageSet)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                displayText.text = "";
            }
        }

    }

    string chooseRandomLine()
    {
        //var random = new System.Random();
        int randInt = UnityEngine.Random.Range(0, killLines.Count());
        string line = killLines[randInt];
        string tempKillerName = "<color=" + killerColor + ">" + killerName + "</color>";
        string tempVictimName = "<color=" + victimColor + ">" + victimName + "</color>";
        line = line.Replace("KILLER", tempKillerName);
        line = line.Replace("VICTIM", tempVictimName);
        return line;
    }

    string chooseSuicideLine()
    {
        int randInt = UnityEngine.Random.Range(0, suicideLines.Count());
        string line = suicideLines[randInt];
        string tempVictimName = "<color=" + victimColor + ">" + victimName + "</color>";
        line = line.Replace("VICTIM", tempVictimName);
        return line;
    }

    public void setMessage(int killer, int victim)
    {
        messageSet = true;
        timeLeft = messageDuration;
        killerName = players[killer];
        victimName = players[victim];
        string line = "An error occured while displaying the kill message";
        if (killer != victim && victim != -1 && killer != -1)
        {
            line = chooseRandomLine();
        }
        else
        {
            line = chooseSuicideLine();
        }
        displayText.text = line;
    }
    


}
