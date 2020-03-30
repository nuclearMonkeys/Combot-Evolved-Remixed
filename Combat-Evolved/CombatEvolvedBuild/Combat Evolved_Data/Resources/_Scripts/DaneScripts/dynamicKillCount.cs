using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dynamicKillCount : MonoBehaviour
{
    public GameObject sms;
    public GameObject text;

    public bool sub;
    public bool add;

    private void Awake()
    {
        sms = GameObject.Find("ScoreboardManager");
        text = GameObject.Find("Kill Threshold");
    }

    private void Start()
    {
        updateKillThreshold();
    }

    void updateKillThreshold()
    {
        text.GetComponent<TextMeshProUGUI>().text = "KILL THRESHOLD\n" + sms.GetComponent<ScoreboardManagerScript>().killThreshold;
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            if (collision.GetComponent<BulletBase>().source.tankID == 0 && sub)
            {
                sms.GetComponent<ScoreboardManagerScript>().killThreshold--;
            }
            else if (collision.GetComponent<BulletBase>().source.tankID == 1 && add)
            {
                sms.GetComponent<ScoreboardManagerScript>().killThreshold++;
            }
            if (sms.GetComponent<ScoreboardManagerScript>().killThreshold < 1)
            {
                sms.GetComponent<ScoreboardManagerScript>().killThreshold = 1;
            }
            if (sms.GetComponent<ScoreboardManagerScript>().killThreshold > 99)
            {
                sms.GetComponent<ScoreboardManagerScript>().killThreshold = 99;
            }
            Destroy(collision.gameObject);
            updateKillThreshold();
        }
    }
}
