using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resultManager : MonoBehaviour
{
    public int maxScore = 15;

    public Slider player1bar;
    public GameObject dataHolder;
    //public GameObject player2bar;
    //public GameObject player3bar;
   // public GameObject player4bar;
    
    void Start()
    {
        dataHolder = GameObject.Find("dataHolder");
        int[] data = dataHolder.GetComponent<gameData>().data;
        print(data[0]);
        print(data[1]);
        Destroy(dataHolder);
        //dataHolder.SetActive(false);
        player1bar.value = 1f / 3f;
    }

    private void Update()
    {
    }

}
