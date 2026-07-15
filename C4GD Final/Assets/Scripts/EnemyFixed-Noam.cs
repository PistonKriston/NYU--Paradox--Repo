using UnityEngine;
using UnityEngine.UI;

public class EnemyFixed1 : MonoBehaviour
{
    [Header("Movement")]
    public bool flying = true;
    public float speed = 5f;
    public float stopDistance = 0.75f;

    [Header("Attack")]
    public float attackRange = 2.5f;   // bigger for testing
    public float damage = 1f;

    [Header("References")]
    public GameObject player;
    public Image healthBar;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isAttacking = false;
    private float facing = 1f;
    private Vector2 desiredVelocity = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("EnemyFixed1: PLAYER IS NULL. Assign it in Inspector.");
        }
        else
        {
            Debug.Log("EnemyFixed1: Player assigned = " + player.name);
        }
    }

    private void Update()
    {
        if (player == null) return;

        HandleMovement();
        HandleAttackLoop();
    }

    private void FixedUpdate()
    {
        rb.velocity = desiredVelocity;
    }

    private void HandleMovement()
    {
        Vector2 toPlayer = player.transform.position - transform.position;
        float dist = toPlayer.magnitude;
        Vector2 dir = toPlayer.normalized;

        facing = player.transform.position.x > transform.position.x ? 1f : -1f;
        transform.localScale = new Vector3(facing, 1f, 1f);

        if (dist <= stopDistance)
            desiredVelocity = Vector2.zero;
        else
            desiredVelocity = flying ? dir * speed : new Vector2(facing * speed, rb.velocity.y);
    }

    private void HandleAttackLoop()
    {
        if (isAttacking) return;

        float dist = Vector2.Distance(player.transform.position, transform.position);

        if (dist <= attackRange)
        {
            Debug.Log("EnemyFixed1: Triggering attack. dist = " + dist);
            animator.SetTrigger("attack");
            isAttacking = true;
        }
    }

    // CALLED BY ANIMATION EVENT
    public void BiteHit()
    {
        Debug.Log("EnemyFixed1: BiteHit fired.");

        if (player == null)
        {
            Debug.LogError("EnemyFixed1: BiteHit FAILED — player is NULL.");
            return;
        }

        float dist = Vector2.Distance(player.transform.position, transform.position);
        Debug.Log("EnemyFixed1: BiteHit distance = " + dist);

        if (dist > attackRange)
        {
            Debug.Log("EnemyFixed1: BiteHit — player OUT OF RANGE.");
            return;
        }

        var h = player.GetComponent<Health>();
        if (h == null)
        {
            Debug.LogError("EnemyFixed1: BiteHit FAILED — Player HAS NO Health component.");
            return;
        }

        Debug.Log("EnemyFixed1: BiteHit SUCCESS — dealing damage " + damage);
        h.TakeDamage(damage);

        if (healthBar != null)
            healthBar.fillAmount -= damage / 10f;
    }

    // CALLED BY ANIMATION EVENT
    public void BiteEnd()
    {
        Debug.Log("EnemyFixed1: BiteEnd — attack unlocked.");
        isAttacking = false;
    }
}
