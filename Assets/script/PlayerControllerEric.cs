using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerEric : MonoBehaviour {

    
    private RaycastHit hit;
    private Collider punchBox;
    private Punch punch;
    private bool punchRight, punchLeft, defense;
    public float delayLeft = 1, delayRight = 1, delayHitbox = 0.3f;
    private Coroutine corou;
    public int hp = 10;


	// Use this for initialization
	void Start () {
        punchBox = transform.GetChild(0).GetComponent<Collider>();
        punch = transform.GetChild(0).GetComponent<Punch>();
        punchRight = true; punchLeft = true; defense = false;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A) && punchLeft)
        {
            Debug.Log("Frappegauch");

            punch.setTechnic(0);
            punchBox.enabled = true;
            punchLeft = false;
            if(corou != null)
            {
                punchBox.enabled = false;
                StopCoroutine(corou);
                punchBox.enabled = true;
            }
            corou = StartCoroutine("TimerHitbox");
            StartCoroutine("CooldownLeft");
            //animation poing gauche
        }
        if (Input.GetKeyDown(KeyCode.E) && punchRight)
        {
            punch.setTechnic(0);
            punchBox.enabled = true;
            punchRight = false;
            if (corou != null)
            {
                punchBox.enabled = false;
                StopCoroutine(corou);
                punchBox.enabled = true;
            }
            corou = StartCoroutine("TimerHitbox");
            StartCoroutine("CooldownRight");
            //animation poing droit
        }
        if (Input.GetKey(KeyCode.R) && punchLeft && punchRight)
        {
            defense = true;
            //ajout animation defense active
        }
        if (Input.GetKeyUp(KeyCode.R) || punchLeft || punchRight)
        {
            defense = false;
            //ajout animation defense desactive
        }
    }


    private IEnumerator CooldownLeft()
    {
        yield return new WaitForSeconds(delayLeft);
        punchLeft = true;
    }

    private IEnumerator CooldownRight()
    {
        yield return new WaitForSeconds(delayRight);
        punchRight = true;
    }

    private IEnumerator TimerHitbox()
    {
        yield return new WaitForSeconds(delayHitbox);
        punchBox.enabled = false;
        corou = null;
    }

    public void Degat(int p_damage)
    {
        hp -= p_damage;
    }

    public bool IsDefense()
    {
        return defense;
    }
}
