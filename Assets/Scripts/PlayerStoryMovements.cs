using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStoryMovements : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;

    private CapsuleCollider2D bodyCollider;
    private BoxCollider2D feetCollider;
    private Animator animator;
    private Rigidbody2D rigidbody;
    private Vector2 moveInput;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Run();
        FlipPlayer();
    }


    void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }

    void Run()
    {
        transform.Translate(moveInput * runSpeed * Time.deltaTime);
        bool isMoving = Mathf.Abs(moveInput.x) > 0;
        animator.SetBool("isRunning", isMoving);
    }

    void FlipPlayer()
    {
        bool isMoving = Mathf.Abs(moveInput.x) > 0;
        if (isMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(moveInput.x), 1f);
        }
    }
}
