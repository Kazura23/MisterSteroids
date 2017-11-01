﻿using System.Collections;
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
    public float facteurVitesseRenvoie = 1.5f;

	bool canPunc = true;

    void OnTriggerEnter(Collider other)
    {
		if( canPunc && ( other.gameObject.tag == Constants._EnnemisTag || other.gameObject.tag == Constants._ObsTag) )
        {
			AbstractObject tryGet = other.GetComponentInChildren<AbstractObject> ( );
			if ( !tryGet )
			{
				return;
			}

            switch (numTechnic)
            {
			case (int)Technic.basic_punch:
				//projection_basic.x *= Random.Range ( -projection_basic.x, projection_basic.x + 1 );

				tryGet.Degat ( projection_basic, numTechnic );
				break;
			case (int)Technic.double_punch:
				tryGet.Degat ( projection_double, numTechnic );
           	 	break;
            }
        }else if (other.gameObject.tag == Constants._MissileBazoo)
        {
            other.gameObject.GetComponent<MissileBazooka>().ActiveTir(-other.gameObject.GetComponent<MissileBazooka>().GetDirection(), facteurVitesseRenvoie, true);
        }
    }

    public void setTechnic(int typeTech)
    {
        numTechnic = typeTech;
    }

	public void SetPunch ( bool canPush )
	{
		canPunc = canPush;
	}
}
