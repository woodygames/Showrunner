using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : Interactable
{
    public override void Trigger()
    {
        Debug.Log("Hallo wilkommen im Shop");
    }

    public override bool GetPass()
    {
        return true;
    }

    public override bool OutlineIsRed()
    {
        float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, gameObject.transform.position);
        return (distance > range);
    }
}
