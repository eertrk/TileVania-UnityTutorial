using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float jumpSpeed = 5f;
    public float climbSpeed = 5f;
    [SerializeField] Vector2 deathkick = new Vector2(2f, 5f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    bool isGrounded;
    CapsuleCollider2D myCapsuleCollider;
    float startGravity;
    BoxCollider2D footCollider;

    bool isAlive = true;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        startGravity = GetComponent<Rigidbody2D>().gravityScale;
        footCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!isAlive) { return; }

        Run();
        FlipSprite();
        ClimbLeadder();
        Die();
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
    }

    void Die()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemies","Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathkick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        isGrounded = footCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (value.isPressed && isGrounded)
        {
            myRigidbody.velocity = new Vector2(0f, jumpSpeed);
        }
    }

    void ClimbLeadder()
    {
        bool isClimbing= footCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));

        if (!isClimbing)
        {
            myRigidbody.gravityScale = startGravity;
            return;
        }

        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0;

        bool verticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", verticalSpeed);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool horizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", horizontalSpeed);       
    }

    void FlipSprite()
    {
        bool horizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (horizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }
}
