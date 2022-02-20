using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{


    private int layer;

    [SerializeField]
    private bool outlineChildren;

    void Start()
    {
        layer = gameObject.layer;
        SetOutline(false);
        Debug.Log(layer + "Thickness");
        // GetComponent<Renderer>().material.SetFloat("OutlineThickness", 0.0f);*/
    }

    public void SetOutline(bool b)
    {
        Debug.Log("Setting Outline ");
        if (b)
        {
           Camera.main.GetComponent<HighLightController>().SetOutlineRed(gameObject.GetComponent<Interactable>().OutlineIsRed());
            gameObject.layer = layer;
            if (!outlineChildren) return;
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.layer = layer;
            }
        }

        else
        {
            gameObject.layer = 6;
            if (!outlineChildren) return;
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.layer = 6;
            }
        }

    }
}
