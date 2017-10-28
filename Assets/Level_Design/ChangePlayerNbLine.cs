using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerNbLine : MonoBehaviour {

	public int NbrLineLeft;
	public int NbrLineRigh;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			other.gameObject.GetComponent<PlayerController>().NbrLineLeft = NbrLineLeft ;
			other.gameObject.GetComponent<PlayerController>().NbrLineRight = NbrLineRigh ;
		}
	}
}
