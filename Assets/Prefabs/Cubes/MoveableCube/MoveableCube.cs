using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableCube : Interactable
{
    bool isAttached = false;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float height;



    // Start is called before the first frame update
    void Start()
    {

    }

   /* private void OnDrawGizmos()
    {
        Vector3 curserPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z)));
        Gizmos.color = Color.red;
        direction = curserPoint - Camera.main.transform.position;
        print(Vector3.Distance(curserPoint, Camera.main.transform.position));
        Gizmos.DrawRay(Camera.main.transform.position,   Camera.main.transform.position- curserPoint);
    }*/

    // Update is called once per frame
    void Update()
    {
       

        
        if (isAttached)
        {
            Vector3 curserPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z)));
            Debug.DrawRay(Camera.main.transform.position, curserPoint - Camera.main.transform.position, Color.green);

            Vector3 aufpunkt = Camera.main.transform.position;
            Vector3 direction = curserPoint - aufpunkt;

            float alpha = (-aufpunkt.y + height) / direction.y;
            Vector3 endpoint = aufpunkt + alpha * direction;

            float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position);
            if (Input.GetMouseButton(0) && distance < range )
            {
              

                Rigidbody rigidbody = GetComponent<Rigidbody>();
                rigidbody.position = endpoint + offset;


                   /* new Vector3(
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z))).x,
                     GameObject.FindGameObjectWithTag("Player").transform.position.y + offset,
                     GameObject.FindGameObjectWithTag("Player").transform.position.z
                    );*/
            }
            else
            {
                isAttached = false;
               // gameObject.transform.position = new Vector3(gameObject.transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y + offset, gameObject.transform.position.z);
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
