// EnemyFinal.cs
// Simple, robust 2D enemy controller: chases the player, attacks via animation events,
// and only deals damage when BiteHit is called by the attack animation.
// No collision/trigger code in this script applies damage.

using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyEvenMoreFinal : MonoBehaviour
{
    [Header("Movement")]
    public bool flying = true;
    public float speed = 3f;
    public float stopDistance = 0.75f;

    [Header("Attack")]
    public float attackRange = 1.5f;
    public float damage = 1f;

    [Header("Animator Settings")]
    [Tooltip("If true, the attack uses a Trigger parameter. If false, uses a Bool parameter.")]
    public bool attackUsesTrigger = true;
    public string attackTriggerName = "attack";
    public string attackBoolName = "attack";
    public string idleStateName = "Idle";

    [Header("References")]
    public Image healthBar;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isAttacking = false;
    [HideInInspector] public float facing = 1f;
    private Vector2 desiredVelocity = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (PlayerController.instance == null)
        {
            Debug.LogWarning("EnemyEvenMoreFinal: PlayerController does not exist right now.");
        }

        //For saving purposes
        if (GameManager.instance.enemyPositions != null)
        {
            foreach ((int, Vector3) t in GameManager.instance.enemyPositions)
            {
                if (t.Item1 == gameObject.GetInstanceID())
                {
                    print("Does this work");
                    transform.position = t.Item2;
                }
            }
        }
    }

    private void Update()
    {
        if (PlayerController.instance == null) return;

        HandleMovement();
        HandleAttackCheck();
        SyncAnimatorMovement();
    }

    private void FixedUpdate()
    {
        // Apply velocity in FixedUpdate for stable physics
        rb.velocity = desiredVelocity;
    }

    private void HandleMovement()
    {
        Vector2 toPlayer = (PlayerController.instance.transform.position - transform.position);
        float dist = toPlayer.magnitude;
        Vector2 dir = toPlayer.normalized;

        // Flip sprite to face player
        if (gameObject.GetComponent<EnemyTurnAround>().flip_cooldown <= 0)
        {
            facing = PlayerController.instance.transform.position.x > transform.position.x ? 1f : -1f;
        }
        transform.localScale = new Vector3(facing, 1f, 1f);

        // Stop when close enough
        if (dist <= stopDistance)
        {
            desiredVelocity = Vector2.zero;
            return;
        }

        // Movement modes
        if (flying)
        {
            desiredVelocity = dir * speed;
        }
        else
        {
            // Grounded: only horizontal movement, preserve current vertical velocity
            desiredVelocity = new Vector2(facing * speed, rb.velocity.y);
        }
    }

    private void HandleAttackCheck()
    {
        if (isAttacking) return;

        float dist = Vector2.Distance(PlayerController.instance.transform.position, transform.position);

        if (dist <= attackRange)
        {
            // Trigger the attack in Animator
            if (attackUsesTrigger)
            {
                animator.SetTrigger(attackTriggerName);
            }
            else
            {
                animator.SetBool(attackBoolName, true);
            }

            isAttacking = true;
        }
    }

    // Optional: keep animator parameters in sync with movement (for blend trees or movement animations)
    private void SyncAnimatorMovement()
    {
        // Example: set horizontal speed parameter if present
        // animator.SetFloat("speed", Mathf.Abs(desiredVelocity.x) + Mathf.Abs(desiredVelocity.y));
        // Keep this method minimal; uncomment and adapt if your Animator uses movement parameters.
    }

    // IMPORTANT: No OnTriggerEnter/OnCollisionEnter code applies damage here.
    // Collisions should not call TakeDamage. This script applies damage only in BiteHit().

    // CALLED BY ANIMATION EVENT at the frame where the bite should deal damage
    public void BiteHit()
    {
        if (PlayerController.instance == null) return;

        float dist = Vector2.Distance(PlayerController.instance.transform.position, transform.position);
        if (dist > attackRange) return;

        Health h = PlayerController.instance.GetComponent<Health>();
        if (h != null)
        {
            h.TakeDamage(damage);

            if (healthBar != null)
            {
                // Assume healthBar.fillAmount is 0..1 and damage is scaled to 10 health units as in original code.
                float delta = damage / 10f;
                healthBar.fillAmount = Mathf.Clamp01(healthBar.fillAmount - delta);
            }
        }
    }

    // CALLED BY ANIMATION EVENT at the end of the attack animation
    public void BiteEnd()
    {
        // Unlock attack state
        isAttacking = false;

        // Reset Animator parameters so it can return to Idle
        if (attackUsesTrigger)
        {
            // Reset the trigger to avoid re-entering attack unexpectedly
            animator.ResetTrigger(attackTriggerName);
        }
        else
        {
            // If using a bool, explicitly set it false
            animator.SetBool(attackBoolName, false);
        }

        // Force the Animator to play Idle if that state exists.
        if (!string.IsNullOrEmpty(idleStateName))
        {
            animator.Play(idleStateName, 0, 0f);
        }
    }

    // Optional debug helper to visualize attack range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}

