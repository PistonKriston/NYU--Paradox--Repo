using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public Button respawn;
    public Button mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        respawn.onClick.AddListener(respawnButton);
        mainMenu.onClick.AddListener(mainMenuButton);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mainMenuButton()
    {
        SceneManager.LoadScene(2);
    }

    public void respawnButton()
    {
        GameManager.instance.Load();
    }
}
