using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPoubelle : MonoBehaviour {

    public ShieldMan enemy;
    public Vector3 propulsion, prop2;
    private GameObject missi;

	public void onActive()
    {
        GameObject.Find("ShieldMan").GetComponent<ShieldMan>().Degat(propulsion, 1);
        /*missi = GameObject.FindGameObjectWithTag(Constants._MissileBazoo);
        missi.GetComponent<MissileBazooka>().ActiveTir(-missi.GetComponent<MissileBazooka>().GetDirection(), 1.5f, true);*/
    }


    public void onActive2()
    {
        GameObject.Find("ShieldMan").GetComponent<ShieldMan>().Degat(prop2, 0);
    }

}
