using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour {

    private enum Technic
    {
        basic_punch
    }

    private int numTechnic;
    public int degat_basic = 1;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "enemy")
        {
            switch (numTechnic)
            {
                case (int)Technic.basic_punch:
                    other.GetComponent<EnemySimpleController>().Degat(degat_basic);
                    break;
            }
        }
    }

    public void setTechnic(int typeTech)
    {
        numTechnic = typeTech;
    }

}
