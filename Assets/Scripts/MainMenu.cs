using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Canvas settingsCanvas;
    
    void Awake()
    {
        settingsCanvas.gameObject.SetActive(false);
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("IsFirstTimeOpening"))
        {
            PlayerPrefs.SetInt("IsFirstTimeOpening", 1);
        }
        else
        {
            PlayerPrefs.SetInt("IsFirstTimeOpening", 0);
        }
    }

    public void OnPlay()
    {
        if (PlayerPrefs.GetInt("IsFirstTimeOpening") == 0 && PlayerPrefs.HasKey("SavedLevel"))
        {
            string levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        if (PlayerPrefs.GetInt("IsFirstTimeOpening") == 0 && !PlayerPrefs.HasKey("SavedLevel"))
        {
            SceneManager.LoadScene(1);
        }
        if (PlayerPrefs.GetInt("IsFirstTimeOpening") == 1)
        {
            SceneManager.LoadScene(1);
            PlayerPrefs.SetInt("IsFirstTimeOpening", 0);
        }
    }

    public void OnRestart()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            PlayerPrefs.DeleteKey("SavedLevel");
            PlayerPrefs.SetInt("IsFirstTimeOpening", 0);
        }
        SceneManager.LoadScene(1);
    }

    public void OnSettings()
    {
        settingsCanvas.gameObject.SetActive(true);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
