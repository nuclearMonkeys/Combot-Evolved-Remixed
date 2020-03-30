using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public string mainMenuSceneName;
    public bool isPaused;
    GameObject menuObject;

    private void Start()
    {
        instance = this;
        menuObject = transform.Find("PauseMenuPanel").gameObject;
        menuObject.SetActive(false);
        isPaused = false;
        transform.parent.gameObject.SetActive(false);
    }

    // Called by Players
    // Script Usage: PauseMenu.instance.PauseGame()
    public void PauseGame()
    {
        Time.timeScale = 0;
        menuObject.SetActive(true);
        isPaused = true;
    }

    // Called by Resume Button
    public void ResumeGame()
    {
        Time.timeScale = 1;
        menuObject.SetActive(false);
        isPaused = false;
    }

    // Called by Exit Button
    public void ExitGame()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
