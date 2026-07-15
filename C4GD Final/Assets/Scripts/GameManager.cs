using System;
using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Vector3 playerPosition = Vector3.zero;
    public ArrayList enemyPositions = new ArrayList();
    public bool playerInPast = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /*
    private void Start()
    {
        SaveGame();
    }
    */

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    public void SaveGame()
    {
        playerPosition = PlayerController.instance.transform.position;
        playerInPast = PlayerController.instance.GetComponent<TimeTravel>().inPast;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemyPositions.Count <= i)
            {
                enemyPositions.Add(Vector3.zero);
            }
            enemyPositions[i] = (enemies[i].GetComponent<EnemyEvenMoreFinal>().ID, enemies[i].transform.position);
        }
    }
    */

    public void Load()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
