using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaMan : AbstractEnnemis {

    public float distanceMax = 30, distanceMin = 10;
    public float cooldownMin = 2, coolDownGeneral = 5;
    private float timer;
    public GameObject Missile;
    private MissileBazooka MissileScript;
    private Transform player;

	// Use this for initialization
	void Start () {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, player.position) <= distanceMax)
        {
            Debug.Log("temps = " + timer);
            if (Vector3.Distance(transform.position, player.position) <= distanceMin && timer >= cooldownMin)
            {
                MissileScript = Instantiate(Missile, transform.position + new Vector3(0, 0, -2), Quaternion.identity).GetComponent<MissileBazooka>();
                MissileScript.ActiveTir(-parentTrans.forward, 1, false);
                timer = 0;
            }
            else if (timer >= coolDownGeneral)
            {
                MissileScript = Instantiate(Missile, transform.position + new Vector3(0, 0, -2), Quaternion.identity).GetComponent<MissileBazooka>();
                MissileScript.ActiveTir(-parentTrans.forward, 1, false);
                timer = 0;
            }
            timer += Time.deltaTime;
        }
	}
}
