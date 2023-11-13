using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLevelController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string activeScene = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("SavedLevel", activeScene);
            gameObject.SetActive(false);
        }
    }
}
