using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_Platform : MonoBehaviour
{
    public Transform waypoint1;
    public Transform waypoint2;

    public int target;

    public float distancex;
    public float distancey;

    public float switchtime = 1.5f;
    public float switch_reset = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        target = 1;
        distancex = (waypoint1.transform.position.x - waypoint2.transform.position.x);
        if (distancex < 0)
        {
            distancex *= -1;
        }
        distancey = (waypoint1.transform.position.y - waypoint2.transform.position.y);
        if (distancey < 0)
        {
            distancey *= -1;
        }

        transform.position = transform.position = new Vector2(waypoint2.transform.position.x, waypoint2.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (switchtime >= switch_reset)
        {
            switchtime = 0;
            if (target == 1)
            {
                target = 2;
                transform.position = new Vector2(waypoint1.transform.position.x, waypoint1.transform.position.y);
            }
            else
            {
                target = 1;
                transform.position = new Vector2(waypoint2.transform.position.x, waypoint2.transform.position.y);
            }
        }
        if (target == 1){
            transform.position= new Vector2(waypoint2.transform.position.x - (distancex*(switchtime/switch_reset)), waypoint2.transform.position.y - (distancey * (switchtime / switch_reset)));
            switchtime += Time.deltaTime;
        }
        if (target == 2)
        {
            transform.position = new Vector2(waypoint1.transform.position.x + (distancex * (switchtime / switch_reset)), waypoint1.transform.position.y + (distancey * (switchtime / switch_reset)));
            switchtime += Time.deltaTime;
        }
    }
}
