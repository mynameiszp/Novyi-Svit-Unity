using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStoryData : MonoBehaviour
{
    private GameSession gameSession;
    private string[] linesRows;
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        linesRows = gameSession.GetLinesRows();
    }

    private string GetDataValue(int row, string index)
    {
        string value = linesRows[row].Substring(linesRows[row].IndexOf(index) + index.Length);
        value = value.Remove(value.IndexOf("|"));
        return value;
    }

    public int GetLinesNumberInScene(string sceneName)
    {
        int number = 0;
        foreach (var row in linesRows)
        {
            if (row.Contains(sceneName))
            {
                number++;
            }
        }
        return number;
    }

    public List<string> GetSpeakersInScene(string sceneName)
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
        return speakers;
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

    public int GetStartLineNumber(string sceneName)
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
        return number;
    }
}
