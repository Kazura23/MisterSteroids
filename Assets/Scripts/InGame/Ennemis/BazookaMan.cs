using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaMan : AbstractObject {

    public float distanceMax = 30, distanceMin = 10;
    public float cooldownMin = 2, coolDownGeneral = 5;
    private float timer;
    public GameObject Missile;
    private MissileBazooka MissileScript;
    private Transform player;
	Transform localShoot;
	// Use this for initialization
	protected override void Start()
	{
        timer = 0;
		player = GlobalManager.GameCont.Player.transform;
		localShoot = getTrans.Find ( "SpawnShoot" );
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, player.position) <= distanceMax)
        {
            Debug.Log("temps = " + timer);
            if (Vector3.Distance(transform.position, player.position) <= distanceMin && timer >= cooldownMin)
            {
				MissileScript = Instantiate ( Missile, localShoot ).GetComponent<MissileBazooka> ( );
				MissileScript.ActiveTir(-getTrans.forward, 1, false);
                timer = 0;
            }
            else if (timer >= coolDownGeneral)
            {
				MissileScript = Instantiate(Missile, localShoot).GetComponent<MissileBazooka>();
				MissileScript.ActiveTir(-getTrans.forward, 1, false);
                timer = 0;
            }
            timer += Time.deltaTime;
        }
	}

	protected override void OnCollisionEnter ( Collision thisColl )
	{
		if ( thisColl.gameObject.tag == Constants._PlayerTag && thisColl.gameObject.GetComponent<PlayerController> ( ).Dash )
		{
			CollDetect ( );
		}
		else
		{
			base.OnCollisionEnter ( thisColl );
		}
	}
}
