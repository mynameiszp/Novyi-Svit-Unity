using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataLoader : MonoBehaviour
{
    private string[] linesRows;
    private bool isEmpty;

    private void Awake()
    {
        WWW linesData = new WWW("http://localhost:3307/noviy_svit_db/LinesData.php");
        while (!linesData.isDone){}
        isEmpty = linesData.text == "";
        if (!isEmpty)
        {
            string linesDataString = linesData.text.Remove(linesData.text.Length - 1);
            linesRows = linesDataString.Split(';');
        }
        else
        {
            SceneManager.LoadScene("ServerError");
        }
    }

    private string GetDataValue(int row, string index)
    {
        string result = null;
        if (!isEmpty)
        {
            string value = linesRows[row].Substring(linesRows[row].IndexOf(index) + index.Length);
            value = value.Remove(value.IndexOf("|"));
            result = value;
        }
        return result;
    }

    public int? GetLinesNumberInScene(string sceneName)
    {
        int? result = null;
        if (!isEmpty)
        {
            int number = 0;
            foreach (var row in linesRows)
            {
                if (row.Contains(sceneName))
                {
                    number++;
                }
            }
            result = number;
        }
        return result;
    }

    public List<string> GetSpeakersInScene(string sceneName)
    {
        List<string> result = null;
        if (!isEmpty)
        {
            List<string> speakers = new List<string>();
            for (int row = 0; row < linesRows.Length; row++)
            {
                if (linesRows[row].Contains(sceneName))
                {
                    name = GetDataValue(row, "Speaker_name:");
                    if (!speakers.Contains(name))
                    {
                        speakers.Add(name);
                    }
                }
            }
            result = speakers;
        }
        return result;
    }

    public string GetCurrentSpeaker(int row)
    {
        return GetDataValue(row, "Speaker_name:");
    }

    public string GetSpeakerText(int row)
    {
        return GetDataValue(row, "Text:");
    }

    public string GetSpeakerAvatarLink(int row)
    {
        return GetDataValue(row, "Avatar_link:");
    }

    public int? GetStartLineNumber(string sceneName)
    {
        int? result = null;
        if (!isEmpty)
        {
            int number = 0;
            for (int row = 0; row < linesRows.Length; row++)
            {
                if (linesRows[row].Contains(sceneName))
                {
                    number = row;
                    break;
                }
            }
            result = number;
        }
        return result;
    }
}
