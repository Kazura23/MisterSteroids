using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchInterrupt : AbstractInterrupt {

    private bool active = false;
    public float vitesse = 8;

	public void Activation()
    {
        active = true;
        Active();
    }
	
	private void Update()
    {
        if (active)
        {
            if(transform.localRotation.eulerAngles.y < 75)
            {
                transform.Rotate(new Vector3(Time.deltaTime * vitesse, 0, 0));
            }
        }
    }
}
