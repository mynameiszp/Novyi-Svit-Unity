using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;
    [SerializeField] AudioMixer audioMixer;
    private float previousAudioValue = 0;

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

    public void OnSound()
    {
        float value;
        if (audioMixer.GetFloat("Volume", out value))
        {
            if (value > -80)
            {
                previousAudioValue = value;
                audioMixer.SetFloat("Volume", -80);
            }
            else audioMixer.SetFloat("Volume", previousAudioValue);
        }
        Debug.Log(value);
    }

    public void OnMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
