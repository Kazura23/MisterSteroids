using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerEric : MonoBehaviour {

    
    private RaycastHit hit;
    private Collider punchBox;
    private Punch punch;
    private bool punchRight, punchLeft, preparRight, preparLeft, defense;
    public float delayLeft = 1, delayRight = 1, delayHitbox = 0.3f, delayPrepare = 0.1f;
    private Coroutine corou, preparPunch;
    public int hp = 10;


	// Use this for initialization
	void Start () {
        punchBox = transform.GetChild(0).GetComponent<Collider>();
        punch = transform.GetChild(0).GetComponent<Punch>();
        punchRight = true; punchLeft = true; preparRight = false; preparLeft = false; defense = false;
        preparPunch = null;
        
    }
	
	// Update is called once per frame
	void Update () {
        /*if(Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.E) && punchLeft && punchRight)
        {
            punch.setTechnic(1);
            punchBox.enabled = true;
            punchLeft = false;
            punchRight = false;
            if (corou != null)
            {
                punchBox.enabled = false;
                StopCoroutine(corou);
                punchBox.enabled = true;
            }
            corou = StartCoroutine("TimerHitbox");
            StartCoroutine("CooldownLeft");
            StartCoroutine("CooldownRight");
        }*/
        if (Input.GetKeyDown(KeyCode.A) && punchLeft)
        {
            preparLeft = true;
            if(preparPunch == null)
            {
                preparPunch = StartCoroutine("StartPunch");
            }
            //animation

            /*punch.setTechnic(0);
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
            //animation poing gauche*/
        }
        if (Input.GetKeyDown(KeyCode.E) && punchRight)
        {
            preparRight = true;
            if (preparPunch == null)
            {
                preparPunch = StartCoroutine("StartPunch");
            }
            //animation

            /*punch.setTechnic(0);
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
            //animation poing droit*/
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

    private IEnumerator StartPunch()
    {
        yield return new WaitForSeconds(delayPrepare);
        if (preparRight && preparLeft)
        {
            punch.setTechnic(1);
            punchBox.enabled = true;
            punchLeft = false;
            punchRight = false;
            if (corou != null)
            {
                punchBox.enabled = false;
                StopCoroutine(corou);
                punchBox.enabled = true;
            }
            corou = StartCoroutine("TimerHitbox");
            /*StartCoroutine("CooldownLeft");
            StartCoroutine("CooldownRight");*/
        }else if (preparLeft)
        {
            punchLeft = false;
            punch.setTechnic(0);
            punchBox.enabled = true;
            //bool
            if (corou != null)
            {
                punchBox.enabled = false;
                StopCoroutine(corou);
                punchBox.enabled = true;
            }
            corou = StartCoroutine("TimerHitbox");
            //StartCoroutine("CooldownLeft");
            
        }else if (preparRight)
        {
            punchRight = false;
            punch.setTechnic(0);
            punchBox.enabled = true;
            // bool
            if (corou != null)
            {
                punchBox.enabled = false;
                StopCoroutine(corou);
                punchBox.enabled = true;
            }
            corou = StartCoroutine("TimerHitbox");
            //StartCoroutine("CooldownRight");
            
        }
        if (preparLeft)
        {
            preparLeft = false;
            StartCoroutine("CooldownLeft");
        }
        if (preparRight)
        {
            preparRight = false;
            StartCoroutine("CooldownRight");
        }
        preparPunch = null;
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
