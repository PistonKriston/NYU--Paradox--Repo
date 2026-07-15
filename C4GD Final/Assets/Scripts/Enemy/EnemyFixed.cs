using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFixed : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    public float speed = 1f;
    public float attackDuration = 2f;
    public float damage = 1f;

    private float attackTimer = 0f;
    private bool attacking = false;

    public bool flying = false;
    public float facing = 1;

    private Vector2 direction;

    public GameObject collisionObject;
    public bool damaging = false;
    public float damageTimer = 0f;

    public Image healthBar;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateFacing();
        AttackTimer();
        DamageTick();
    }

    private void FixedUpdate()
    {
        FollowPlayer();
        MoveEnemy();
    }

    private void UpdateFacing()
    {
        float playerX = PlayerController.instance.transform.position.x;

        if (playerX > transform.position.x)
            facing = 1;
        else
            facing = -1;
    }

    private void FollowPlayer()
    {
        direction = (PlayerController.instance.transform.position - transform.position).normalized;
    }

    private void MoveEnemy()
    {
        if (flying)
        {
            Vector2 vel = direction * speed;

            if (attacking)
                vel = Vector2.zero;

            rb.velocity = vel;

            transform.localScale = new Vector3(facing, 1, 1);
        }
        else
        {
            float xVel = facing * speed;

            if (attacking)
                xVel = 0;

            rb.velocity = new Vector2(xVel, rb.velocity.y);

            transform.localScale = new Vector3(facing, 1, 1);
        }
    }

    private void AttackTimer()
    {
        if (attacking)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDuration)
            {
                attacking = false;
                damaging = false;
                animator.SetBool("attack", false);
            }
        }
    }

    private void DamageTick()
    {
        if (!damaging) return;

        damageTimer -= Time.deltaTime;

        if (damageTimer <= 0)
        {
            collisionObject.GetComponent<Health>().TakeDamage(damage);
            healthBar.fillAmount -= damage / 10f;
            damageTimer = 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        attacking = true;
        damaging = true;
        attackTimer = 0f;
        damageTimer = 1f;

        animator.SetBool("attack", true);
        collisionObject = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        attacking = false;
        damaging = false;
        animator.SetBool("attack", false);
    }
}
