using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{

    /// <summary>
    /// The SettingsMenu Singleton.
    /// </summary>
    public static SettingsMenu singleton;

    /// <summary>
    /// The AudioMixer to adjust the volume.
    /// </summary>
    [SerializeField]
    private AudioMixer mixer;

    /// <summary>
    /// The Canvas GameObject containing the SettingsMenu
    /// </summary>
    [SerializeField]
    private GameObject canvas;

    /// <summary>
    /// The GameObject of the finsihButton
    /// </summary>
    [SerializeField]
    private GameObject finishButton;

    [SerializeField]
    private string gameScene;

    /// <summary>
    /// Adjusts the master sound value according to the slider value.
    /// </summary>
    /// <param name="sliderValue">The new Slider Value.</param>
    public void SetSoundLevel(float sliderValue)
    {
        mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }

    /// <summary>
    /// Sets this gameObject (SettingsMenu) inactive.
    /// </summary>
    public void Close()
    {
        canvas.SetActive(false);
        finishButton.SetActive(false);
    }

    #region Standard Methods
    private void Start()
    {
        if (SettingsMenu.singleton != null)
        {
            Destroy(this.gameObject);
            return;
        }

        SettingsMenu.singleton = this;
        GameObject.DontDestroyOnLoad(this);
    }

    private void Update()
    {

        if(SceneManager.GetActiveScene().name.Equals(gameScene) && PlayerInput.singleton.escape && !canvas.activeSelf)
        {
            canvas.SetActive(true);
            finishButton.SetActive(true);

        } else if (PlayerInput.singleton.escape && canvas.activeSelf)
        {
            Close();
        }

        if (listenForInput && Input.anyKey)
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    OnKeySelected(keyCodes[i]);
                    break;
                }
            }
        }
    }
    #endregion

    #region Controls
    /// <summary>
    /// True if an input from the user is expected
    /// </summary>
    private bool listenForInput = false;

    /// <summary>
    /// The currently selected control Button
    /// </summary>
    private GetKeyButton selectedControlButton;

    /// <summary>
    /// The currently selected Action
    /// </summary>
    private Action selectedAction;

    /// <summary>
    /// The valid keys in this game
    /// </summary>
    private static readonly KeyCode[] keyCodes = Enum.GetValues(typeof(KeyCode))
                                                 .Cast<KeyCode>() //directly cast into Keycodes so we don't have to cast 
                                                 .Where(k => ((int)k < (int)KeyCode.Joystick1Button0)) //exclude all the joystick keys from the list (may have to be removed in the future)
                                                 .ToArray();

    /// <summary>
    /// this method should be called when a control-select-button is pressed
    /// </summary>
    /// <param name="b">The pressed button</param>
    public void OnButtonSelected(GetKeyButton b)
    {
        selectedControlButton = b;
        selectedAction = (Action)selectedControlButton.action;
        listenForInput = true;
        selectedControlButton.GetComponentInChildren<TextMeshProUGUI>().text = "Press the new key!";
    }

    /// <summary>
    /// This method should be called when a key is selected by the user. It sets everything up so this gets the new key for the selected action.
    /// </summary>
    /// <param name="k">The new KeyCode</param>
    private void OnKeySelected(KeyCode k)
    {
        listenForInput = false;
        UserData.singleton.ChangeKey(selectedAction, k);
        selectedControlButton.GetComponentInChildren<TextMeshProUGUI>().text = ((KeyCode)UserData.singleton.keymap[(int)selectedAction]).ToString();
    }

    #endregion

}
