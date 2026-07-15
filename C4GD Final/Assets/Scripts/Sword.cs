using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float damage = 1;
    public Rigidbody2D enemyRB;
    private readonly HashSet<Health> hitTargets = new HashSet<Health>();
    public float force = 10f;
    public EnemyEvenMoreFinal enemyFinal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyFinal = collision.gameObject.GetComponentInParent<EnemyEvenMoreFinal>();
       
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
        enemyFinal.Knockback();
        
    }
}
