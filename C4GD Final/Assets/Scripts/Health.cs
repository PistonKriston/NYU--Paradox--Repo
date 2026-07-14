using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHP = 10f;
    public float currentHP = 10f;
    public GameObject DeathScreen;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float amt)
    {
        currentHP -= amt;

        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
