using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Animator animator;
    public float speed = 1f;

    private bool attacking = false;
    [SerializeField] private float attackTimer;
    public float attackDuration = 2f;

    public bool flying;

    private float VelocityX;
    private float VelocityY;


    public float damage = 1;
    Vector2 direction;
    public GameObject collisionObject;
    public bool damaging = false;
    public float damageTimer;
    public GameObject player;
    public Image healthBar;
    void Start()
    {
        for (int i = 0; i < GameManager.instance.enemyPositions.Count; i++)
        {
            print("bugged line of code");
        }
        GetComponent<TimeTravel>().inPast = GameManager.instance.playerInPast;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    public float facing = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackTimer = attackDuration;

    }

    // Update is called once per frame
    private void Update()
    {
        FollowPlayer();
        AttackTimer();

        if (damaging == true)
        {
            if (damageTimer <= 0)
            {
                healthBar.fillAmount -= damage / 10;
                collisionObject.GetComponent<Health>().TakeDamage(damage);
                damageTimer = 1f;

            }

            if (damageTimer > 0)
            {
                damageTimer -= Time.deltaTime;
            }
            else
            {
                damageTimer = 0;
            }
        }


    }

    private void FixedUpdate()
    {
        FollowPlayer();
        if (flying == true)
        {
            if (facing == 1)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                rb.velocity = new Vector2(direction.x, VelocityY);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                rb.velocity = new Vector2(direction.x, VelocityY);
            }
        }
        else
        {
            if (facing == -1)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                rb.velocity = new Vector2(-speed, 0);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                rb.velocity = new Vector2(speed, 0);
            }
        }
        
        Debug.Log(facing);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!attacking)
            {
                attacking = true;
                attackTimer = -1;
                animator.SetBool("attack", true);
                collisionObject = collision.gameObject;
                damaging = true;
                damageTimer = 1f;
                /*
                if (attacking = true)
                {
                    InvokeRepeating("Attack", 1f, 1f);
                }
               */

            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            attackTimer = 0;
            damaging = false;

        }
    }

    private void FollowPlayer()
    {
        direction = (PlayerController.instance.transform.position - transform.position).normalized;
        VelocityX = 0;

        if (flying != true)
        {
            VelocityX = speed * facing;
            if (attacking)
            {
                VelocityY = 0;
            }
            direction = Vector3.zero;


        }
        else
        {
            if (!attacking)
            {
                VelocityX = direction.x * speed;
            }
            VelocityY = rb.velocity.y;
            if (attacking)
            {
                VelocityY = 0;
            }
            else if (flying)
            {
                VelocityY = direction.y * speed;
            }
        }

    }

    private void AttackTimer()
    {
        if (attackTimer >= 0 && attackTimer < attackDuration)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDuration)
            {
                attacking = false;
                animator.SetBool("attack", false);
            }
        }
    }
    public void Attack()
    {

        collisionObject.GetComponent<Health>().TakeDamage(damage);
    }
}