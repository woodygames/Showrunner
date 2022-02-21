using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    /// <summary>
    /// The GameManager Singleton.
    /// </summary>
    public static GameManager singleton;

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

    /// <summary>
    /// True if the game is running false if not.
    /// </summary>
    private bool gameStartet = false;

    #region Standard Methods
    void Awake()
    {
        if (GameManager.singleton != null)
        {
            Destroy(this.gameObject);
            return;
        }
        GameManager.singleton = this;
        GameObject.DontDestroyOnLoad(this);
        timeRemaining = time;
    }

    void Update()
    {
        if (gameStartet)
        {
            UpdateTime();
        }
    }
    #endregion

    /// <summary>
    /// Switches to the gameScene and starts the game.
    /// </summary>
    public void StartGame()
    {
        gameStartet = true;
        Menu.singleton.gameObject.SetActive(false);
        SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
    }

    /// <summary>
    /// Finishes the game and switches back to the mainMenu
    /// </summary>
    public void FinishGame()
    {
        gameStartet = false;
        settingsMenu.Close();
        SceneManager.LoadScene(mainMenuScene, LoadSceneMode.Single);
        Menu.singleton.gameObject.SetActive(true);
    }

    #region Time Manageement
    /// <summary>
    /// Time availlable for the game
    /// </summary>
    [SerializeField]
    private float time = 180f;

    /// <summary>
    /// The Time remaining until the game is finished
    /// </summary>
    private float timeRemaining;

    /// <summary>
    /// The TextField for the time.
    /// </summary>
    [SerializeField]
    private TMP_Text timeText;

    /// <summary>
    /// Updates the timer (also in the UI)
    /// </summary>
    private void UpdateTime()
    {
        timeRemaining -= Time.deltaTime;
        if(timeRemaining <= 0)
        {
            timeText.text = "";
            timeRemaining = time;
            FinishGame();
        }
        else
        {
            float minutes = Mathf.FloorToInt(timeRemaining / 60);
            float seconds = Mathf.FloorToInt(timeRemaining % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    #endregion

}
