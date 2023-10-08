using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
   public void PlayGame()
    {
        SceneManager.LoadScene("IntroCutscene");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();

    }
    public void OptionsMenu()
    {

        Debug.Log("Loading Options Menu...");
        SceneManager.LoadScene("OptionsMenu");
    }

    public void CreditsMenu()
    {
        Debug.Log("Loading Credits Menu...");
        SceneManager.LoadScene("CreditsMenu");
    }


}
