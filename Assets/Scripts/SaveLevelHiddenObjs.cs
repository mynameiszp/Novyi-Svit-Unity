using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLevelHiddenObjs : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            string activeScene = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("SavedLevel", activeScene);
            gameObject.SetActive(false);
            UnityEngine.Debug.Log(PlayerPrefs.GetString("SavedLevel"));
        }
    }
}
