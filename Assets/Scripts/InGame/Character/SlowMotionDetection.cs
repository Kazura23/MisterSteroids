using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionDetection : MonoBehaviour {

    public float ratioSlow = 0.3f;
    public float timeSlow = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Constants._EnnemisTag)
        {
            Time.timeScale = ratioSlow;
            StartCoroutine("delaySlowMotio");
        }
    }


    private IEnumerator delaySlowMotio()
    {
        yield return new WaitForSeconds(timeSlow);
        if (Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }
    }

}
