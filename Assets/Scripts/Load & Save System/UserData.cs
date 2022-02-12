using UnityEngine;

[System.Serializable]
public class UserData
{
    #region singleton logic
    private static UserData _data;

    /// <summary>
    /// The UserData singleton
    /// </summary>
    public static UserData singleton
    {
        get
        {
            if (_data == null) _data = new UserData();
            return _data;
        }

        private set
        {
            if (_data != null) return;
            _data = (value != null) ? value : new UserData();
            DataLoaded.Invoke();
        }
    }

    #endregion

    /// <summary>
    /// The language the user has the set
    /// </summary>
    public int language;

    #region KeyMapping
    /// <summary>
    /// The Keymap of the control system
    /// </summary>
    public int[] keymap = { (int)KeyCode.W, (int)KeyCode.S, (int)KeyCode.A, (int)KeyCode.D, (int)KeyCode.LeftShift, (int)KeyCode.C, (int)KeyCode.E, (int)KeyCode.Mouse1, (int)KeyCode.Mouse0, (int)KeyCode.Space, (int)KeyCode.Escape };

    /// <summary>
    /// Changes the Key for the given action to the given Key.
    /// </summary>
    /// <param name="action">The Action the Key should be changed.</param>
    /// <param name="newKey">The new Key for the Action.</param>
    public void ChangeKey(Action action, KeyCode newKey)
    {
        foreach (KeyCode code in keymap)
        {
            if (code == newKey) return;
        }

        keymap[(int)action] = (int)newKey;
        if (PlayerInput.singleton != null) PlayerInput.singleton.loadKeyMap();
    }
    #endregion

    /// <summary>
    /// Loads the Data from previous sessions
    /// </summary>
    public static void Setup()
    {
        singleton = SaveSystem.LoadUserData();
    }

    #region Events
    public static event DataLoadedAction DataLoaded;
    public delegate void DataLoadedAction();
    #endregion
}