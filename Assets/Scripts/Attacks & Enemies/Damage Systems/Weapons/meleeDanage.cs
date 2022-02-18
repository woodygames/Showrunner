using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeDanage : MonoBehaviour
{
    //if set to true, damaged list is cleared
    public bool canDamage;
    //List of already damaged enemies, cant damage twice
    private List<GameObject> damaged = new List<GameObject>();

    //How long does the collider inflict damage? should be the animations time
    [SerializeField]
    private float damageTime;

    private int damage;

    private float timer;

    private void Awake()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject&&(other.gameObject.GetComponent<HealthController>()|| other.gameObject.GetComponentInParent<HealthController>()) && timer<damageTime&&other.gameObject.layer!=2)
        {

            if (!damaged.Contains(other.gameObject))
            {
                other.gameObject.GetComponent<HealthController>().ChangeHealth(-damage);
                damaged.Add(other.gameObject);
            }
        }
    }
}
