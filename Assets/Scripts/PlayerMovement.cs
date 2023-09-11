using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpLength = 3f;
    [SerializeField] float jumpHeigth = 5f;

    CapsuleCollider2D collider;
    Animator animator;
    Rigidbody2D rigidbody;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {

    }

    IEnumerator OnJump(InputValue input)
    {
        if (input.isPressed && collider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            animator.SetBool("isJumping", input.isPressed);
            rigidbody.velocity = new Vector2(jumpLength, jumpHeigth);
            yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            animator.SetBool("isJumping", false);
        }

    }
}
