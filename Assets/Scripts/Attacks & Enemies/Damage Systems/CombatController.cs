using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    //Slots for the weapons - scriptable objects store the data for different items
    [SerializeField]
    private WeaponTemplate rangedWeapon;
    [SerializeField]
    private WeaponTemplate meleeWeapon;

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

    //Update once per frame
    private void Update()
    {
        CombatInput();
        switchWeapons();
    }

    //CALL IN UPDATE
    //Method to check the overall Inputs concerning combat
    public void CombatInput()
    {
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
            }

            //fire the weapon in any other case
            else
            {
                //TODO
                //if ranged: get ammo and fire point from the weapon model or the predefined on the character, whatever it will be
                //if melee: do da swing

            }
        }
    }


    //CALL IN UPDATE
    //Method for switching weapons
    //weapons need to be on the character, otherwise dont do anything
    //X to switch to bare hands (currentWeaponIndex==3) , INPUT CHANGES IN THE FUTURE! <------------------
    public void switchWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeaponIndex = 1;
            UpdateWeaponModel();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeaponIndex = 2;
            UpdateWeaponModel();
        }
        else if (Input.GetKeyDown(KeyCode.X))
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
            }
        }
        else if(currentWeaponIndex==2)
        {
            if (meleeWeapon)
            {
                Destroy(currentWeaponModel);
                currentWeaponModel = Instantiate(meleeWeapon.weaponModel, meleePosition, false);
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
}

