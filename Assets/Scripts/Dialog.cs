using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    private bool canMove = true;
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        canMove = false;
        ///
        
    }

    void ChatWithCharacter(){

    }
}
