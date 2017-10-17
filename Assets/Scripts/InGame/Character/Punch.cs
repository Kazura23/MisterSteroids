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
<<<<<<< HEAD:Assets/Scripts/InGame/Character/Punch.cs
			case (int)Technic.basic_punch:
				if ( Random.Range ( 0, 2 ) == 0 )
				{
					degat_basic.x *= -1;
				}
				other.GetComponentInChildren<AbstractEnnemis>().Degat(degat_basic);
=======
                case (int)Technic.basic_punch:
                    other.GetComponent<EnemySimpleController>().Degat(projection_basic);
                    break;
                case (int)Technic.double_punch:
                    other.GetComponent<EnemySimpleController>().Degat(projection_double);
>>>>>>> Eric:Assets/script/Punch.cs
                    break;
            }
        }
    }

    public void setTechnic(int typeTech)
    {
        numTechnic = typeTech;
    }

}
