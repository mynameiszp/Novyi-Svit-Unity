using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScene : MonoBehaviour
{
    private readonly float _transitionTime = 1f;

    private void Awake()
    {
        PlayerPrefs.DeleteKey("SavedLevel");
        PlayerPrefs.SetInt("IsFirstTimeOpening", 0);
    }

    public void OnClick()
    {
        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene("MainMenu");
    }
}
