using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("A list of all playable characters; they are being played from first to last")]
    [SerializeField]
    private List<GameObject> characters;

    [Tooltip("The start position of the player")]
    [SerializeField]
    private Transform start;

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

    public void Awake()
    {
        if (GameManager.singleton != null)
        {
            Destroy(this.gameObject);
            return;
        }
        GameManager.singleton = this;
        GameObject.DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += SetStartPosition;
    }

    /// <summary>
    /// Switches to the gameScene and starts the game.
    /// </summary>
    public void StartGame()
    {
        Menu.singleton.gameObject.SetActive(false);
        SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
    }

    /// <summary>
    /// Finishes the game and switches back to the mainMenu
    /// </summary>
    public void FinishGame()
    {
        settingsMenu.Close();
        SceneManager.LoadScene(mainMenuScene, LoadSceneMode.Single);
        Menu.singleton.gameObject.SetActive(true);
    }

    /// <summary>
    /// removes and returns the first character from the list of playable characters, and spawns the character into the game.
    /// </summary>
    /// <returns>The transform of the character removed</returns>
    public Transform GetNextTarget()
    {
        if(characters.Count <= 0)
        {
            ShowGameOver();
            return start;
        }
        GameObject nextCharacter = characters[0];
        GameObject player = Instantiate(nextCharacter, start.position, start.rotation);
        characters.Remove(nextCharacter);

        return player.transform;
    }

    public void ShowGameOver()
    {

    }

    public void SetStartPosition(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(gameScene.Equals(scene.name))
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Start");
            start = obj.transform;
            Camera.main.GetComponent<CameraController>().SwitchTarget();
        }
    }

}
