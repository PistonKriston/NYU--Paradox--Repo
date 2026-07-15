// HealthDebug.cs
// Replaces Health for debugging. Logs every TakeDamage call with a short stack trace.
// Use this temporarily to find who is calling TakeDamage.

using UnityEngine;

public class HealthDebug : MonoBehaviour
{
    public float maxHP = 10f;
    public float currentHP = 10f;

    [Tooltip("Enable verbose stack traces (may be noisy).")]
    public bool verboseStack = false;

    [Tooltip("If true, logs will include frame count and time.")]
    public bool includeTime = true;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        // debug: press R to kill self
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(currentHP);
        }
    }

    public void TakeDamage(float amt)
    {
        if (includeTime)
            Debug.Log($"HealthDebug: TakeDamage called on '{gameObject.name}' (tag='{gameObject.tag}') amount={amt} currentHP(before)={currentHP} frame={Time.frameCount} time={Time.time:F2}");

        // Print a short stack trace to identify the caller
        string stack = UnityEngine.StackTraceUtility.ExtractStackTrace();
        if (!string.IsNullOrEmpty(stack))
        {
            // Optionally trim the stack to the first few lines to reduce noise
            string[] lines = stack.Split('\n');
            int maxLines = verboseStack ? lines.Length : Mathf.Min(6, lines.Length);
            string shortStack = string.Join("\n", lines, 0, maxLines);
            Debug.Log($"HealthDebug: StackTrace (top frames):\n{shortStack}");
        }

        currentHP -= amt;

        if (currentHP <= 0f)
        {
            Debug.Log($"HealthDebug: '{gameObject.name}' died.");
            Destroy(gameObject);
        }
    }
}
