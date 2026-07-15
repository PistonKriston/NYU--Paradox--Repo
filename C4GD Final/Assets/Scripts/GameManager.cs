using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Vector3 playerPosition = Vector3.zero;
    public ArrayList enemyPositions;
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
    private void Start()
    {
        SaveGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveGame()
    {
        playerPosition = PlayerController.instance.transform.position;
        playerInPast = PlayerController.instance.GetComponent<TimeTravel>().inPast;
        EnemyEvenMoreFinal[] enemies = FindObjectsByType<EnemyEvenMoreFinal>(FindObjectsSortMode.InstanceID);
        print(enemies.ToString());
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyPositions[i] = (enemies[i].gameObject.GetInstanceID(), enemies[i].transform.position);
        }
        print(enemyPositions.ToString());
    }

    public void Load()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
