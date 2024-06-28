using UnityEngine;

public class AvatarsManager : MonoBehaviour
{
    [SerializeField] private SpeakersDatabase _database;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
