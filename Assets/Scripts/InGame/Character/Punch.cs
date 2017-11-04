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
	public bool RightPunch = false;

	bool canPunc = true;


    void Start()
    {
		control = GlobalManager.GameCont.Player.GetComponent<PlayerController>();
        barMadness = control.barMadness;
    }

    void OnTriggerEnter(Collider other)
    {
		if( canPunc && other.gameObject.tag == Constants._EnnemisTag )
        {
			AbstractObject tryGet = other.GetComponentInChildren<AbstractObject> ( );
			if ( !tryGet )
			{
				return;
			}
			Vector3 getProj = projection_basic;
            switch (numTechnic)
            {
			case (int)Technic.basic_punch:
				if ( RightPunch )
				{
					getProj.x *= Random.Range ( -getProj.x, -getProj.x / 2 );
				}
				else
				{
					getProj.x *= Random.Range ( getProj.x / 2, getProj.x );
				}

				tryGet.Degat ( getProj, numTechnic );
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
