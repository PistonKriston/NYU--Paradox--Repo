using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDamager : MonoBehaviour
{
    public float ignore_timer;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ignore_timer <= 0)
        {

            ignore_timer = 0;
            if (collision.gameObject.CompareTag("Player"))
            {
                Player.GetComponent<Health>().currentHP -= 5;
                ignore_timer = 0.5f;
            }
        }
        else
        {
            ignore_timer -= Time.deltaTime;
        }

    }
}
