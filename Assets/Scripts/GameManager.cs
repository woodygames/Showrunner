using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The name of the gameScene
    /// </summary>
    [SerializeField]
    private string gameScene;

    /// <summary>
    /// The name of the mainMenu scene
    /// </summary>
    [SerializeField]
    private string mainMenuScene;

    /// <summary>
    /// Reference to the settingsMenu
    /// </summary>
    [SerializeField]
    private SettingsMenu settingsMenu;

    public void Awake()
    {
        GameObject.DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Switches to the gameScene and starts the game.
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
    }

    /// <summary>
    /// Finishes the game and switches back to the mainMenu
    /// </summary>
    public void FinishGame()
    {
        settingsMenu.Close();
        SceneManager.LoadScene(mainMenuScene, LoadSceneMode.Single);
    }

}
