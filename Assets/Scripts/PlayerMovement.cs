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

    private CapsuleCollider2D collider;
    private Animator animator;
    private Rigidbody2D rigidbody;
    private float waitBeforeReload = 0.1f;
    private bool isAlive = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
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
        if (collider.IsTouchingLayers(LayerMask.GetMask("Bottom")))
        {
            animator.SetTrigger("hasFallen");
            isAlive = false;
            yield return new WaitForSecondsRealtime(waitBeforeReload);
            //reload level
        }
    }

    IEnumerator OnJump(InputValue input)
    {
        if (input.isPressed && collider.IsTouchingLayers(LayerMask.GetMask("Ground")) && isAlive)
        {
            animator.SetBool("isJumping", input.isPressed);
            rigidbody.velocity = new Vector2(jumpLength, jumpHeigth);
            yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            animator.SetBool("isJumping", false);
        }
    }
}
