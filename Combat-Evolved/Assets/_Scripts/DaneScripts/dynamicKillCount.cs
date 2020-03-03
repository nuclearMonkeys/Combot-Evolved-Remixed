using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dynamicKillCount : MonoBehaviour
{
    public GameObject sms;
    public GameObject text;

    private void Start()
    {
        updateKillThreshold();
    }

    void updateKillThreshold()
    {
        text.GetComponent<TextMeshProUGUI>().text = "KILL THRESHOLD\n-\t" + sms.GetComponent<ScoreboardManagerScript>().killThreshold + "\t+";
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            if (collision.GetComponent<BulletBase>().source.tankID == 0)
            {
                sms.GetComponent<ScoreboardManagerScript>().killThreshold--;
            }
            else if (collision.GetComponent<BulletBase>().source.tankID == 1)
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
