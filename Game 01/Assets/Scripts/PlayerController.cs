using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    enum State { idle,working};

    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private SpriteRenderer playerRenderer;
    private AudioSource playerAudio;

    private State state = State.idle;
    private int moveSpeed = 0;
    private bool isGrounded = true;
    private float jumpForce = 3.5f;

    public AudioClip attackClip;
    public AudioClip walkClip;



    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerRenderer = GetComponent<SpriteRenderer>();
        playerAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (state == State.idle)
        {
            Move();
            Jump();
            Attack();
        }
    }

    void FixedUpdate()
    {
        if (state == State.idle)
            playerAnimator.SetFloat("velocityY", playerRigidbody.velocity.y);
    }
    void Move()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            moveSpeed = -1;
            playerRenderer.flipX = true;
            if (!playerAudio.isPlaying)
            {
                playerAudio.clip = walkClip;
                playerAudio.Play();
            }
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            moveSpeed = 1;
            playerRenderer.flipX = false;
            if (!playerAudio.isPlaying)
            {
                playerAudio.clip = walkClip;
                playerAudio.Play();
            }
        }
        else 
        {
            moveSpeed = 0;
            if (!playerAudio.isPlaying)
            {
                playerAudio.Stop();
            }
        }
        playerAnimator.SetInteger("moveSpeed", moveSpeed);
        playerRigidbody.velocity = new Vector2(moveSpeed, playerRigidbody.velocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.X)&&isGrounded)
        {
            moveSpeed = 0;
            playerAnimator.SetTrigger("Jump");
            playerRigidbody.velocity = new Vector2(moveSpeed, playerRigidbody.velocity.y);
            playerRigidbody.AddForce(new Vector2(moveSpeed, jumpForce),ForceMode2D.Impulse);
        }
    }

    void Attack()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            StartCoroutine("AttackCoroutine");
        }
    }

    void BeHitted()
    {
        StopAllCoroutines();
        StartCoroutine("BeHittedCoroutine");
    }

    void Dead()
    {
        state = State.working;
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.bodyType = RigidbodyType2D.Static;
        playerAnimator.SetTrigger("Dead");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.8f)
        {
            isGrounded = true;
            playerAnimator.SetBool("isGrounded", isGrounded);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    IEnumerator AttackCoroutine()
    {
        playerAnimator.SetTrigger("Attack");
        state = State.working;
        SendMessage("TurnOnAttackRange", playerRenderer.flipX);
        if (playerAudio.isPlaying)
        {
            playerAudio.Stop();
            playerAudio.clip = attackClip;
            playerAudio.Play();
        }


        yield return new WaitForSeconds(0.5f);
        SendMessage("TurnOffAttackRange");
        state = State.idle;
    }

    IEnumerator BeHittedCoroutine()
    {
        playerAnimator.SetTrigger("beHitted");
        state = State.working;
        playerRigidbody.AddForce(new Vector2(-moveSpeed, 1) * 2, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.3f);

        state = State.idle;
    }
}
