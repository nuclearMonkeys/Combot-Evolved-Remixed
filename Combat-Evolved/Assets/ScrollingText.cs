using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingText : MonoBehaviour
{
    public string text;
    public Text textComponent;
    public float scrollSpeed = 1;
    float delay = .1f;

    private void Start()
    {
        textComponent.text = "";
        //ScrollText();
    }

    void ScrollText()
    {
        StartCoroutine(ScrollTextEnumerator());
    }

    IEnumerator ScrollTextEnumerator()
    {
        textComponent.text = "";
        foreach(char c in text)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(delay / scrollSpeed);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ScrollText();
        }
    }
}
