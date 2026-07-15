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
        if (Input.GetKey("w") && Input.GetMouseButtonDown(0))
            {
                print("Help");
                playerPosition = new Vector2(player.transform.position.x, player.transform.position.y + 1.5f);
                print(playerPosition);
                GameObject s = Instantiate(sword, playerPosition, Quaternion.Euler(0f, 0f, 90f));
                //s.transform.position.x = player.transform.position.x;
               //s.transform.localPosition = new Vector3(0,2,0);
               s.GetComponent<Sword>().direction = new Vector2(0,1);
               s.GetComponent<Sword>().player = gameObject;
               s.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
               GameObject.Destroy(s,0.325f);
            }
        else if (Input.GetMouseButtonDown(0))
        {
            playerPosition = new Vector2(player.transform.position.x + 1, player.transform.position.y);
            GameObject s = Instantiate(sword, playerPosition, transform.rotation);
            
            if (PlayerController.instance.facingRight == true)
            {
                //s.transform.localPosition = new Vector3(1, 0, 0);
                //StartCoroutine(Wait(s,0.325f));
                s.GetComponent<Sword>().direction = new Vector2(1,0);
                 s.GetComponent<Sword>().player = gameObject;
            }
            if (PlayerController.instance.facingRight == false)
            {
                //s.transform.localPosition = new Vector3(1, 0, 0);
                s.GetComponent<Sword>().direction = new Vector2(-1,0);
                 s.GetComponent<Sword>().player = gameObject;
                 s.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            
            GameObject.Destroy(s,0.325f);
        }
       
    }
    IEnumerator Wait(GameObject toDestroy, float time)
        {
        
        yield return new WaitForSeconds(time); // wait 3 seconds
        GameObject.Destroy(toDestroy);
        
        }
}
