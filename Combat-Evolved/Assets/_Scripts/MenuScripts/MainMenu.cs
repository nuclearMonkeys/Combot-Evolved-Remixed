using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject startPrompt;
    public GameObject controllerPrompt1;
    public GameObject controllerPrompt2;
    public List<GameObject> options = new List<GameObject>();
    public GameObject menuSelector;
    int cooldown = 0;
    int index = 0;

    // Start is called before the first frame update
    void Update() 
    {
        if (cooldown > 0)
            cooldown--;
    }

    void Start()
    {
        StartCoroutine(BlinkShow());
    }
    
    private IEnumerator BlinkShow() 
    {
        // startPrompt.SetActive(true);
        controllerPrompt1.SetActive(true);
        controllerPrompt2.SetActive(true);
        yield return new WaitForSecondsRealtime(1.0f);
        StartCoroutine(BlinkUnshow());
    }

    private IEnumerator BlinkUnshow()
    {
        // startPrompt.SetActive(false);
        controllerPrompt1.SetActive(false);
        controllerPrompt2.SetActive(false);
        yield return new WaitForSecondsRealtime(1.0f);
        StartCoroutine(BlinkShow());
    }

    public void ChangeSelection(InputAction.CallbackContext context) {
        if(cooldown > 0)
            return;
        
        Vector2 vec = context.ReadValue<Vector2>();
        print(vec);
        
        if(vec.x != 0)
            return;

        if(vec.y == 1) 
            index--;
        else
            index++;
            
        if(index == 0 || index == 3) 
        {
            menuSelector.GetComponent<RectTransform>().localPosition = new Vector3(3, -76);
            index = 0;
        }
        else if(index == 1) 
        {
            menuSelector.GetComponent<RectTransform>().localPosition = new Vector3(3, -115);
        }
        else if(index == 2 || index == -1) 
        {
            menuSelector.GetComponent<RectTransform>().localPosition = new Vector3(3, -154);
            index = 2;
        }


        GetComponent<AudioSource>().Play();

        cooldown = 20;
    }

    public void Transition() 
    {
        if (index == 0)
            SceneManager.LoadScene("Scenes/Lobby");
        else if (index == 1)
            SceneManager.LoadScene("Scenes/Credits");
        else
            Application.Quit();
        Destroy(this.gameObject);
    }
}
