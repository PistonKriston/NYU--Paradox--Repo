using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
     Rigidbody2D rb2d;
    float horizontalInput;

    public float moveSpeed = 10f;
    public float jumpSpeed = 5f;

    public Transform GroundCheckPoint; 
    public Transform WallCheckPointRight;
    public Transform WallCheckPointLeft;
    public LayerMask GroundLayer;
    public float GroundCheckRadius = 0.2f;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

     void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        float nextVelocityX = horizontalInput * moveSpeed;
        float nextVelocityY = rb2d.velocity.y;
        
        
        rb2d.velocity = new Vector2(nextVelocityX, nextVelocityY);
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3( -1, 1, 1);
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3( 1, 1, 1);
        }
        
       
    }
}
