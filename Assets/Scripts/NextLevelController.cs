using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NextLevelController : MonoBehaviour
{
    [SerializeField] Animator transition;
    [SerializeField] float transitionTime = 1f;
    private GameObject ground;

    private void Awake() {
        ground = GameObject.FindWithTag("GroundTilemap");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadLevel(other.GetComponent<PlayerInput>(), SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    IEnumerator LoadLevel(PlayerInput playerInput, int levelIndex)
    {
        transition.SetTrigger("Start");
        playerInput.DeactivateInput();
        if (ground != null && ground.GetComponent<SurfaceEffector2D>() != null)
        {
            ground.GetComponent<SurfaceEffector2D>().speed = 0f;
        }
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
