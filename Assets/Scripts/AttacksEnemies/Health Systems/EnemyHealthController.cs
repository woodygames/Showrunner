using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    //max health after instanciation
    [SerializeField]
    private int maxHealth;
    private int health; //Current health

    //What weapon is droppen on Destruction?
    [SerializeField]
    private GameObject weaponDrop;
    [SerializeField]
    private float probability;

    [SerializeField]
    private GameObject deathObject;

    [SerializeField]
    private Transform dropPosition;

    void Awake()
    {
        health = maxHealth;
    }

    void Update()
    {
        //update the health bar/lives here

        //kill if no health left
        if (health <= 0)
        {
            Die();
        }
    }

    //Changes health based on value (-> damage is negative!)
    public void ChangeHealth(int value)
    {
        health += value;
    }

    private void Die()
    {
        Instantiate(deathObject, transform.position, transform.rotation);
        if(Random.value<probability)
            Instantiate(weaponDrop,dropPosition.position,dropPosition.rotation);
        Destroy(gameObject);
        //kill effect goes right here
    }
}
