using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Punch : MonoBehaviour {
    private Slider barMadness;
    public float addPointBarByPunchSimple = 3;
    public float addPointBarByPunchDouble = 5;
    public float puissanceOnde = 15;
    private PlayerController control;

    private enum Technic
    {
        basic_punch,
        double_punch,
        onde_choc
    }
		
    private int numTechnic;
	[Tooltip ("X = force droite / gauche - Y = force haut / bas - Z = force Devant / derriere" )]
    public Vector3 projection_basic, projection_double;
    public float facteurVitesseRenvoie = 1.5f;
	public bool RightPunch = false;

	bool canPunc = true;
	float pourcPunch = 100;
    void Start()
    {
		control = GlobalManager.GameCont.Player.GetComponent<PlayerController>();
        barMadness = control.BarMadness;
    }

    void OnTriggerEnter(Collider other)
    {
        if(numTechnic == (int)Technic.onde_choc)
        {
            switch (other.tag)
            {
                case Constants._EnnemisTag :
                    Vector3 dir = Vector3.Normalize(other.transform.position - transform.position);
                    AbstractObject enn = other.GetComponentInChildren<AbstractObject>();
                    if (!enn)
                    {
                        return;
                    }
                    enn.Degat(dir * puissanceOnde, (int)Technic.onde_choc);
                    break;
                case Constants._ObsPropSafe:
                    GlobalManager.GameCont.MeshDest.SplitMesh(other.gameObject, control.transform, 100, 3);
                    break;
                //case tag bibli
            }
        }
		else if( canPunc && ( other.gameObject.tag == Constants._EnnemisTag || other.gameObject.tag == Constants._ObsPropSafe))
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
				tryGet.Degat ( projection_double * pourcPunch, numTechnic );
           	 	break;
            }
            MadnessMana("Double");
        }else if (other.gameObject.tag == Constants._MissileBazoo)
        {
            other.gameObject.GetComponent<MissileBazooka>().ActiveTir(-other.gameObject.GetComponent<MissileBazooka>().GetDirection(), facteurVitesseRenvoie, true);
            MadnessMana("Double");
        }
    }

	public void setTechnic(int typeTech, float pourc = 100 )
    {
		pourcPunch = pourc;
        numTechnic = typeTech;
    }

	public void SetPunch ( bool canPush )
	{
		canPunc = canPush;
	}

    public void MadnessMana(string type)
    {
        //if (!control.IsInMadness()) {
            if (/*barMadness.value + addPointBarByPunchSimple < barMadness.maxValue &&*/ type == "Simple")
            {
                //barMadness.value += addPointBarByPunchSimple;
                control.AddSmoothCurve(addPointBarByPunchSimple);
            } else if (/*barMadness.value + addPointBarByPunchDouble < barMadness.maxValue &&*/ type == "Double")
            {
                //barMadness.value += addPointBarByPunchDouble;
                control.AddSmoothCurve(addPointBarByPunchDouble);
            }
            /*else
            {
                barMadness.value = barMadness.maxValue;
                control.SetInMadness(true);
            }*/
        //}
    }
}
