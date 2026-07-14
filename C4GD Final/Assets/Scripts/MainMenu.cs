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
    public GameObject controlls;
    public GameObject mainMenu;
   
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGameButton);
        controllsButton.onClick.AddListener(ControlsGameButton);
        mainMenuButton.onClick.AddListener(MainMenuButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGameButton()
    {
        
        SceneManager.LoadScene("Level 1");

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


}
