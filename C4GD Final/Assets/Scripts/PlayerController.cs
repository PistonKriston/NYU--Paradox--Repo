using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    public float playerinput;

    public float move_speed = 7f;
    public float jump_speed = 10f;
    public float max_speed = 6f;

    public Transform GroundCheckPoint;
    public Transform WallCheckPointLeft;
    public Transform WallCheckPointRight;
    public LayerMask GroundLayer;
    public LayerMask WallLayer;
    public float groundCheckRadius = 0.2f;

    public float no_input;
    private bool facingRight;

    public static PlayerController instance;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        playerinput = Input.GetAxis("Horizontal");
        float next_vel_x = 0;
        float next_vel_y = rb2d.velocity.y;
        bool isGrounded = CheckGrounded();
        bool isLeftWall = CheckWallLeft();
        bool isRightWall = CheckWallRight();
        if (rb2d.velocity.x > max_speed || rb2d.velocity.x < -max_speed)
        {
            next_vel_x = rb2d.velocity.x * 0.99f;
            if (isGrounded)
            {
                next_vel_x = rb2d.velocity.x * 0.90f;
            }
        }
        else
        { 
            if (no_input==0 && playerinput!=0)
            {
                if (playerinput >= 0)
                {
                    playerinput = 1;
                    next_vel_x = playerinput * move_speed;
                }
                    
                else
                {
                    playerinput = -1;
                    next_vel_x = playerinput * move_speed;
                }
                
            }
            else
            {
                next_vel_x = rb2d.velocity.x * 0.99f;
                if (isGrounded)
                {
                    next_vel_x = rb2d.velocity.x * 0.60f;
                }
            }
            
        }
        if (playerinput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        else if (playerinput >= 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }

        if (no_input <= 0)
        {
            no_input = 0;
        }
        else
        {
            no_input -= Time.deltaTime;
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            next_vel_y = jump_speed;
            Debug.Log("LEFT" + isLeftWall + " RIGHT" + isRightWall);
        }
        
            
        if (isRightWall && Input.GetKeyDown(KeyCode.Space))
        {
            next_vel_y = jump_speed * 0.4f;
            if (playerinput > 0)
            {
                rb2d.AddForce(transform.right * -600f);
            }
            else if (playerinput < 0)
            {
                rb2d.AddForce(transform.right * 600f);
            }
            no_input = 0.3f;
            Debug.Log("LEFT" + isLeftWall + " RIGHT" + isRightWall);
        }
            
        
        
        
        rb2d.velocity = new Vector2(next_vel_x, next_vel_y);
    }

    bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheckPoint.position, groundCheckRadius, GroundLayer);
    }
    bool CheckWallLeft()
    {
        return Physics2D.OverlapCircle(WallCheckPointLeft.position, groundCheckRadius, WallLayer);
    }

    bool CheckWallRight()
    {
        return Physics2D.OverlapCircle(WallCheckPointRight.position, groundCheckRadius, WallLayer);
    }
}
