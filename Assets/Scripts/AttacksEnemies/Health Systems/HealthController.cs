using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    //max health after instanciation
    [SerializeField]
    private int maxHealth;
    private int health; //Current health
    [SerializeField]
    private Transform healthBar;

    [SerializeField]
    private GameObject deadBody;

    void Awake()
    {
        health = maxHealth;
    }

    void Update()
    {
        //update the health bar/lives here
        healthBar.localScale = new Vector3((((float)health) /maxHealth), healthBar.localScale.y, healthBar.localScale.z);
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
        Instantiate(deadBody, transform.position, transform.rotation);
        Destroy(gameObject);
        //kill effect goes right here
    }
}
