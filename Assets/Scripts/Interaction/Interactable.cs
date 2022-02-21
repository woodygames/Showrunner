using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    /// <summary>
    /// Observer-list
    /// </summary>
    protected LinkedList<Observer> observers = new LinkedList<Observer>();

    /// <summary>
    /// Maximum range distance from player to interactable
    /// </summary>
    [SerializeField]
    protected float range = 5.0f;

    /// <summary>
    /// Add an Observer observing the interactables state to the observer-list.
    /// </summary>
    /// <param name="o">Observer</param>
    public void SetObserver(Observer o)
    {
        observers.AddLast(o);
    }

    /// <summary>
    /// Notifies all observers in the observer-list.
    /// </summary>
    protected void NotifyAllObservers()
    {
        foreach(Observer o in observers)
        {
            o?.Notify();
        }
    }

    public abstract bool OutlineIsRed();

    /// <summary>
    /// Called by clicking on the interactable.
    /// </summary>
    public abstract void Trigger();
   
    /// <summary>
    /// 
    /// </summary>
    /// <returns>player is allowed to pass further in the game</returns>
    public abstract bool GetPass();
}
