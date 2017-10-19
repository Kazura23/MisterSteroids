using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPoubelle : MonoBehaviour {

    public ShieldMan enemy;
    public Vector3 propulsion;

	public void onActive()
    {
        enemy.Degat(propulsion, 1);
    }
}
