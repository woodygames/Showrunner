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

    private bool isRanged;

    [SerializeField]
    private GameObject blade;
    [SerializeField]
    private GameObject effects;

    [SerializeField]
    private GameObject muzzleFlash;

    [SerializeField]
    private Transform muzzle1,muzzle2;

    private bool isMuzzle1;

    private void Start()
    {
        currentMagSize = 0;
        isRanged = GetComponent<EnemyController>().isRangedWeapon;
    }


    private void Update()
    {
        if (GetComponent<EnemyController>().readyToSlash()&&!isRanged)
        {
            effects.SetActive(true);
        }
        else if(!GetComponent<EnemyController>().readyToSlash() && !isRanged)
        {
            effects.SetActive(false);
        }

        isReady = GetComponent<EnemyController>().readyToFire();
        //Debug.Log(isReady);
        if (isReady|| GetComponent<EnemyController>().readyToSlash())
        {
            if (isRanged && isReady)
            {
                shoot();
            }
            else if(!isRanged&& GetComponent<EnemyController>().readyToSlash())
            {
                slash();
            }
        }
    }
    private void slash()
    {
        if (timer >= 1/fireRate)
        {
            GetComponentInChildren<meleeDamage>().canDamage = true;
            timer = .0f;
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
            if (muzzle1 && muzzle2)
            {
                if (isMuzzle1)
                {
                    Instantiate(muzzleFlash, muzzle1.position, muzzle1.rotation);
                    var bullet = Instantiate(projectile, muzzle1.position, muzzle1.rotation);
                    bullet.GetComponent<projectileController>().damage = damage;
                    bullet.GetComponent<projectileController>().range = GetComponent<EnemyController>().maxEngagementDistance;
                    isMuzzle1 = false;
                }
                else
                {
                    Instantiate(muzzleFlash, muzzle2.position, muzzle2.rotation);
                    var bullet = Instantiate(projectile, muzzle2.position, muzzle2.rotation);
                    bullet.GetComponent<projectileController>().damage = damage;
                    bullet.GetComponent<projectileController>().range = GetComponent<EnemyController>().maxEngagementDistance;
                    isMuzzle1 = true;
                }


            }
       
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
