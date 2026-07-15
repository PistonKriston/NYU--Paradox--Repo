// EnemyFinal.cs
// Minimal, stable enemy: triggers attack when player is in range (but not when overlapping/touching).
// Damage is applied only in BiteHit (animation event) and only after a short windup time.
// This file intentionally keeps the original name and modifies only the enemy behavior.

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyFinalDebug : MonoBehaviour
{
    [Header("Movement")]
    public bool flying = true;
    public float speed = 3f;
    public float stopDistance = 0.75f;

    [Header("Attack")]
    public float attackRange = 1.5f;
    public float damage = 1f;

    [Header("Attack Timing")]
    [Tooltip("Minimum seconds after attack trigger before BiteHit can apply damage (defensive windup).")]
    public float minHitDelay = 0.15f;

    [Header("Animator Settings")]
    [Tooltip("If true, the attack uses a Trigger parameter. If false, uses a Bool parameter.")]
    public bool attackUsesTrigger = true;
    public string attackTriggerName = "attack";
    public string attackBoolName = "attack";
    public string idleStateName = "Idle";

    [Header("References")]
    public GameObject player;
    public Image healthBar;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isAttacking = false;
    private float attackStartTime = -999f;
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
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                Debug.LogWarning("EnemyFinal: Player not assigned and no GameObject with tag 'Player' found.");
        }

        // Safety: ensure attackRange is not smaller than stopDistance
        if (attackRange <= stopDistance)
        {
            attackRange = stopDistance + 0.1f;
            Debug.Log($"EnemyFinal: Adjusted attackRange to {attackRange:F2} to be larger than stopDistance.");
        }
    }

    private void Update()
    {
        if (player == null) return;

        HandleMovement();
        HandleAttackCheck();
    }

    private void FixedUpdate()
    {
        rb.velocity = desiredVelocity;
    }

    private void HandleMovement()
    {
        Vector2 toPlayer = (player.transform.position - transform.position);
        float dist = toPlayer.magnitude;
        Vector2 dir = toPlayer.normalized;

        // Flip sprite to face player
        facing = player.transform.position.x > transform.position.x ? 1f : -1f;
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

        float dist = Vector2.Distance(player.transform.position, transform.position);

        // Prevent starting an attack if the player is overlapping/touching (dist <= stopDistance)
        if (dist <= stopDistance)
        {
            // Player is too close/overlapping; do not start attack here.
            return;
        }

        if (dist <= attackRange)
        {
            // Start attack: set animator and record start time for windup check
            Debug.Log($"EnemyFinal: Triggering attack (enemy={gameObject.name}) dist={dist:F2}");
            if (attackUsesTrigger)
                animator.SetTrigger(attackTriggerName);
            else
                animator.SetBool(attackBoolName, true);

            isAttacking = true;
            attackStartTime = Time.time;
        }
    }

    // CALLED BY ANIMATION EVENT at the frame where the bite should deal damage
    public void BiteHit()
    {
        Debug.Log($"EnemyFinal: BiteHit called on '{gameObject.name}' at frame {Time.frameCount}");

        if (player == null)
        {
            Debug.LogError("EnemyFinal: BiteHit FAILED — player is NULL.");
            return;
        }

        // Defensive checks: require isAttacking and minHitDelay elapsed
        if (!isAttacking)
        {
            Debug.Log("EnemyFinal: BiteHit ignored because isAttacking == false.");
            return;
        }

        if (Time.time < attackStartTime + minHitDelay)
        {
            Debug.Log($"EnemyFinal: BiteHit ignored because windup not finished (elapsed {Time.time - attackStartTime:F2}s).");
            return;
        }

        // Re-check distance before applying damage
        float dist = Vector2.Distance(player.transform.position, transform.position);
        Debug.Log($"EnemyFinal: BiteHit distance = {dist:F2}");

        if (dist > attackRange)
        {
            Debug.Log("EnemyFinal: BiteHit — player OUT OF RANGE.");
            return;
        }

        var h = player.GetComponent<Health>();
        if (h == null)
        {
            Debug.LogError("EnemyFinal: BiteHit FAILED — Player HAS NO Health component.");
            return;
        }

        Debug.Log($"EnemyFinal: BiteHit SUCCESS — dealing damage {damage}");
        h.TakeDamage(damage);

        if (healthBar != null)
            healthBar.fillAmount = Mathf.Clamp01(healthBar.fillAmount - damage / 10f);
    }

    // CALLED BY ANIMATION EVENT at the end of the attack animation
    public void BiteEnd()
    {
        Debug.Log($"EnemyFinal: BiteEnd called on '{gameObject.name}' at frame {Time.frameCount}");
        isAttacking = false;

        // Reset animator parameters so it can return to Idle
        if (attackUsesTrigger)
            animator.ResetTrigger(attackTriggerName);
        else
            animator.SetBool(attackBoolName, false);

        // Force Idle state to ensure visual return to idle
        if (!string.IsNullOrEmpty(idleStateName))
        {
            animator.Play(idleStateName, 0, 0f);
        }
    }

    // Debug hooks: log collisions/triggers so we can find any legacy collision-based damage callers
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"EnemyFinal: OnTriggerEnter2D with '{other.gameObject.name}' (tag='{other.gameObject.tag}') at frame {Time.frameCount}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"EnemyFinal: OnCollisionEnter2D with '{collision.gameObject.name}' (tag='{collision.gameObject.tag}') at frame {Time.frameCount}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
