using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnAround : MonoBehaviour
{
    public Transform left_check_point;
    
    public LayerMask BothLayer;
    public float groundCheckRadius = 0.01f;

    public bool left_side;


    public GameObject Enemy;

    public float flip_cooldown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy.GetComponent<Enemy>().flying != true)
        {
            left_side = CheckLeft();

            if (flip_cooldown != 0)
            {
                flip_cooldown -= Time.deltaTime;
            }
            else
            {
                flip_cooldown = 0;
                if (left_side != true)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
                    Enemy.GetComponent<Enemy>().facing *= -1;
                    Debug.Log("The Enemy should have flipped");
                    flip_cooldown = 0.1f;
                }
            }
        }
        
        

    }

    
    bool CheckLeft()
    {
        if (Physics2D.OverlapCircle(left_check_point.position, groundCheckRadius, BothLayer))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    
}
