using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Collider2D c2d;
    private Animator animator;
    public float speed = 1f;

    private bool attacking = false;
    [SerializeField] private float attackTimer;
    public float attackDuration = 2f;

    public bool flying;

    private float VelocityX;
    private float VelocityY;
    public Vector2 direction;
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
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            
        }
        else if (direction.x >= 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
           
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(VelocityX, VelocityY);
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
            }
        }
        else if (collision.gameObject.layer == 3)
        {
            if (flying && !attacking)
            {
                c2d.isTrigger = true;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            attackTimer = 0;
            
        }
        else if (collision.gameObject.layer == 3)
        {
            c2d.isTrigger = false;
        }
    }

    private void FollowPlayer()
    {
        direction = (PlayerController.instance.transform.position - transform.position).normalized;
        VelocityX = 0;
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
     
}
