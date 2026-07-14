using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravel : MonoBehaviour
{
    
    public bool inPast;
    public bool isEnemy;
    public float distanceBetweenLevels = 100f;
    // Start is called before the first frame update
    void Awake()
    {
        if (inPast)
        {
            transform.Translate(Vector3.up * distanceBetweenLevels);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inPast)
            {
                if (isEnemy)
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
                if (isEnemy)
                {
                    gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                }
                else
                {
                    transform.Translate(Vector3.up * distanceBetweenLevels);
                }
            }
            inPast = !inPast;
        }

        if (isEnemy && inPast)
        {
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }

    }

}
