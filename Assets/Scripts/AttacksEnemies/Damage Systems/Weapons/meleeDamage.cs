using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeDamage : MonoBehaviour
{
    //if set to true, damaged list is cleared
    public bool canDamage;
    //List of already damaged enemies, cant damage twice
    private List<GameObject> damaged = new List<GameObject>();

    //How long does the collider inflict damage? should be the animations time
    [SerializeField]
    private float damageTime;

    [SerializeField]
    private int damage;

    private float timer;

    [SerializeField]
    private bool isPlayer;

    [SerializeField]
    private GameObject hitPlayer, hitEnemy;

    private void Start()
    {
        if (GetComponentInParent<EnemyCombat>())
        {
            damage = GetComponentInParent<EnemyCombat>().damage;
        }
        else if (GetComponentInParent<CombatController>())
        {
            damage = GetComponentInParent<CombatController>().meleeWeapon.damage;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (canDamage)
        {
            timer = .0f;
            canDamage = false;
            damaged.Clear();
        }

        if(timer<damageTime*2)
            timer += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Collided with" + other);
        if (other.gameObject&&(other.gameObject.GetComponent<EnemyHealthController>() || other.gameObject.GetComponentInParent<EnemyHealthController>()) && timer < damageTime && isPlayer)
        {
            //Debug.Log("Hit Enemy!");
            if (!damaged.Contains(other.gameObject))
            {
                Instantiate(hitEnemy, transform.position, transform.rotation);

                if(other.gameObject.GetComponent<EnemyHealthController>())
                    other.gameObject.GetComponent<EnemyHealthController>().ChangeHealth(-damage);
                else
                    other.gameObject.GetComponentInParent<EnemyHealthController>().ChangeHealth(-damage);
                damaged.Add(other.gameObject);
            }
        }

        else if (other.gameObject && (other.gameObject.GetComponent<HealthController>() || other.gameObject.GetComponentInParent<HealthController>()) && timer<damageTime &&!isPlayer)
        {

            //Debug.Log("Hit Player!");
            if (!damaged.Contains(other.gameObject))
            {
                Instantiate(hitPlayer, transform.position, transform.rotation);
                if (other.gameObject.GetComponent<HealthController>())
                    other.gameObject.GetComponent<HealthController>().ChangeHealth(-damage);
                else
                    other.gameObject.GetComponentInParent<HealthController>().ChangeHealth(-damage);
                damaged.Add(other.gameObject);
            }
        }
        
    }
}
