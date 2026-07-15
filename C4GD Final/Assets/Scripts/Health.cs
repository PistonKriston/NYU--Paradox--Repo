using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHP = 10f;
    public float currentHP = 10f;
    public GameObject deathScreen;
    public Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(currentHP);
        }
        if (healthBar != null)
        {
             healthBar.fillAmount = currentHP / 10f;
        }
        
    }
    public void TakeDamage(float amt)
    {
        currentHP -= amt;

        if (currentHP <= 0)
        {
            Destroy(gameObject);
            if (gameObject.CompareTag("Player"))
            {
                deathScreen.SetActive(true);
            }
        }
    }
}
