using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameData : MonoBehaviour
{
    public int[] data;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void setData(int[] newData)
    {
        data = newData;
    }
}
