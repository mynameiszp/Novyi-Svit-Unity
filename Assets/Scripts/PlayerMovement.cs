using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpLength = 15f;
    [SerializeField] float jumpHeigth = 18f;
    [SerializeField] Tilemap ground;

    private BoxCollider2D bodyCollider;
    private CapsuleCollider2D feetCollider;
    private Animator animator;
    private Rigidbody2D rigidbody;
    private float waitBeforeReload = 0.05f;
    private bool isAlive = true;
    private SurfaceEffector2D surface;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<BoxCollider2D>();
        feetCollider = GetComponent<CapsuleCollider2D>();
        surface = ground.GetComponent<SurfaceEffector2D>();
    }

    void Update()
    {
        if (isAlive)
        {
            Move();
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
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && isAlive)
        {
            surface.speed = 15f;
            animator.SetBool("isJumping", true);
            rigidbody.velocity = new Vector2(jumpLength, jumpHeigth);
            yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            animator.SetBool("isJumping", false);
        }
    }

    void Move()
    {
        Vector2 currentPosition = rigidbody.transform.position;
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            rigidbody.transform.position = currentPosition + new Vector2(-1, 0);
            surface.speed = 5f;
        }
    }
}
