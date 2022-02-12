using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="name",menuName ="Weapon")]
public class WeaponTemplate:ScriptableObject
{
    public int damage;
    public int attackRate;
    [SerializeField]
    private bool isRanged;
    public GameObject weaponModel;
    public GameObject weaponPhysicalItem;

    public bool GetIsRanged()
    {
        return isRanged;
    }
}
