using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    public int piece;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Constants._PlayerTag)
        {
           // AllPlayerPrefs.piece += piece;
			AllPlayerPrefs.SetIntValue ( Constants.Coin, piece );
            //animation
            Destroy(this.gameObject);
        }
    }
}
