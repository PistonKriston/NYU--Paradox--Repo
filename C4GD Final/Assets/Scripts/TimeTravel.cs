using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravel : MonoBehaviour
{
    
    public bool inPast;
    public bool isGroundedEnemy;
    public float distanceBetweenLevels = 100f;
    public LayerMask bothLayer;
    private float groundCheckRadius = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        inPast = GameManager.instance.playerInPast;
        if (inPast)
        {
            if (isGroundedEnemy)
            {
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
            else
            {
                transform.Translate(Vector3.up * distanceBetweenLevels);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool isGrounded = checkForGround();
            if (!isGrounded)
            {
                if (inPast)
                {
                    if (isGroundedEnemy)
                    {
                        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    }
                    else
                    {
                        transform.Translate(Vector3.down * distanceBetweenLevels);
                    }
                }
                else
                {
                    if (isGroundedEnemy)
                    {
                        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    }
                    else
                    {

                        if (!isGrounded)
                        {
                            transform.Translate(Vector3.up * distanceBetweenLevels);
                        }
                    }
                }
                inPast = !inPast;
            }
        }

        if (isGroundedEnemy && inPast)
        {
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }

    }

    private bool checkForGround()
    {
        if (inPast)
        {
            return Physics2D.OverlapCircle((PlayerController.instance.transform.position + Vector3.down * distanceBetweenLevels), groundCheckRadius, bothLayer);
        }
        else
        {
            return Physics2D.OverlapCircle((PlayerController.instance.transform.position + Vector3.up * distanceBetweenLevels), groundCheckRadius, bothLayer);
        }
    }
}
