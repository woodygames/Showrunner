using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    /// <summary>
    /// The GameObject for the SettingsMenu.
    /// </summary>
    [SerializeField]
    private GameObject settingsMenu;

    /// <summary>
    /// Quits the Application.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Activates the SettingsMenu GameObject.
    /// </summary>
    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }
    
}
