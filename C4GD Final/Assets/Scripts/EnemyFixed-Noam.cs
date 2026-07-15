using UnityEngine;
using UnityEngine.UI;

public class EnemyFixed1 : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public float stopDistance = 1.2f;
    public bool flying = false;

    [Header("Attack")]
    public float attackRange = 1.5f;
    public float attackDuration = 1f;   // length of attack animation
    public float attackCooldown = 0.5f;
    public float damage = 1f;

    [Header("References")]
    public GameObject player;
    public Image healthBar;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isAttacking = false;
    private bool canAttack = true;
    private float facing = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (player == null && PlayerController.instance != null)
            player = PlayerController.instance.gameObject;
    }

    private void Update()
    {
        if (player == null) return;

        HandleMovementAndAttack();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    // ---------------- MOVEMENT + ATTACK DECISION ----------------

    private Vector2 desiredVelocity = Vector2.zero;

    private void HandleMovementAndAttack()
    {
        Vector2 toPlayer = player.transform.position - transform.position;
        float dist = toPlayer.magnitude;
        Vector2 dir = toPlayer.normalized;

        // Update facing
        facing = player.transform.position.x > transform.position.x ? 1f : -1f;
        transform.localScale = new Vector3(facing, 1f, 1f);

        if (isAttacking)
        {
            desiredVelocity = Vector2.zero;
            return;
        }

        // If close enough to stop, don't push the player
        if (dist <= stopDistance)
        {
            desiredVelocity = Vector2.zero;
        }
        else
        {
            if (flying)
            {
                desiredVelocity = dir * speed;
            }
            else
            {
                desiredVelocity = new Vector2(facing * speed, rb.velocity.y);
            }
        }

        // If within attack range and allowed to attack, start attack
        if (dist <= attackRange && canAttack && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private void ApplyMovement()
    {
        rb.velocity = desiredVelocity;
    }

    // ---------------- ATTACK ROUTINE ----------------

    private System.Collections.IEnumerator AttackRoutine()
    {
        isAttacking = true;
        canAttack = false;

        // Play attack animation
        animator.SetBool("attack", true);

        // Wait for animation duration
        float timer = 0f;
        while (timer < attackDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Stop animation
        animator.SetBool("attack", false);

        // Deal damage once
        if (player != null)
        {
            Health h = player.GetComponent<Health>();
            if (h != null)
            {
                h.TakeDamage(damage);
            }

            if (healthBar != null)
            {
                healthBar.fillAmount -= damage / 10f;
            }
        }

        // Small cooldown before next attack
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        canAttack = true;
    }
}
