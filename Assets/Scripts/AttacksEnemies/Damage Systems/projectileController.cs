using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileController : MonoBehaviour
{
    public int damage;
    public float range;

    private float rangeTravelled;

    [SerializeField]
    private float bulletSpeed;

    private int layerMask = 257;

    [SerializeField]
    private GameObject playerHit;

    [SerializeField]
    private GameObject enviromentHit;

    [SerializeField]
    private bool isPlayer = false;

    private void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, bulletSpeed * Time.fixedDeltaTime, layerMask))
        {
            if (hit.collider.gameObject.GetComponent<HealthController>())
            {
                hit.collider.gameObject.GetComponent<HealthController>().ChangeHealth(-damage);
                Instantiate(playerHit, hit.point, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                Instantiate(enviromentHit, hit.point, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position += transform.TransformDirection(Vector3.forward).normalized * bulletSpeed * Time.fixedDeltaTime;
            rangeTravelled += (transform.TransformDirection(Vector3.forward).normalized * bulletSpeed * Time.fixedDeltaTime).magnitude;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rangeTravelled > range)
        {
            Destroy(gameObject);
        }


        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, bulletSpeed*Time.fixedDeltaTime, layerMask))
        {
            if (hit.collider.gameObject.GetComponent<HealthController>()&&!isPlayer)
            {
                hit.collider.gameObject.GetComponent<HealthController>().ChangeHealth(-damage);
                Instantiate(playerHit, hit.point, Quaternion.identity);
                Destroy(gameObject);
            }
            else if(hit.collider.GetComponent<EnemyHealthController>()&&isPlayer)
            {
                hit.collider.gameObject.GetComponent<EnemyHealthController>().ChangeHealth(-damage);
                Instantiate(playerHit, hit.point, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                Instantiate(enviromentHit, hit.point, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position += transform.TransformDirection(Vector3.forward).normalized * bulletSpeed * Time.fixedDeltaTime;
            rangeTravelled += (transform.TransformDirection(Vector3.forward).normalized * bulletSpeed * Time.fixedDeltaTime).magnitude;
        }
    }
}
