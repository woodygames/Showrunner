using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    /// <summary>
    /// The Menu
    /// </summary>
    public static Menu singleton;

    /// <summary>
    /// The GameObject for the SettingsMenu.
    /// </summary>
    [SerializeField]
    private GameObject settingsMenu;

    void Awake()
    {
        if(Menu.singleton != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Menu.singleton = this;
        DontDestroyOnLoad(this);
    }

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
