using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [SerializeField] GameObject player;

    void Start()
    {
    }

    void Update()
    {
        transform.position = new Vector2(player.transform.position.x, transform.position.y);
    }
}
