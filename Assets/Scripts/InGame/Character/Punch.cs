using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour {

    private enum Technic
    {
        basic_punch
    }

    private int numTechnic;
    public Vector3 degat_basic;

    void OnTriggerEnter(Collider other)
    {
		if(other.gameObject.tag == Constants._EnnemisTag)
        {
            switch (numTechnic)
            {
			case (int)Technic.basic_punch:
				if ( Random.Range ( 0, 2 ) == 0 )
				{
					degat_basic.x *= -1;
				}
				other.GetComponentInChildren<AbstractEnnemis>().Degat(degat_basic);
                    break;
            }
        }
    }

    public void setTechnic(int typeTech)
    {
        numTechnic = typeTech;
    }

}
