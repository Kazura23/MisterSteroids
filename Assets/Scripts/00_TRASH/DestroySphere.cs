using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySphere : AbstarctDestinationInterrupt
{

    protected override void UnlockTarget()
    {
        Destroy(this.gameObject);
    }
}
