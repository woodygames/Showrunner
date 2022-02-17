using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Observer
{
    /// <summary>
    /// Method is call from the observed object when its state changes
    /// </summary>
    public void Notify();
}
