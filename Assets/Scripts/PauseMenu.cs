using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;

    void Start()
    {
        pauseScreen.SetActive(false);
    }

    public void OnPause()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void OnResume()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void OnSettings()
    {

    }

    public void OnMainMenu()
    {
        //save current position and data
        SceneManager.LoadScene(0);
    }
}
