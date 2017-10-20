using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerNbLine : MonoBehaviour {

	public int nbLine ;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerController>().NbrLine = nbLine ;
		}
	}
}
