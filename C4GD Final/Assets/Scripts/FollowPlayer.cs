using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public float lookAhead = 10f;
    public float lookAheadMaxDistanceDelta = 5f;
    void Start()
    {
        transform.position = new Vector3(PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (PlayerController.instance != null)
        {
            Vector3 targetPosition = new Vector3(PlayerController.instance.transform.position.x  + (lookAhead * PlayerController.instance.transform.localScale.x), 
                PlayerController.instance.transform.position.y, 
                transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, lookAheadMaxDistanceDelta * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, PlayerController.instance.transform.position.y, transform.position.z);
        }
    }
}
