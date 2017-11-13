using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : AbstarctDestinationInterrupt {

    public float vitesse = 8;
    private bool active = false;

    protected override void UnlockTarget()
    {
        active = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            if (transform.localRotation.eulerAngles.y < 180)
            {
                transform.Rotate(new Vector3(0, Time.deltaTime * vitesse, 0));
            }
        }
	}
}
