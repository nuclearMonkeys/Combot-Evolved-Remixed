using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resultManager : MonoBehaviour
{
    int[] data = { 12, 10, 2, 9, 15, 4 };
    public Slider player1bar;
    public Slider player2bar;
    public Slider player3bar;
    public Slider player4bar;
    private GameObject dataHolder = null;
    bool dataSet = false;
    //public GameObject player2bar;
    //public GameObject player3bar;
   // public GameObject player4bar;
    
    void Start()
    {
        GameObject mngr = GameObject.Find("Manager");
        if (mngr != null)
        {
            ScoreboardManagerScript sms = mngr.GetComponentInChildren<ScoreboardManagerScript>();
            if (sms != null)
            {
                dataHolder = sms.dataHolder;
            }
            if (dataHolder != null)
            {
                data = dataHolder.GetComponent<gameData>().data;
                dataSet = true;
            }
        }
        
        
        
        player1bar.value = (float)data[0] / (float)data[4];
        player1bar.gameObject.GetComponentInChildren<Text>().text = "PLAYER 1\n" + data[0] + " kills";
        player2bar.value = (float)data[1] / (float)data[4];
        player2bar.gameObject.GetComponentInChildren<Text>().text = "PLAYER 2\n" + data[1] + " kills";
        player3bar.value = (float)data[2] / (float)data[4];
        player3bar.gameObject.GetComponentInChildren<Text>().text = "PLAYER 3\n" + data[2] + " kills";
        player4bar.value = (float)data[3] / (float)data[4];
        player4bar.gameObject.GetComponentInChildren<Text>().text = "PLAYER 4\n" + data[3] + " kills";
        //print(data[5] > 2 || data[2] > 0);
        player3bar.gameObject.SetActive(data[5] > 2 || data[2] > 0);
        player4bar.gameObject.SetActive(data[5] > 3 || data[3] > 0);
        //print(player3bar.IsActive() || player4bar.IsActive());
        Destroy(GameObject.Find("Manager"));

    }
    

}
