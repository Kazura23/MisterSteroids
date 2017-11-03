using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Piece : MonoBehaviour {

    public int piece;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Constants._PlayerTag)
        {
            // AllPlayerPrefs.piece += piece;

            Physics.IgnoreCollision(this.GetComponent<Collider>(), other.GetComponent<Collider>());

           
            AllPlayerPrefs.SetIntValue ( Constants.Coin, piece );

            transform.DOLocalRotate(new Vector3(0, 2000, 0),1f,RotateMode.FastBeyond360);

            Destroy(this.gameObject, 1f);
            
        }
    }
}
