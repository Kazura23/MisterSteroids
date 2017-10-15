using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleController : MonoBehaviour {

    public float speed = 8, cooldownPunch = 2, distanceMin = 2, delayDead = 2;
    public int hp = 1, degat = 1;
    private Transform PlayerPos;
    private PlayerController Player;
    private Rigidbody rigi;
    private Vector3 reff, move, moveCalcul;
    private float distance;
    private bool punchReady, isDead;

	// Use this for initialization
	void Start () {
        PlayerPos = GameObject.Find("Player").transform;
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        rigi = GetComponent<Rigidbody>();
        reff = new Vector3(3, 0, 3);
        move = new Vector3(); moveCalcul = new Vector3();
        punchReady = false; isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (hp <= 0)
        {
            isDead = true;
            StartCoroutine("Dead");
        }
        else if(!isDead)
        {
            if (Vector3.Distance(Player.transform.position, transform.position) > distanceMin)
            {
                moveCalcul.Set(Player.transform.position.x - transform.position.x, 0, Player.transform.position.z - transform.position.z);
                distance = Vector3.Distance(Player.transform.position, transform.position) / Vector3.Distance(reff, Vector3.zero);

                move.Set(transform.position.x + ((moveCalcul.x / distance) * speed * Time.deltaTime), 0, transform.position.z + ((moveCalcul.z / distance) * speed * Time.deltaTime));
                rigi.MovePosition(move);
            }
            else
            {
                if (punchReady)
                {
                    punchReady = false;
                    if (Player.IsDefense())
                    {
                        // bruitage de defense
                    }
                    else
                    {
                        Player.Degat(degat);
                        //bruitage succes punch
                    }
                    //animation punch
                }
                else
                {
                    StartCoroutine("ReloadPunch");
                }
            }
        }
        
	}

    private IEnumerator ReloadPunch()
    {
        yield return new WaitForSeconds(cooldownPunch);
        punchReady = true;
    }

    public void Degat(int p_damage)
    {
        hp -= p_damage;
    }

    public IEnumerator Dead()
    {
        //animation dead
        yield return new WaitForSeconds(delayDead);
        Destroy(this.gameObject);
    }
}
