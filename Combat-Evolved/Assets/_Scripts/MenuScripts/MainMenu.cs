using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject startPrompt;
    public GameObject controllerPrompt1;
    public GameObject controllerPrompt2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BlinkShow());
    }
    
    private IEnumerator BlinkShow() 
    {
        startPrompt.SetActive(true);
        controllerPrompt1.SetActive(true);
        controllerPrompt2.SetActive(true);
        yield return new WaitForSecondsRealtime(1.0f);
        StartCoroutine(BlinkUnshow());
    }

    private IEnumerator BlinkUnshow()
    {
        startPrompt.SetActive(false);
        controllerPrompt1.SetActive(false);
        controllerPrompt2.SetActive(false);
        yield return new WaitForSecondsRealtime(1.0f);
        StartCoroutine(BlinkShow());
    }
}
