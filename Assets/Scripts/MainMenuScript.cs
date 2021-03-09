using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject optionsScreen;
    public GameObject mainMenu;

    void Start()
    {
        optionsScreen = GameObject.Find("OptionsCanvas");
        optionsScreen.SetActive(false);

        mainMenu = GameObject.Find("MainMenuCanvas");
        mainMenu.SetActive(true);
    }

   public void startButton()
    { SceneManager.LoadScene("George_TestScene"); }

    public void menuButton()
    { SceneManager.LoadScene("MainMenuScene"); }

    public void optionsButton()
    {
        if (!optionsScreen.activeInHierarchy)
        {
            optionsScreen.SetActive(true);
            mainMenu.SetActive(false);
        }
        else
        {
            optionsScreen.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    public void quitButton()
    { Application.Quit(); }

}
