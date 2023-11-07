using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    private string[] linesRows;
    private bool isEmpty;
    
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        StartCoroutine(OutputRoutine("http://localhost:3307/noviy_svit_db/LinesData.php"));
    } 

    private IEnumerator OutputRoutine(string url)
    {
        var linesData = new UnityWebRequest(url);
        linesData.downloadHandler = new DownloadHandlerBuffer();
        yield return linesData.SendWebRequest();
        string result = linesData.downloadHandler.text;
        while (!linesData.isDone) { }
        isEmpty = result == "";
        if (!isEmpty)
        {
            string linesDataString = result.Remove(result.Length - 1);
            linesRows = linesDataString.Split(';');
        }
        else
        {
            SceneManager.LoadScene("ServerError");
        }
    }

    public string[] GetLinesRows()
    {
        return linesRows;
    }

    public void ProcessPlayerDeath()
    {
        ResetGameSession();
    }

    void ResetGameSession()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Destroy(gameObject);
    }
}
