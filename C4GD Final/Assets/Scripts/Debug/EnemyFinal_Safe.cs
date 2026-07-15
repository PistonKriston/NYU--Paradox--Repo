// EnemyFinal_Safe.cs
// Updated enemy that only deals damage via BiteHit (animation event).
// Adds stricter attack gating: cooldown, facing angle, optional line-of-sight.
// Adds collision debug logs to help find legacy collision-based damage.
/*
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyFinal_Safe : MonoBehaviour
{
    [Header("Movement")]
    public bool flying = true;
    public float speed = 3f;
    public float stopDistance = 0.75f;

    [Header("Attack")]
    public float attackRange = 1.5f;
    public float damage = 1f;

    [Header("Attack gating")]
    [Tooltip("Minimum seconds between attack starts")]
    public float attackCooldown = 1.0f;
    [Tooltip("Angle (degrees) within which the enemy must be facing the player to start an attack")]
    [Range(0f, 180f)]
    public float attackAngle = 60f;
    [Tooltip("Require an unobstructed line of sight (raycast) to start an attack")]
    public bool requireLineOfSight = false;
    [Tooltip("LayerMask used for line-of-sight blocking (e.g., ground, walls)")]
    public LayerMask losBlockingMask;

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
    private float facing = 1f;
    private Vector2 desiredVelocity = Vector2.zero;

    // internal timers
    private float attackCooldownTimer = 0f;

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
                Debug.LogWarning("EnemyFinal_Safe: Player not assigned and no GameObject with tag 'Player' found.");
        }
    }

    private void Update()
    {
        if (player == null) return;

        // update timers
        if (attackCooldownTimer > 0f)
            attackCooldownTimer -= Time.deltaTime;

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
        if (attackCooldownTimer > 0f) return;

        float dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist > attackRange) return;

        // Facing check: require player to be roughly in front of enemy
        Vector2 toPlayer = (player.transform.position - transform.position).normalized;
        Vector2 forward = new Vector2(transform.localScale.x, 0f).normalized; // local facing direction
        float angle = Vector2.Angle(forward, toPlayer);
        if (angle > attackAngle * 0.5f)
        {
            // Not sufficiently facing the player
            Debug.Log($"EnemyFinal_Safe: Not attacking because angle {angle:F1} > {attackAngle*0.5f:F1}");
            return;
        }

        // Optional line-of-sight check
        if (requireLineOfSight)
        {
            Vector2 origin = (Vector2)transform.position;
            Vector2 target = (Vector2)player.transform.position;
            Vector2 dir = (target - origin).normalized;
            float distToPlayer = Vector2.Distance(origin, target);
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, distToPlayer, losBlockingMask);
            if (hit.collider != null)
            {
                Debug.Log($"EnemyFinal_Safe: LOS blocked by {hit.collider.gameObject.name}; not attacking.");
                return;
            }
        }

        // All checks passed: start attack
        Debug.Log($"EnemyFinal_Safe: Starting attack (enemy={gameObject.name}) dist={dist:F2} angle={angle:F1}");
        if (attackUsesTrigger)
            animator.SetTrigger(attackTriggerName);
        else
            animator.SetBool(attackBoolName, true);

        isAttacking = true;
        attackCooldownTimer = attackCooldown;
    }

    // CALLED BY ANIMATION EVENT at the frame where the bite should deal damage
    public void BiteHit()
    {
        Debug.Log($"EnemyFinal_Safe: BiteHit called on '{gameObject.name}' at frame {Time.frameCount}");

        if (player == null)
        {
            Debug.LogError("EnemyFinal_Safe: BiteHit FAILED — player is NULL.");
            return;
        }

        // Re-check distance and facing before applying damage
        float dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist > attackRange)
        {
            Debug.Log($"EnemyFinal_Safe: BiteHit — player OUT OF RANGE (dist={dist:F2}).");
            return;
        }

        Vector2 toPlayer = (player.transform.position - transform.position).normalized;
        Vector2 forward = new Vector2(transform.localScale.x, 0f).normalized;
        float angle = Vector2.Angle(forward, toPlayer);
        if (angle > attackAngle * 0.5f)
        {
            Debug.Log($"EnemyFinal_Safe: BiteHit — player not in attack angle (angle={angle:F1}).");
            return;
        }

        // Optional LOS re-check
        if (requireLineOfSight)
        {
            Vector2 origin = (Vector2)transform.position;
            Vector2 target = (Vector2)player.transform.position;
            Vector2 dir = (target - origin).normalized;
            float distToPlayer = Vector2.Distance(origin, target);
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, distToPlayer, losBlockingMask);
            if (hit.collider != null)
            {
                Debug.Log($"EnemyFinal_Safe: BiteHit — LOS blocked by {hit.collider.gameObject.name}; aborting damage.");
                return;
            }
        }

        // Apply damage only here
        var hd = player.GetComponent<HealthDebug>();
        if (hd != null)
        {
            Debug.Log($"EnemyFinal_Safe: BiteHit SUCCESS — dealing damage {damage} via HealthDebug");
            hd.TakeDamage(damage);
        }
        else
        {
            var h = player.GetComponent<Health>();
            if (h == null)
            {
                Debug.LogError("EnemyFinal_Safe: BiteHit FAILED — Player HAS NO Health component.");
                return;
            }

            Debug.Log($"EnemyFinal_Safe: BiteHit SUCCESS — dealing damage {damage} via Health");
            h.TakeDamage(damage);
        }

        if (healthBar != null)
            healthBar.fillAmount = Mathf.Clamp01(healthBar.fillAmount - damage / 10f);
    }

    // CALLED BY ANIMATION EVENT at the end of the attack animation
    public void BiteEnd()
    {
        Debug.Log($"EnemyFinal_Safe: BiteEnd called on '{gameObject.name}' at frame {Time.frameCount}");
        isAttacking = false;

        if (attackUsesTrigger)
            animator.ResetTrigger(attackTriggerName);
        else
            animator.SetBool(attackBoolName, false);

        if (!string.IsNullOrEmpty(idleStateName))
            animator.Play(idleStateName, 0, 0f);
    }

    // Debug hooks: log collisions/triggers so we can find any legacy collision-based damage callers
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"EnemyFinal_Safe: OnTriggerEnter2D with '{other.gameObject.name}' (tag='{other.gameObject.tag}') at frame {Time.frameCount}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"EnemyFinal_Safe: OnCollisionEnter2D with '{collision.gameObject.name}' (tag='{collision.gameObject.tag}') at frame {Time.frameCount}");
    }

    // Optional debug helper to visualize attack range and angle in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);

        // Draw attack cone
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        Vector3 forward = new Vector3(transform.localScale.x, 0f, 0f);
        float halfAngle = attackAngle * 0.5f;
        Quaternion leftRot = Quaternion.Euler(0, 0, halfAngle);
        Quaternion rightRot = Quaternion.Euler(0, 0, -halfAngle);
        Vector3 leftDir = leftRot * forward;
        Vector3 rightDir = rightRot * forward;
        Gizmos.DrawLine(transform.position, transform.position + leftDir.normalized * attackRange);
        Gizmos.DrawLine(transform.position, transform.position + rightDir.normalized * attackRange);
    }
}
*/