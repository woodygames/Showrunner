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
}
