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
    private GameObject dataHolder;
    //public GameObject player2bar;
    //public GameObject player3bar;
   // public GameObject player4bar;
    
    void Start()
    {
        dataHolder = GameObject.Find("dataHolder");
        if (dataHolder != null)
        {
            data = dataHolder.GetComponent<gameData>().data;
            Destroy(dataHolder);
        }
        
        //dataHolder.SetActive(false);
        player1bar.value = (float)data[0] / (float)data[4];
        player2bar.value = (float)data[1] / (float)data[4];
        player3bar.value = (float)data[2] / (float)data[4];
        player4bar.value = (float)data[3] / (float)data[4];
        player3bar.gameObject.SetActive(data[5] > 2 || data[2] > 0);
        player4bar.gameObject.SetActive(data[5] > 3 || data[3] > 0);
        

    }

    private void Update()
    {
    }

}
