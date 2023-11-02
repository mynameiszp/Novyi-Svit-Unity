using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelHiddenObjs : MonoBehaviour
{
    [SerializeField] Animator transition;
    [SerializeField] float transitionTime = 1f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(position, Vector3.zero);
            if (hit && hit.collider != null)
            {
                StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
            }
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
