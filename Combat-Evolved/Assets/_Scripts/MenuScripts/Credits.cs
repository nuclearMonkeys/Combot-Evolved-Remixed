using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Credits : MonoBehaviour
{
    public GameObject menuSelector;
    public string scene = "";
    int cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldown > 0)
            cooldown--;
    }

    public void ChangeSelection(InputAction.CallbackContext context) {
        if(cooldown > 0)
            return;

        menuSelector.SetActive(true);

        GetComponent<AudioSource>().Play();

        cooldown = 10;
    }

    public void Transition() 
    {
        if (menuSelector)
            SceneManager.LoadScene("Scenes/" + scene);
        Destroy(this.gameObject);
    }
}
