using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	#region Variables
	[Header ("Caract on same Line")]
	public float MaxSpeed = 5;
	public float Acceleration = 10;
	public float Deceleration = 1;

	[Header ("Caract when change Line")]
	public float MaxSpeedCL = 5;
	public float AccelerationCL = 10;
	public float ImpulsionCL = 10;
	public float DecelerationCL = 1;

	[Header ("Caract both")]
	public float RotationSpeed = 1;
	public bool Running = true;
	//public float JumpForce = 200;

	[Space]
	[Header("Nombre de ligne bonus d'un seul coté")]
	public int NbrLine = 3;

    [Header("Caract Fight")]
    /*public float delayLeft = 1;
	public float delayRight = 1;*/
    public float delayPunch = 1;
	public float delayHitbox = 0.3f;
	public float delayPrepare = 0.1f;
	public Vector3 DistPoingDroit = Vector3.zero;
	public Vector3 DistPoingGauche = Vector3.zero;
	public GameObject poingGauche;
	public GameObject poingDroite;

	[HideInInspector]
	public bool playerDead = false;

	private Collider punchBox;
	private Punch punch;
    private bool canPunch, punchRight;//, punchLeft, preparRight, preparLeft, defense;
	//private Coroutine corou/*, preparPunch*/;

	//Rigidbody thisRig;
	Transform pTrans;
	Direction currentDir = Direction.North;
	Direction newDir = Direction.North;
	//Vector3 posDir;
	Vector3 dirLine = Vector3.zero;
	Collider currCol;
	IEnumerator currCouR;
	IEnumerator currCouL;

	float currSpeed = 0;
	float currSpLine = 0;
	//float calPos = 0;
	float newH = 0;
	float newDist;
	float saveDist;
	float befRot = 0;
	int currLine = 0;

	int LastImp = 0;
	int clDir = 0;

	//bool canJump = true;
	bool newPos = false;
	#endregion

	#region Mono
	void Awake ( )
	{
		pTrans = transform;
		currCol = GetComponent<Collider> ( );
		punchBox = pTrans.GetChild(0).GetComponent<Collider>();
		punch = pTrans.GetChild(0).GetComponent<Punch>();
        canPunch = true; punchRight = true;
        /* punchLeft = true; preparRight = false; preparLeft = false; defense = false;
		preparPunch = null;*/
    }

	void Update ( )
	{
		playerFight ( );
	}

	void FixedUpdate ( )
	{
		if ( playerDead )
		{
			return;
		}

		/*if ( newPos )
		{
			float getCurr = Vector3.Distance ( posDir, pTrans.position );
			if ( newDist != getCurr )
			{
				newDist = getCurr;

				if ( calPos > newDist )
				{
					calPos = newDist;
				}
				else
				{
					currentDir = newDir;
					pTrans.position = new Vector3 ( posDir.x, pTrans.position.y, posDir.z );
					calPos = 0;
					newPos = false;
				}
			}
		}*/

		float deltTime = Time.deltaTime;

		if ( Running )
		{
			if ( currSpeed < MaxSpeed )
			{
				currSpeed += Acceleration * deltTime;
			}
			else if ( currSpeed > MaxSpeed )
			{
				currSpeed = MaxSpeed;
			}
		}
		else
		{
			currSpeed -= Deceleration * deltTime;

			if ( currSpeed < 0 )
			{
				currSpeed = 0;
			}

			currSpLine -= Deceleration * deltTime;

			if ( currSpLine < 0 )
			{
				currSpLine = 0;
			}
		}

		changeLine ( deltTime );
		playerMove ( deltTime, currSpeed );
	}
	#endregion

	#region Public Functions
	#endregion

	#region Private Functions
	void playerMove ( float delTime, float speed )
	{
		Transform transPlayer = pTrans;
		Vector3 calTrans = Vector3.zero;
		delTime = Time.deltaTime;

		if ( currentDir == Direction.North )
		{
			calTrans = Vector3.forward * speed * delTime;
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, 0, 0 ) ), RotationSpeed * delTime );
		}
		else if ( currentDir == Direction.South )
		{
			calTrans = Vector3.back  * speed * delTime;
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, 180, 0 ) ), RotationSpeed * delTime );
		}
		else if ( currentDir == Direction.East )
		{
			calTrans = Vector3.right * speed * delTime;
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, 90, 0 ) ), RotationSpeed * delTime );
		}
		else if ( currentDir == Direction.West )
		{
			calTrans = Vector3.left * speed * delTime;
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, -90, 0 ) ), RotationSpeed * delTime );
		}

		if ( newPos )
		{
			befRot -= calTrans.magnitude;

			if ( befRot < 0 )
			{
				newPos = false;
				currentDir = newDir;
				pTrans.Translate ( pTrans.forward * befRot * delTime, Space.World );
			}
		}

		pTrans.Translate ( calTrans, Space.World );
			
		/*if ( canJump && Input.GetAxis ( "Jump" ) > 0 )
		{
			canJump = false;
			thisRig.AddForce ( transPlayer.up * JumpForce, ForceMode.Impulse );
		}*/
	}

	void changeLine ( float delTime )
	{
		float newImp = Input.GetAxis ( "Horizontal" );
		float lineDistance = Constants.LineDist;

		if ( newImp == 1 && LastImp != 1 && currLine + 1 <= NbrLine && ( clDir == 1 || newH == 0 ) )
		{
			currLine++;
			LastImp = 1;
			clDir = 1;
			newH = newH +lineDistance;
			saveDist = newH;
		}
		else if ( newImp == -1 && LastImp != -1 && currLine - 1 >= -NbrLine && ( clDir == -1 || newH == 0 ) )
		{
			currLine--;
			LastImp = -1;
			clDir = -1;
			newH = newH - lineDistance;
			saveDist = newH;
		}
		else if ( newImp == 0 )
		{
			LastImp = 0;
		}

		if ( newH != 0 )
		{
			if ( Running )
			{
				float accLine = 0;

				if ( saveDist < 0 && newH > -lineDistance / 2 || saveDist > 0 && newH < lineDistance / 2 )
				{
					currSpLine -= DecelerationCL * delTime;

					if ( currSpLine < 0 )
					{
						currSpLine = 0;
					}
				}
				else if ( currSpLine < MaxSpeedCL )
				{
					accLine = ( currSpLine * ImpulsionCL )/ MaxSpeedCL; 

					if ( accLine > 1 || accLine == 0 )
					{
						accLine = 1;
					}

					currSpLine += AccelerationCL * accLine * delTime;
				}
				else if ( currSpLine > MaxSpeedCL )
				{
					currSpLine = MaxSpeedCL;
				}
			}

			float calTrans = clDir * currSpLine * delTime;
			newH -= calTrans;

			if ( saveDist > 0 && newH - calTrans < 0 || saveDist < 0 && newH > 0 )
			{
				calTrans += newH;
				newH = 0;
				currSpLine = 0;
			}

			dirLine = pTrans.right * calTrans;
			pTrans.Translate ( dirLine, Space.World );
		}
		else
		{
			currSpLine = 0;
		}
	}

	void playerFight ( )
	{
		if(Input.GetKeyDown(KeyCode.A) && canPunch)
        {
            canPunch = false;
            if (punchRight)
            {
                poingDroite.SetActive(true);

				if ( currCouR != null )
				{
					StopCoroutine ( currCouR );
				}
				currCouR = animePunch ( true );
				StartCoroutine ( currCouR );
            }
            else
            {
                poingGauche.SetActive(true);

				if ( currCouL != null )
				{
					StopCoroutine ( currCouL );
				}
				currCouL = animePunch ( false );
				StartCoroutine ( currCouL );
            }
            punchRight = !punchRight;
            StartCoroutine("StartPunch", 0);
        }else if(Input.GetKeyDown(KeyCode.E) && canPunch)
        {
            canPunch = false;
            poingDroite.SetActive(true);
            poingGauche.SetActive(true);
            StartCoroutine("StartPunch", 1);

			currCouL = animePunch ( false );
			currCouR = animePunch ( true );

			StartCoroutine ( currCouL );
			StartCoroutine ( currCouR );
        }
         
        
        
        
        //en stock
        /*if (Input.GetKeyDown(KeyCode.A) && punchLeft)
		{
			preparLeft = true;
			poingGauche.SetActive (true);

			if(preparPunch == null)
			{
				preparPunch = StartCoroutine("StartPunch");
			}

			StartCoroutine ( animePunch ( true ) );
		}
		if (Input.GetKeyDown(KeyCode.E) && punchRight)
		{
			preparRight = true;
			poingDroite.SetActive (true);

			if (preparPunch == null)
			{
				preparPunch = StartCoroutine("StartPunch");
			}
		}*/

		/*
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
		 */
	}

	private IEnumerator StartPunch(int type_technic)
	{
		yield return new WaitForSeconds(delayPrepare);
        punch.setTechnic(type_technic);
        punchBox.enabled = true;
       /* corou =*/ StartCoroutine("TimerHitbox");
        StartCoroutine("CooldownPunch");

        // en stock
		/*if (preparRight && preparLeft)
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
			corou = StartCoroutine("TimerHitbox");*/
			/*StartCoroutine("CooldownLeft");
            StartCoroutine("CooldownRight");*/
		/*}else if (preparLeft)
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
		preparPunch = null;*/
	}


    private IEnumerator CooldownPunch()
    {
        yield return new WaitForSeconds(delayPunch);
        if (poingDroite.activeInHierarchy)
        {
            poingDroite.SetActive(false);
        }
        if (poingGauche.activeInHierarchy)
        {
            poingGauche.SetActive(false);
        }
        canPunch = true;
    }
    //en stock
	/*private IEnumerator CooldownLeft()
	{
		yield return new WaitForSeconds(delayLeft);
		poingGauche.SetActive (false);
		punchLeft = true;
	}

	private IEnumerator CooldownRight()
	{
		yield return new WaitForSeconds(delayRight);
		poingDroite.SetActive (false);
		punchRight = true;
	}*/

	private IEnumerator TimerHitbox()
	{
		yield return new WaitForSeconds(delayHitbox);
		punchBox.enabled = false;
		//corou = null;
	}

	IEnumerator animePunch ( bool rightPoing )
	{
		WaitForEndOfFrame thisFrame = new WaitForEndOfFrame ( );
		Transform thisPoing;
		Vector3 getStart;
		Vector3 thisDist;

		float getTime = delayPunch / 2;
		float currTime = 0;

		if ( rightPoing )
		{
			thisPoing = poingDroite.transform;	
			thisDist = DistPoingDroit;
		}
		else
		{
			thisPoing = poingGauche.transform;
			thisDist = DistPoingGauche;
		}

		getStart = thisPoing.localPosition;

		do 
		{
			yield return thisFrame;

			currTime += Time.deltaTime;
			thisPoing.localPosition = getStart + thisDist * ( currTime / getTime);
		}while ( currTime < getTime );

		do 
		{
			yield return thisFrame;

			currTime -= Time.deltaTime;
			thisPoing.localPosition = getStart + thisDist * ( currTime / getTime);
		}while ( currTime > 0 );

		thisPoing.localPosition = getStart;

		if ( rightPoing )
		{
			currCouR = null;
		}
		else
		{
			currCouL = null;
		}
	}

	/* public bool IsDefense()
    {
        return defense;
    }*/

	void OnTriggerEnter ( Collider thisColl )
	{
		if ( thisColl.tag == Constants._NewDirec )
		{
			//posDir = thisColl.transform.position;
			//befRot = thisColl.GetComponent<BoxCollider> ( ).size.z / 2 + pTrans.GetComponent<BoxCollider> ( ).size.z ;

			newPos = true;
			newDir = thisColl.GetComponent<NewDirect> ( ).NewDirection;
			befRot = Vector3.Distance ( thisColl.transform.position, pTrans.position );
		} 
	}

	void OnCollisionEnter ( Collision thisColl )
	{
		if ( thisColl.gameObject.tag == Constants._EnnemisTag || thisColl.gameObject.tag == Constants._ObsTag || thisColl.gameObject.tag == Constants._MissileBazoo )
		{
            if(thisColl.gameObject.tag == Constants._MissileBazoo)
            {
                thisColl.gameObject.GetComponent<MissileBazooka>().Explosion();
            }
			playerDead = true;
			GlobalManager.Ui.DisplayOver ( true );

			GlobalManager.GameCont.Restart ( );

		}
		else if ( thisColl.gameObject.tag == Constants._DebrisEnv )
		{
			Physics.IgnoreCollision ( thisColl.collider, currCol );
		}
	}


	/*void OnCollisionStay ( Collision thisColl )
	{
		if ( thisColl.gameObject.layer == 9 )
		{
			canJump = true;
		}
	}*/
	#endregion
}
