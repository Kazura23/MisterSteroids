using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour {

    private enum Technic
    {
        basic_punch,
        double_punch
    }

    private int numTechnic;
    public Vector3 projection_basic, projection_double;

    void OnTriggerEnter(Collider other)
    {
		if(other.gameObject.tag == Constants._EnnemisTag)
        {
            switch (numTechnic)
            {
			case (int)Technic.basic_punch:
				if ( Random.Range ( 0, 2 ) == 0 )
				{
					projection_basic.x *= -1;
				}

				// 	projection_basic.x *= Random.Range ( -projection_basic.x, projection_basic.x + 1 );

				other.GetComponentInChildren<AbstractEnnemis>().Degat(projection_basic, numTechnic);
				break;
            case (int)Technic.double_punch:
				other.GetComponentInChildren<AbstractEnnemis>().Degat(projection_double, numTechnic);
           	 	break;
            }
        }
    }

    public void setTechnic(int typeTech)
    {
        numTechnic = typeTech;
    }

}
