using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour {

   
    private float width;

    void Start()
    {
        /*width = GetComponent<Renderer>().material.GetFloat("OutlineThickness");
        Debug.Log(width + "Thickness");
        GetComponent<Renderer>().material.SetFloat("OutlineThickness", 0.0f);*/
    }

    public void SetOutline(bool b)
    {
         /*Debug.Log("Setting Outline Width :" + b);
         if (b)
             GetComponent<Renderer>().material.SetFloat("OutlineThickness", width);
         else
             GetComponent<Renderer>().material.SetFloat("OutlineThickness", 0.0f);*/
    }
}
