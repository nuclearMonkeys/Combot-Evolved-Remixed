using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resultManager : MonoBehaviour
{
    public int maxScore = 15;

    public GameObject player1bar;
    public GameObject player2bar;
    public GameObject player3bar;
    public GameObject player4bar;

    private int player1score = 12;
    private int player2score = 15;
    private int player3score;
    private int player4score;
    // Start is called before the first frame update
    void Start()
    {
        float p1barPercent = (float)player1score / (float)maxScore * 100f;
        print(p1barPercent);
        player1bar.GetComponent<RectTransform>().localPosition = new Vector3(0f, p1barPercent, 0f);
        player1bar.GetComponent<RectTransform>().offsetMax = new Vector2(player1bar.GetComponent<RectTransform>().offsetMax.x, 106.3f);
    }

    private void Update()
    {
        //player1score += (int)Input.GetAxis("Vertical");
        //player1bar.GetComponent<RectTransform>().localPosition = new Vector3(0f, (float)player1score / (float)maxScore * 100 / 2, 0f);
        //player1bar.GetComponent<RectTransform>().offsetMax = new Vector2(player1bar.GetComponent<RectTransform>().offsetMax.x, 106.3f);
        //print((float)player1score / (float)maxScore * 100 / 2);
    }

}
