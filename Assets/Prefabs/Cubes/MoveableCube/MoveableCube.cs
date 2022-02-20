using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableCube : Interactable
{
    bool isAttached = false;

    [SerializeField]
    private float offset = 2f;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
  
        if (isAttached)
        {
            float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position);
            if (Input.GetMouseButton(0) && distance < range )
            {
                print("flying");
                Rigidbody rigidbody = GetComponent<Rigidbody>();
                rigidbody.position = new Vector3(
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)).x,
                     GameObject.FindGameObjectWithTag("Player").transform.position.y + offset,
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)).z
                    );
            }
            else
            {
                print("stopped flying");
                isAttached = false;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y + offset, gameObject.transform.position.z);
            }
        }
    }

    public override bool GetPass()
    {
        return true;
    }

    /// <summary>
    /// Trigger opens the door
    /// </summary>
    public override void Trigger()
    {
        float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position);
        if (distance > range) return;
        print("triggered");
        isAttached = true;
    }

    public override bool OutlineIsRed()
    {
        float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position);
        return (distance > range);
    }
}
