using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AvatarsManager : MonoBehaviour
{
    [SerializeField] private SpeakersDatabase _database;
    private Dictionary<string, Sprite> _avatarDictionary;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        _avatarDictionary = new Dictionary<string, Sprite>();

        _avatarDictionary = _database.speakers
            .GroupBy(speaker => speaker.name)
            .ToDictionary(speaker => speaker.Key, speaker => speaker.First().avatar);
    }

    public Sprite GetSpeakerAvatar(string name)
    {
        if (_avatarDictionary.TryGetValue(name, out Sprite avatar))
        {
            return avatar;
        }
        else
        {
            return null;
        }
    }
}
