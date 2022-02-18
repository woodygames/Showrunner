using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField]
    public int damage;
    [SerializeField]
    private float magSize;

    private float currentMagSize;

    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float cooldownTime;

    private bool isReady;

    private bool reloading = false;

    private float timer;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private Transform firePoint;

    private bool isRanged;

    [SerializeField]
    private GameObject blade;
    [SerializeField]
    private Animation anim;

    private void Start()
    {
        currentMagSize = magSize;
        isRanged = GetComponent<EnemyController>().isRangedWeapon;
    }


    private void Update()
    {
        isReady = GetComponent<EnemyController>().readyToFire();
        //Debug.Log(isReady);
        if (isReady|| GetComponent<EnemyController>().readyToSlash())
        {
            if (isRanged&&isReady)
                shoot();
            else
            {
                if (GetComponent<EnemyController>().readyToSlash()) ;
                    slash();
            }
        }
    }
    private void slash()
    {
        if (timer >= 1/fireRate)
        {
            GetComponentInChildren<meleeDanage>().canDamage = true;
            timer = .0f;
            anim.Play("MeleeAnim");
            Debug.Log("play");
        }
        else
        {
            timer += Time.deltaTime;
        }
    }


    private void shoot()
    {
        if (timer >= 1 / fireRate&&currentMagSize>0)
        {
            currentMagSize--;
            timer = .0f;
            var bullet = Instantiate(projectile,firePoint.position,firePoint.rotation);
            bullet.GetComponent<projectileController>().damage=damage;
            bullet.GetComponent<projectileController>().range = GetComponent<EnemyController>().maxEngagementDistance;
        }
        else
        {
            if (currentMagSize == 0)
            {
                if (timer >= cooldownTime)
                {
                    currentMagSize = magSize;
                    timer = 0.0f;
                }
            }
            timer += Time.deltaTime;
        }
    }

}
