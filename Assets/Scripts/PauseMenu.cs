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

    public void OnRestart()
    {
        Time.timeScale = 1;
        FindObjectOfType<GameSession>().ResetGameSession();
    }

    public void OnSound()
    {
        if (audioMixer.GetFloat("Volume", out float value))
        {
            if (value > -80)
            {
                previousAudioValue = value;
                audioMixer.SetFloat("Volume", -80);
            }
            else audioMixer.SetFloat("Volume", previousAudioValue);
        }
    }

    public void OnMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
