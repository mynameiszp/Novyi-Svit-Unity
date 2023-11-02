using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NextLevelNoExit : MonoBehaviour
{
    [SerializeField] Animator transition;
    [SerializeField] float transitionTime = 2f;
    private GameObject ground;

    private void Awake()
    {
        ground = GameObject.FindWithTag("GroundTilemap");
    }

    public void LoadNextLevel(int levelIndex)
    {
        StartCoroutine(LoadLevel(levelIndex));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        if (ground != null && ground.GetComponent<SurfaceEffector2D>() != null)
        {
            ground.GetComponent<SurfaceEffector2D>().speed = 0f;
        }
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
