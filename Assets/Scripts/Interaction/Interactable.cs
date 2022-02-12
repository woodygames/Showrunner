using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private LinkedList<Observer> observers = new LinkedList<Observer>();

   
    public void SetObserver(Observer o)
    {
        observers.AddLast(o);
    }

    protected void NotifyAllObservers()
    {
        foreach(Observer o in observers)
        {
            o?.Notify();
        }
    }

    public abstract void Trigger();
   
    public abstract bool GetPass();
}
