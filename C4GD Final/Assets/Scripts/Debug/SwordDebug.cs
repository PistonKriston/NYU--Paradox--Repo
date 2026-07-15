// SwordDebug.cs
// Debug variant of Sword: logs every trigger enter and the Health target it hits.

using System.Collections.Generic;
using UnityEngine;

public class SwordDebug : MonoBehaviour
{
    public float damage = 1f;
    private readonly HashSet<Health> hitTargets = new HashSet<Health>();

    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
            Debug.Log("SwordDebug: Collider was not trigger; set to trigger for sword prefab.");
        }
    }

    private void OnEnable()
    {
        hitTargets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        Debug.Log($"SwordDebug: OnTriggerEnter2D with '{collision.gameObject.name}' (tag='{collision.gameObject.tag}')");

        Health health = collision.GetComponent<Health>() ?? collision.GetComponentInParent<Health>();
        if (health == null)
        {
            Debug.Log("SwordDebug: No Health component found on collider or parent.");
            return;
        }

        if (!health.CompareTag("Enemy"))
        {
            Debug.Log($"SwordDebug: Target '{health.gameObject.name}' is not tagged 'Enemy' (tag='{health.gameObject.tag}'). Ignoring.");
            return;
        }

        if (hitTargets.Contains(health))
        {
            Debug.Log($"SwordDebug: Already hit '{health.gameObject.name}' with this swing. Ignoring duplicate.");
            return;
        }

        hitTargets.Add(health);
        Debug.Log($"SwordDebug: Hitting '{health.gameObject.name}' for {damage} damage.");
        health.TakeDamage(damage);
    }

    private void OnDisable()
    {
        hitTargets.Clear();
    }

    private void OnDestroy()
    {
        hitTargets.Clear();
    }
}
