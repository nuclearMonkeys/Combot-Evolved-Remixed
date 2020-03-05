using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingText : MonoBehaviour
{
    public Text textComponent;
    float scrollSpeed = 2;
    string text;
    float delay = .1f;
    float show = 1;

    private void Start()
    {
        textComponent.text = "";
        transform.localPosition = new Vector3(0, 2, 0);
        ScrollText();
    }

    public void SetText(string text)
    {
        this.text = text;
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
        yield return new WaitForSeconds(show);
        Destroy(gameObject);
    }
}
