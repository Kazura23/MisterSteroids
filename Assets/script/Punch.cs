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
        if(other.gameObject.tag == "enemy")
        {
            switch (numTechnic)
            {
                case (int)Technic.basic_punch:
                    other.GetComponent<EnemySimpleController>().Degat(projection_basic);
                    break;
                case (int)Technic.double_punch:
                    other.GetComponent<EnemySimpleController>().Degat(projection_double);
                    break;
            }
        }
    }

    public void setTechnic(int typeTech)
    {
        numTechnic = typeTech;
    }

}
