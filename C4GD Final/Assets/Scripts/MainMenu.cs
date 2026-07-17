using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button controllsButton;
    public Button mainMenuButton;
    public GameObject continueButton;
    public GameObject controlls;
    public GameObject mainMenu;
    public GameObject loreMenu;
    public GameObject music;
    public bool canContinue = false;

      public float maxTimeRemaining = 30; // seconds

    public float timeRemaining = 30; // seconds
    public bool timerIsRunning = false;
    
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(LoreMenu);
        controllsButton.onClick.AddListener(ControlsGameButton);
        mainMenuButton.onClick.AddListener(MainMenuButton);
    }

    // Update is called once per frame
    void Update()
    {
         if (timerIsRunning)
        {
             if (timeRemaining > 0)
        {
        timeRemaining -= Time.deltaTime;
       
        }
        else
        {
    
            canContinue = true;
        
        }
        
        }
        if (canContinue == true)
        {
            continueToGame();
        }
    }
    public void StartGameButton()
    {
        
        SceneManager.LoadScene("Level _1_Map");

    }
     public void ControlsGameButton()
    {
        controlls.SetActive(true);
        
        mainMenu.SetActive(false);
    }
     public void MainMenuButton()
    {
        controlls.SetActive(false);
      
        mainMenu.SetActive(true);
    }
    public void LoreMenu()
    {
        music.SetActive(false);
        mainMenu.SetActive(false);
        loreMenu.SetActive(true);
        timeRemaining = maxTimeRemaining;
        timerIsRunning = true;
    }
    public void continueToGame()
    {
        continueButton.SetActive(true);
        timerIsRunning = false;
        canContinue = false;
    }
}
