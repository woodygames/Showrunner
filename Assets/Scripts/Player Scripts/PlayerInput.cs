using System;
using UnityEngine;

/// <summary>
/// All actions that can be startet/done/activated with a key
/// </summary>
public enum Action
{
    Forwards,       //0
    Backwards,      //1
    Left,           //2
    Right,          //3
    Run,            //4
    Crouch,         //5
    Interact,       //6
    Use,            //7
    Attacking,      //8
    Jump,           //9
    Escape          //10
}

public class PlayerInput : MonoBehaviour
{
    #region Singleton Logic
    public static PlayerInput singleton;

    private void Awake()
    {
        if (singleton != null) return;

        singleton = this;
        GameObject.DontDestroyOnLoad(this);
        UserData.DataLoaded += new UserData.DataLoadedAction(loadKeyMap);
    }
    #endregion

    private void Start()
    {
        loadKeyMap();
    }

    #region Key Managment
    /// <summary>
    /// The KeyCodes for the Actions in the Actions enum
    /// </summary>
    private KeyCode[] keyCodes;

    /// <summary>
    /// Loads the Users Keymap from the UserData
    /// </summary>
    public void loadKeyMap()
    {
        keyCodes = new KeyCode[UserData.singleton.keymap.Length];
        UserData.singleton.keymap.CopyTo(keyCodes, 0);
    }
    #endregion

    #region Input Checks

    #region "Looking"
    ///<summary>
    /// returns the input of the WASD keys / of the arrow keys
    ///</summary>
    public Vector2 wasd
    {
        get
        {
            Vector2 i = Vector2.zero;
            i.x = Input.GetAxis("Horizontal");
            i.y = Input.GetAxis("Vertical");
            i *= (i.x != 0.0f && i.y != 0.0f) ? .7071f : 1.0f;
            return i;
        }
    }

    ///<summary>
	/// same as input, but x and y can only be -1, 0 or 1
	///</summary>
    public Vector2 raw
    {
        get
        {
            Vector2 i = Vector2.zero;
            i.x = Input.GetAxisRaw("Horizontal");
            i.y = Input.GetAxisRaw("Vertical");
            i *= (i.x != 0.0f && i.y != 0.0f) ? .7071f : 1.0f;
            return i;
        }
    }
    #endregion

    /// <summary>
    /// returns the input of the A and D keys
    /// </summary>
	public float horizontal
	{
		//get { return Input.GetAxis("Horizontal"); }
		get
        { 
            if(Input.GetKey(keyCodes[(int)Action.Right]) || Input.GetKeyDown(keyCodes[(int)Action.Right])) return 1;
            if(Input.GetKey(keyCodes[(int)Action.Left]) || Input.GetKeyDown(keyCodes[(int)Action.Left])) return -1;
            return 0;
        }
	}

    /// <summary>
    /// returns the input of the W and S keys
    /// </summary>
	public float vertical
	{
        //get { return Input.GetAxis("Vertical"); }
        get
        {
            if (Input.GetKey(keyCodes[(int)Action.Forwards])) return 1;
            if (Input.GetKey(keyCodes[(int)Action.Backwards])) return -1;
            return 0;
        }
    }


    /// <summary>
    /// returns the hoirzontal mouse input
    /// </summary>
    public float mouseX
	{
		get { return Input.GetAxis("Mouse X"); }
	}

    /// <summary>
    /// returns the vertical mouse input
    /// </summary>
    public float mouseY
    {
        get { return Input.GetAxis("Mouse Y"); }
    }

    ///<summary>
    /// checks if LeftShift is pressed, which is the key for sprinting
    ///</summary>
    public bool running
    {
        get { return Input.GetKey(keyCodes[(int)Action.Run]); }
    }

    ///<summary>
    /// checks if LeftShift is pressed down, which is the key for sprinting
    ///</summary>
    public bool run
    {
        get { return Input.GetKeyDown(keyCodes[(int)Action.Run]); }
    }

    ///<summary>
	/// checks if C is pressed down, which is the key for crouching
	///</summary>
    public bool crouch
    {
        get { return Input.GetKeyDown(keyCodes[(int)Action.Crouch]); }
    }

    ///<summary>
	/// checks if C is pressed, which is the key for crouching
	///</summary>
    public bool crouching
    {
        get { return Input.GetKey(keyCodes[(int)Action.Crouch]); }
    }
	
	///<summary>
	/// checks if C is pressed up, which is the key for crouching
	///</summary>
	public bool stopCrouch
	{
		get { return Input.GetKeyUp(keyCodes[(int)Action.Crouch]); }
	}

    ///<summary>
	/// checks if E is pressed, which is the key for interacting
	///</summary>
    public bool interact
    {
        get { return Input.GetKeyDown(keyCodes[(int)Action.Interact]); }
    }

    ///<summary>
    /// checks if right mouse button is pressed, which is the button for using an item
    ///</summary>
    public bool use
    {
        get { return Input.GetKeyDown(keyCodes[(int)Action.Use]); }
    }

    ///<summary>
    /// checks if left mouse button is pressed, which is the button for attacking
    ///</summary>
    public bool attacking
    {
        get { return Input.GetKeyDown(keyCodes[(int)Action.Attacking]); }
    }

    /*public float mouseScroll
    { 
        get { return Input.GetAxisRaw("Mouse ScrollWheel"); }
    }*/

    ///<summary>
    /// checks if space is pressed
    ///</summary>
    public bool jump
    {
        get { return Input.GetKeyDown(keyCodes[(int)Action.Jump]); }
    }

    ///<summary>
    /// checks if space is pressed
    ///</summary>
    public bool jumping
    {
        get { return Input.GetKey(keyCodes[(int)Action.Jump]); }
    }

    /// <summary>
    /// checks if escape is pressed
    /// </summary>
    public bool escape
	{
		get { return Input.GetKeyDown(keyCodes[(int)Action.Escape]); }
	}
	
    #endregion
}
