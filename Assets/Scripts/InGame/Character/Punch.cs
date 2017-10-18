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
	[Tooltip ("X = force droite / gauche - Y = force haut / bas - Z = force Devant / derriere" )]
    public Vector3 projection_basic, projection_double;

    void OnTriggerEnter(Collider other)
    {
		if(other.gameObject.tag == Constants._EnnemisTag || other.gameObject.tag == Constants._ObsTag )
        {
            switch (numTechnic)
            {
			case (int)Technic.basic_punch:
				projection_basic.x *= Random.Range ( -projection_basic.x, projection_basic.x + 1 );
				other.GetComponentInChildren<AbstractObject> ( ).Degat ( projection_basic );
				break;
			case (int)Technic.double_punch:
				other.GetComponentInChildren<AbstractObject> ( ).Degat ( projection_double );
           	 	break;
            }
        }
    }

    public void setTechnic(int typeTech)
    {
        numTechnic = typeTech;
    }
}
