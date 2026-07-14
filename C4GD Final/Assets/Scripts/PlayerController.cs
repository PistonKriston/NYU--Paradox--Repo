using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Rigidbody2D rb;
    public float moveSpeed = 10f;
    public float jumpSpeed = 5f;
    // Awake is called when the object loads
    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float direction = Input.GetAxis("Horizontal");
        float VelocityX = direction * moveSpeed;
        float VelocityY = rb.velocity.y;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            VelocityY = jumpSpeed;
        }
        rb.velocity = new Vector2(VelocityX, VelocityY);
    }
}
