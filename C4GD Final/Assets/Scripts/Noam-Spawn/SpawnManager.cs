using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject spawner;
    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (spawner.gameObject.tag == ("Spawner") && collision.gameObject.tag == ("Enemy"))
        {
            enemyPrefab.SetActive(true);
        }
    }

}
