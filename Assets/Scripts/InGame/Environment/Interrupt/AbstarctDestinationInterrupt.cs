using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstarctDestinationInterrupt : MonoBehaviour {

    [Range(1, 10)]
    public int nbCondition = 1;

    private bool[] condition;

	void Awake () {
        condition = new bool[nbCondition];
	}
	
	public void ActiveCondition(bool p_active, int p_numCondition)
    {
        Debug.Log("active");
        condition[p_numCondition] = p_active;
        for(int i = 0; i < condition.Length; i++)
        {
            if (!condition[i])
            {
                return;
            }
        }
        UnlockTarget();
    }

    protected abstract void UnlockTarget();

}
