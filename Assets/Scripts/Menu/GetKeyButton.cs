using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GetKeyButton : Button
{
    [Header("Action Settings")]
    /// <summary>
    /// The action this Button gets the key for
    /// </summary>
    [SerializeField] public int action;

    // Start is called before the first frame update
    new void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = ((KeyCode)UserData.singleton.keymap[action]).ToString();
    }
}
