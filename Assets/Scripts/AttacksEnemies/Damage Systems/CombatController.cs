using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CombatController : MonoBehaviour
{
    //Slots for the weapons - scriptable objects store the data for different items
    [SerializeField]
    public WeaponTemplate rangedWeapon;
    [SerializeField]
    public WeaponTemplate meleeWeapon;

    //Positions for the different weapon archetypes
    [SerializeField]
    private Transform rangedPosition;
    [SerializeField]
    private Transform meleePosition;

    //Weapon index, 1 for ranged, 2 for melee
    //if there is no weapon or a ranged weapon only, 2 is bare hands with no fighting ability
    private int currentWeaponIndex=3;
    //the model of the currently used weapon
    private GameObject currentWeaponModel;

    //max distance to pick up weapon
    [SerializeField]
    private float maxPickupDistance = 3.0f;

    public WeaponTemplate currentWeapon;

    private float timer;
    private Animation anim; //change when using Animator
    private int currentMagSize;
    private float coneSize;

    private Animator animator;

    private AnimatorStateInfo stateInfo;

    private bool pelletMode;
    private int pelletCount;
    private GameObject muzzleFlash;

    [SerializeField]
    private Transform firePoint;

    private void Start()
    {
        if (rangedWeapon)
        {
            currentMagSize = rangedWeapon.magSize;
        }
        animator = GetComponentInChildren<Animator>();
    }
    //Update once per frame
    private void Update()
    {
        if (currentWeapon)
        {
            if(currentWeapon.muzzleFlash)
                muzzleFlash = currentWeapon.muzzleFlash;
            coneSize = currentWeapon.coneSize;
            pelletMode = currentWeapon.usePelletMode;
            pelletCount = currentWeapon.pelletCount;

        }
        timer += Time.deltaTime;

        if (currentWeaponIndex == 1)
        {
            currentWeapon = rangedWeapon;
        }
        else if (currentWeaponIndex == 2)
        {
            currentWeapon = meleeWeapon;
        }
        else
        {
            currentWeapon = null;
        }
        CombatInput();
        switchWeapons();

        if (rangedWeapon){
            if (currentWeapon == rangedWeapon && timer >= rangedWeapon.magReload && currentMagSize == 0)
            {
                currentMagSize = rangedWeapon.magSize;
                timer = 0.0f;
            }
        }
        if (currentWeaponIndex == 1)
        {
            //disable attack movement
            GetComponent<PlayerMovementController>().RemoveMovementType(GetComponent<AttackMovement>());
            animator.runtimeAnimatorController = Resources.Load("CharacterAttacksFinished") as RuntimeAnimatorController;
        }
        else if (currentWeaponIndex == 2)
        {
            //enable attack movement
            GetComponent<PlayerMovementController>().AddMovementType(GetComponent<AttackMovement>());
            animator.runtimeAnimatorController = Resources.Load("CharacterMeleeFinished") as RuntimeAnimatorController;
        }
        else
        {
            //disable attack movement
            GetComponent<PlayerMovementController>().RemoveMovementType(GetComponent<AttackMovement>());
            //GetComponent<DashMovement>().enabled = true;
            animator.runtimeAnimatorController = Resources.Load("CharacterWeaponlessFinished") as RuntimeAnimatorController;
        }
    }

    //CALL IN UPDATE
    //Method to check the overall Inputs concerning combat
    public void CombatInput()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (Input.GetMouseButton(1))
        {
            
            if (currentWeapon == rangedWeapon && rangedWeapon&&(stateInfo.IsName("Walking")|| stateInfo.IsName("Idle")))
            {
                GetComponent<PlayerMovementController>().RemoveMovementType(GetComponent<AttackMovement>());
                shoot();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            
            if (currentWeapon == meleeWeapon && meleeWeapon)
            {
                GetComponent<PlayerMovementController>().AddMovementType(GetComponent<AttackMovement>());
                slash();
            }
        }
        //what was clicked on?
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = Camera.main.gameObject.GetComponent<CameraController>().GetCursorHit();
            //Pickup weapon if clicked
            if (hit.collider.gameObject.GetComponent<WeaponPhysicalItem>())
            {
                var hitObject = hit.collider.gameObject.GetComponent<WeaponPhysicalItem>();
                //Is the weapon in a certain radius arround the player?
                if(Vector3.Distance(transform.position,hitObject.gameObject.transform.position) <= maxPickupDistance)
                {
                    OnWeaponChange(hitObject.weapon);
                    Destroy(hitObject.gameObject);
                }
                return;
            }
        }
    }


    //CALL IN UPDATE
    //Method for switching weapons
    //weapons need to be on the character, otherwise dont do anything
    //X to switch to bare hands (currentWeaponIndex==3) , INPUT CHANGES IN THE FUTURE! <------------------
    public void switchWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)&&!(Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space)))
        {
            currentWeaponIndex = 1;
            UpdateWeaponModel();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space)))
        {
            currentWeaponIndex = 2;
            UpdateWeaponModel();
        }
        else if (Input.GetKeyDown(KeyCode.X) && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space)))
        {
            currentWeaponIndex = 3;
            UpdateWeaponModel();
        }
    }




    //Update the weapons appearence on the character
    //if the index 2, add melee, if index 1, add ranged, else, free the hands
    //should work for switching as well as picking weapons up.
    public void UpdateWeaponModel()
    {
        if (currentWeaponIndex == 1)
        {
            if (rangedWeapon)
            {
                
                //same as below, optional implementation of object pooling in the future
                Destroy(currentWeaponModel);
                currentWeaponModel = Instantiate(rangedWeapon.weaponModel, rangedPosition, false);
                currentWeaponModel.transform.localPosition = new Vector3(0, 0, 0);
                currentWeaponModel.transform.localRotation = Quaternion.identity;
            }
        }
        else if(currentWeaponIndex==2)
        {
            if (meleeWeapon)
            {
               
                Destroy(currentWeaponModel);
                currentWeaponModel = Instantiate(meleeWeapon.weaponModel, meleePosition, false);
                currentWeaponModel.transform.localPosition = new Vector3(0, 0, 0);
                currentWeaponModel.transform.localRotation = Quaternion.identity;
            }
        }
        else
        { 
            Destroy(currentWeaponModel);
        }
    }


    //Exchange the weapon objects and put the former one on the ground if it exists; should be triggered when clicking on a weapon (access its template)
    public void OnWeaponChange(WeaponTemplate weapon)
    {
        if (weapon.GetIsRanged())
        {
            //Spawn formerly equipped weapon object as collectable item on the ground (maybe change the spawn position and flight path), no collision with player
            //maybe use object-pooling for better performance later!
            //weapon cant be null, otherwise just add the new weapon
            if (rangedWeapon)
            {
                GameObject weaponPrefab = Instantiate(rangedWeapon.weaponPhysicalItem);
                weaponPrefab.transform.position = gameObject.transform.position;
                weaponPrefab.transform.rotation = Quaternion.identity;
                currentWeaponIndex = 1;
            }
            //If there is no weapon present, switch to the newly aquired one
            else
            {
                currentWeaponIndex = 1;
            }

            //add the weapon to the inventory and update the displayed weapon model
            rangedWeapon = weapon;
            currentMagSize = weapon.magSize;
            UpdateWeaponModel();
        }
        //same as above, just for the melee weapon
        else
        {
            if (meleeWeapon)
            {
                GameObject weaponPrefab = Instantiate(meleeWeapon.weaponPhysicalItem);
                weaponPrefab.transform.position = gameObject.transform.position;
                weaponPrefab.transform.rotation = Quaternion.identity;
                currentWeaponIndex = 2;
            }
            else
            {
                currentWeaponIndex = 2;
            }

            meleeWeapon = weapon;
            UpdateWeaponModel();
        }
    }

    private void slash()
    {
        if (timer >= 1 / meleeWeapon.attackRate)
        {
            GetComponentInChildren<meleeDamage>().canDamage = true;
            timer = 0.0f;
            animator.SetTrigger("attack");
        }
    }


    private void shoot()
    {
        if (timer >= (1 / rangedWeapon.attackRate) && currentMagSize > 0)
        {
            var muzzlePosition = GetComponentInChildren<muzzleFlashManager>().gameObject.transform;
            if (!pelletMode)
            {
                Instantiate(muzzleFlash, muzzlePosition.position, muzzlePosition.rotation);
                currentMagSize--;
                timer = 0.0f;
                float randx = Random.value;
                float randy = Random.value;
                Vector3 cone = new Vector3(randx*coneSize,0,randy*coneSize);
                var bullet = Instantiate(rangedWeapon.projectile, firePoint.position,new Quaternion(firePoint.rotation.x+cone.x, firePoint.rotation.y, firePoint.rotation.z+cone.z, firePoint.rotation.w));
                bullet.GetComponent<projectileController>().damage = rangedWeapon.damage;
                bullet.GetComponent<projectileController>().range = rangedWeapon.maxRange;
            }
            else
            {
                currentMagSize--;
                timer = 0.0f;

                Instantiate(muzzleFlash,muzzlePosition.position,muzzlePosition.rotation);

                for(int i=0; i < pelletCount; i++)
                {
                    float randx = (Random.value-Random.value);
                    float randy = (Random.value - Random.value);
                    float randz = (Random.value - Random.value);
                    Vector3 cone = new Vector3(randx * coneSize, randy*coneSize, randz * coneSize);
                    var bullet = Instantiate(rangedWeapon.projectile, firePoint.position, new Quaternion(firePoint.rotation.x + cone.x, firePoint.rotation.y+cone.y, firePoint.rotation.z + cone.z, firePoint.rotation.w));
                    bullet.GetComponent<projectileController>().damage = rangedWeapon.damage;
                    bullet.GetComponent<projectileController>().range = rangedWeapon.maxRange;
                }
            }
        }
        
    }

}

