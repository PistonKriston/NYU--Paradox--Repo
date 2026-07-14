using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public GameObject sword;
    public GameObject player;
    Vector2 playerPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerPosition = new Vector2(player.transform.position.x + 1, player.transform.position.y);
            GameObject s = Instantiate(sword, player.transform);
            /*
            if (PlayerController.instance.facingRight)
            {
                s.transform.localPosition = new Vector3(1, 0, 0);

            }
            */
        }
    }
}
