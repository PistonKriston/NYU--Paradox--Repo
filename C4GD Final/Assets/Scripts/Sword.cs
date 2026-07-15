using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float damage = 1;
    public Rigidbody2D enemyRB;
    private readonly HashSet<Health> hitTargets = new HashSet<Health>();
    public float force = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 diagonalForce = new Vector2(1, 1).normalized * force;
       
        Health health = collision.GetComponent<Health>();
        if (health == null)
            health = collision.GetComponentInParent<Health>();
           

        if (health == null || !health.CompareTag("Enemy"))
            return;

        if (hitTargets.Contains(health))
            return;

        hitTargets.Add(health);
        Debug.Log("Sword hit: " + health.name);
        health.TakeDamage(damage);
        enemyRB = collision.gameObject.GetComponent<Rigidbody2D>();
        
    }
}
