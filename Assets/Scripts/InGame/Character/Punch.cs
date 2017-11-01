using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Punch : MonoBehaviour {
    private Slider barMadness;
    public float addPointBarByPunch = 3;
    private PlayerController control;

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


    private void Awake()
    {
        control = GetComponentInParent<PlayerController>();
        barMadness = control.barMadness;
    }

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
				projection_basic.x *= Random.Range ( -projection_basic.x, projection_basic.x + 1 );

				tryGet.Degat ( projection_basic, numTechnic );
				break;
			case (int)Technic.double_punch:
				tryGet.Degat ( projection_double, numTechnic );
           	 	break;
            }
            MadnessMana();
        }else if (other.gameObject.tag == Constants._MissileBazoo)
        {
            other.gameObject.GetComponent<MissileBazooka>().ActiveTir(-other.gameObject.GetComponent<MissileBazooka>().GetDirection(), facteurVitesseRenvoie, true);
            MadnessMana();
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


    private void MadnessMana()
    {
        if (barMadness.value + addPointBarByPunch < barMadness.maxValue)
        {
            barMadness.value += addPointBarByPunch;
        }
        else
        {
            barMadness.value = barMadness.maxValue;
            control.SetInMadness(true);
        }
    }
}
