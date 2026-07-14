using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseResume : MonoBehaviour
{
     public GameObject pauseScreen;
     public bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            Pause();
          Debug.Log("Paused");
         
          isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
        {
            Resume();
          Debug.Log("Resume");
          isPaused = false;
        }
      
    }
    void Pause()
    {
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
       // Debug.Log("Paused");
    }
    void Resume()
    {
        Time.timeScale = 1f;
         pauseScreen.SetActive(false);
         //Debug.Log("Resume");
    }
}
