using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    //max health after instanciation
    [SerializeField]
    private int maxHealth;
    private int health; //Current health

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
        Destroy(gameObject);
        //kill effect goes right here
    }
}
