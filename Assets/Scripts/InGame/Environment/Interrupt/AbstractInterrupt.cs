using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractInterrupt : MonoBehaviour {

    public AbstarctDestinationInterrupt dest;
    private bool etat;

    [Range(0, 9)]
    public int numCondition;

    protected virtual void Awake()
    {
        etat = false;
    }
	
    protected void Active()
    {
        dest.ActiveCondition((etat = !etat), numCondition); // a voir si reactivable
    }


}
