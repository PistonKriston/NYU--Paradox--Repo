using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public float lookAhead = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (PlayerController.instance != null)
        {
            transform.position = new Vector3(PlayerController.instance.transform.position.x  + lookAhead, 
                PlayerController.instance.transform.position.y, 
                transform.position.z);
        }
    }
}
