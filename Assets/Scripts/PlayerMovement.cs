using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpLength = 3f;
    [SerializeField] float jumpHeigth = 5f;

    private CapsuleCollider2D bodyCollider;
    private BoxCollider2D feetCollider;
    private Animator animator;
    private Rigidbody2D rigidbody;
    private float waitBeforeReload = 0.1f;
    private bool isAlive = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (isAlive)
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Bottom")) 
        || bodyCollider.IsTouchingLayers(LayerMask.GetMask("Bottom")))
        {
            animator.SetTrigger("hasFallen");
            isAlive = false;
            yield return new WaitForSecondsRealtime(waitBeforeReload);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    IEnumerator OnJump(InputValue input)
    {
        if (input.isPressed && feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && isAlive)
        {
            animator.SetBool("isJumping", input.isPressed);
            rigidbody.velocity = new Vector2(jumpLength, jumpHeigth);
            yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            animator.SetBool("isJumping", false);
        }
    }
}
