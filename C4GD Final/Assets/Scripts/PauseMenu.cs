using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuObject;
    public GameObject controlls;
    public Button controllsButton;
    public Button mainMenuButton;
    public Button BackToGame;
    public GameObject playerUI;
    // Start is called before the first frame update
    void Start()
    {
        controllsButton.onClick.AddListener(ControlsGameButton);
        mainMenuButton.onClick.AddListener(MainMenuButton);
        BackToGame.onClick.AddListener(BackToGameButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     public void MainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Noam-Testing");
    }
     public void ControlsGameButton()
    {
        controlls.SetActive(true);
        playerUI.SetActive(false);
   
    }
     public void BackToGameButton()
    {
        controlls.SetActive(false);
        playerUI.SetActive(true);
    }
  
}
