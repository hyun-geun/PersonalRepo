using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : LifeEntity
{
    enum State { idle, working };
    Animator enemyAnimator;
    Rigidbody2D enemyRigidbody;
    CircleCollider2D enemyCollider;
    SpriteRenderer enemyRenderer;
    AudioSource enemyAudio;

    public Slider hpSlider;
    int movePattern;
    State state;
    void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<CircleCollider2D>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        enemyAudio = GetComponent<AudioSource>();

        offencePoint = 10;
        healthPoint = 50;
        isDead = false;

        movePattern = 0;
        state = State.idle;

        hpSlider.maxValue = healthPoint;
        hpSlider.value = healthPoint;
    }

    private void Start()
    {
        StartCoroutine("SetMovePatternCoroutine");
    }
    private void FixedUpdate()
    {
        if (state == State.idle)
        {
            RandomMove();
            DontFall();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine("ReactCoroutine");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && state == State.idle)
        {
            TracePlayer(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine("SetMovePatternCoroutine");
    }
    
    void RandomMove()
    {
        if (movePattern < 0)
        {
            enemyRenderer.flipX = true;
        }
        else
        {
            enemyRenderer.flipX = false;
        }
        enemyAnimator.SetInteger("movePattern", movePattern);
        enemyRigidbody.velocity = new Vector2(movePattern / 3.0f, enemyRigidbody.velocity.y);
    }

    void DontFall()
    {
        Vector2 frontPosition= new Vector2(enemyRigidbody.position.x + (0.1f * movePattern), enemyRigidbody.position.y);
        Debug.DrawRay(frontPosition, Vector2.down * 0.3f, new Color(0, 1, 0));
        RaycastHit2D fallSensor = Physics2D.Raycast(frontPosition, Vector3.down * 0.3f, 0.3f, LayerMask.GetMask("Foreground"));

        if (fallSensor.collider == null)
        {
            movePattern *= -1;
        }
    }

    void TracePlayer(Collider2D collision)
    {
        Vector2 traceVector = (collision.transform.position - this.transform.position).normalized;
        if (traceVector.x < 0)
        {
            movePattern = -2;
        }
        else
        {
            movePattern = 2;
        }
    }
    public override void OnDamage(int damage)
    {
        healthPoint -= damage;
        hpSlider.value = healthPoint;
        if (healthPoint > 0)
        {
            enemyAnimator.SetTrigger("Hit");
        }
        else
        {
            enemyAnimator.SetTrigger("Dead");
            OnDead();
        }
    }
    public override void OnDead()
    {
        GameManager.instance.AddScore(20);
        StartCoroutine("DestroyCoroutine");
    }
    IEnumerator SetMovePatternCoroutine()
    {
        while (!isDead)
        {
            movePattern = Random.Range(-1, 2);
            
            yield return new WaitForSeconds(5.0f);
        }
    }
    IEnumerator DestroyCoroutine()
    {
        state = State.working;
        enemyRigidbody.velocity = Vector2.zero;
        enemyRigidbody.bodyType = RigidbodyType2D.Static;
        isDead = true;
        enemyAudio.Play();
        enemyCollider.enabled = false;

        yield return new WaitForSeconds(1.5f);

        Destroy(this.gameObject);
    }
    IEnumerator ReactCoroutine()
    {
        state = State.working;
        enemyAnimator.SetTrigger("React");

        yield return new WaitForSeconds(0.9f);
        state = State.idle;
    }
}
