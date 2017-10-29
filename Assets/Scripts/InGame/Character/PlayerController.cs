﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
	#region Variables
	[Header ("Caractéristique sur une même Lane")]
	public float MaxSpeed = 5;
	public float Acceleration = 10;
	public float Deceleration = 1;

	[Tooltip ("Cooldown qui commence une fois le dash terminé")]
	public float CooldownDash = 3;

	[Header ("Caractéristique de changement de Lane")]
	public float MaxSpeedCL = 5;
	public float AccelerationCL = 10;
	public float ImpulsionCL = 10;
	public float DecelerationCL = 1;

	[Header ("Autres runner Caractéristiques ")]
	public float RotationSpeed = 1;
	public float SpeedEffectTime;
	public bool Running = true;
	[Tooltip ("Force bonus en plus de la gravitée")]
	public float BonusGrav = 0;
	[Tooltip ("Pourcentage de ralentissement du personnage dans les airs")]
	public float PourcRal = 50;

	//public float JumpForce = 200;
    [Header("Caractéristique Fight")]
    /*public float delayLeft = 1;
	public float delayRight = 1;*/
	public float DashTime = 2;
	[Tooltip ("La valeur de DashSpeed est un multiplicateur sur la vitesse du joueur")]
	public float DashSpeed = 2;

	[Header ("Slow Motion Caractéristique")]
	[Tooltip ("De combien la vitesse va diminuer au maximun par rapport à la vitesse standard")]
	public float SlowMotion = 1;
	[Tooltip ("Vitesse pour atteindre le slowMotion")]
	public float SpeedSlowMot = 1;
	[Tooltip ("Vitesse pour revenir à la vitesse normal")]
	public float SpeedDeacSM = 3;

	[Tooltip ("Vitesse de descente du slider content")]
	public float ReduceSlider;
	[Tooltip ("Vitesse de récupération du slider content")]
	public float RecovSlider;

	[Header ("Caractérique de temps sur les punchs")]
    public float delayPunch = 1;
	public float delayHitbox = 0.3f;
	public float delayPrepare = 0.1f;

	[Header ("Caractéristique de force des punchs")]
	public float PropulseBalls = 100;
	[Tooltip ("Le temps max sera delayPunch")]
	public float TimePropulsePunch = 0.1f, TimePropulseDoublePunch = 0.2f;
	[Tooltip ("La valeur est un multiplicateur sur la vitesse du joueur")]
	public float SpeedPunchRun = 1.2f, SpeedDoublePunchRun = 1.5f;

	public Vector3 DistPoingDroit = Vector3.zero;
	public Vector3 DistPoingGauche = Vector3.zero;
	public GameObject poingGauche;
	public GameObject poingDroite;

	[Header ("SphereMask")]
	public float Radius;
	public float SoftNess;

	[HideInInspector]
	public int NbrLineRight = 1;
	[HideInInspector]
	public int NbrLineLeft = 1;

	[HideInInspector]
	public bool playerDead = false;
	[HideInInspector]
	public bool Dash = false;

	public bool StopPlayer = false;

	private Collider punchBox;
	private Punch punch;
    private bool canPunch, punchRight;//, punchLeft, preparRight, preparLeft, defense;
	//private Coroutine corou/*, preparPunch*/;

	//Rigidbody thisRig;
	Transform pTrans;
	Rigidbody pRig;
	Direction currentDir = Direction.North;
	Direction newDir = Direction.North;
	//Vector3 posDir;
	Vector3 dirLine = Vector3.zero;
	IEnumerator currCouR;
	IEnumerator currCouL;
	IEnumerator propPunch;
	Punch getPunch;
	Camera thisCam;
	Slider SliderSlow;

	float currSpeed = 0;
	float currSpLine = 0;
	//float calPos = 0;
	float newH = 0;
	float newDist;
	float saveDist;
	float befRot = 0;
	float SliderContent;
	int currLine = 0;

	int LastImp = 0;
	int clDir = 0;

	//bool canJump = true;
	bool propP = false;
	bool propDP = false;
	bool newPos = false;
	bool resetAxeS = true;
	bool resetAxeD = true;
	bool canDash = true;
	bool inAir = false;
	bool canChange = true;
	#endregion

	#region Mono
	void Start ( )
	{
		pTrans = transform;
		pRig = gameObject.GetComponent<Rigidbody> ( );
		punchBox = pTrans.GetChild(0).GetComponent<Collider>();
		punch = pTrans.GetChild(0).GetComponent<Punch>();
        canPunch = true; 
		punchRight = true;
		getPunch = GetComponentInChildren<Punch> ( );
		thisCam = GetComponentInChildren<Camera> ( );
		SliderSlow = GlobalManager.Ui.MotionSlider;
		SliderContent = 10;
		SliderSlow.maxValue = 10;
        /* punchLeft = true; preparRight = false; preparLeft = false; defense = false;
		preparPunch = null;*/
    }

	void Update ( )
	{
		punch.CanPunch ( !playerDead );

		if ( !Dash && !playerDead )
		{
			if ( Input.GetAxis ( "CoupSimple" ) == 0 )
			{
				resetAxeS = true;
			}

			if ( Input.GetAxis ( "CoupDouble" ) == 0 )
			{
				resetAxeD = true;
			}

			playerFight ( );
		}

		if ( Input.GetAxis ( "Dash") != 0 && newH == 0 && canDash )
		{
			Dash = true;
			canDash = false;

			StartCoroutine ( waitStopDash ( ) );
		}

		if ( Input.GetAxis ( "SlowMot" ) > 0 && SliderContent > 0 )
		{
			if ( Time.timeScale > 1 / SlowMotion )
			{
				Time.timeScale -= Time.deltaTime * SpeedSlowMot;
			}

			SliderContent -= ReduceSlider * Time.deltaTime;
		}
		else if ( Time.timeScale < 1 )
		{
			if ( SliderContent < 0 )
			{
				SliderContent = 0;
			}

			Time.timeScale += Time.deltaTime * SpeedDeacSM;
		}
		else if ( SliderContent < 10 )
		{
			Time.timeScale = 1;
			SliderContent += RecovSlider * Time.deltaTime;
		}
		else
		{
			SliderContent = 10;
		}

		SliderSlow.value = SliderContent;

		Mathf.Clamp ( Radius, 0, 100 );
		Mathf.Clamp ( SoftNess, 0, 100 );

		Shader.SetGlobalVector ( "GlobaleMask_Position", new Vector4 ( pTrans.position.x, pTrans.position.y, pTrans.position.z, 0 ) );
		Shader.SetGlobalFloat ( "GlobaleMask_Radius", Radius );
		Shader.SetGlobalFloat ( "GlobaleMask_SoftNess", SoftNess );
		Shader.SetGlobalFloat ( "_SlowMot", Time.timeScale );
	}

	void FixedUpdate ( )
	{
		if ( playerDead || StopPlayer )
		{
			thisCam.fieldOfView = Constants.DefFov;
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

		checkInAir ( );

		changeLine ( deltTime );

		playerMove ( deltTime, currSpeed );
	}
	#endregion

	#region Public Functions
	#endregion

	#region Private Functions
	void checkInAir ( )
	{
		RaycastHit[] allHit;
		bool inAir = true;

		allHit = Physics.RaycastAll ( pTrans.position, Vector3.down, 5 );
		foreach ( RaycastHit thisRay in allHit )
		{
			if ( thisRay.collider.gameObject.layer == 9 )
			{
				inAir = false;

				Transform getThis = thisRay.collider.transform;

				if ( getThis.rotation.x < 0 )
				{
					pTrans.Translate ( new Vector3 ( 0, ( ( 360 - getThis.eulerAngles.x ) / 4  ) * Time.deltaTime, 0 ), Space.World );
					pRig.useGravity = false;
				}
				else if ( getThis.rotation.x > 0 )
				{
					pTrans.Translate ( new Vector3 ( 0, ( -getThis.eulerAngles.x / 4 ) * Time.deltaTime, 0 ), Space.World );
					pRig.useGravity = true;
				}

				inAir = false;
			}
		}

		if ( inAir )
		{
			inAir = true;
			pRig.useGravity = true;
			pRig.AddForce ( Vector3.down * BonusGrav, ForceMode.Acceleration );
		}
	}

	void playerMove ( float delTime, float speed )
	{
		Transform transPlayer = pTrans;
		Vector3 calTrans = Vector3.zero;
		delTime = Time.deltaTime;

        GlobalManager.Ui.CloseDashSpeed();

        if ( inAir )
		{
			speed = ( speed / 100 ) * PourcRal;
		}

		if ( Dash )
		{
			speed *= DashSpeed;

            GlobalManager.Ui.OpenDashSpeed();

        }
		else if ( propP )
		{
			speed *= SpeedPunchRun;
		}
		else if ( propDP )
		{
			speed *= SpeedDoublePunchRun;
		}

		float calCFov = Constants.DefFov * ( speed / MaxSpeed );

		Shader.SetGlobalFloat ( "_ReduceVis", speed / MaxSpeed);

		if ( thisCam.fieldOfView < calCFov)
		{
			thisCam.fieldOfView += Time.deltaTime * SpeedEffectTime;
		}
		else if ( thisCam.fieldOfView > calCFov)
		{
			thisCam.fieldOfView -= Time.deltaTime * SpeedEffectTime;
		}

		if ( currentDir == Direction.North )
		{
			calTrans = Vector3.forward * speed * delTime;
			transPlayer.rotation = Quaternion.Slerp ( transPlayer.rotation, Quaternion.Euler ( new Vector3 ( 0, 0, 0 ) ), RotationSpeed * delTime );
		}
		/*else if ( currentDir == Direction.South )
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
		}*/

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

		if ( ( canChange || newH == 0 ) && !Dash && !inAir )
		{
			if ( newImp == 1 && LastImp != 1 && currLine + 1 <= NbrLineRight && ( clDir == 1 || newH == 0 ) )
			{
				canChange = false;
				currLine++;
				LastImp = 1;
				clDir = 1;
				newH = newH + lineDistance;
				saveDist = newH;
			}
			else if ( newImp == -1 && LastImp != -1 && currLine - 1 >= -NbrLineLeft && ( clDir == -1 || newH == 0 ) )
			{
				canChange = false;
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
		}

		if ( newH != 0 )
		{
			if ( Running )
			{
				float accLine = 0;

				if ( saveDist < 0 && newH > -lineDistance / 2 || saveDist > 0 && newH < lineDistance / 2 )
				{
					currSpLine -= DecelerationCL * delTime;

					canChange = true;

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
		if(Input.GetAxis("CoupSimple") != 0 && canPunch && resetAxeS )
        {
			resetAxeS = false;
            canPunch = false;
			propP = true;

            ScreenShake.Singleton.ShakeHit();

           

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
			propPunch = propulsePunch ( TimePropulsePunch );
			StartCoroutine ( propPunch );
		}else if(Input.GetAxis("CoupDouble") != 0 && canPunch && resetAxeD )
        {
			propDP = true;
			resetAxeD = false;
            canPunch = false;
            poingDroite.SetActive(true);
            poingGauche.SetActive(true);
            StartCoroutine("StartPunch", 1);

			currCouL = animePunch ( false );
			currCouR = animePunch ( true );

			StartCoroutine ( currCouL );
			StartCoroutine ( currCouR );
			propPunch = propulsePunch ( TimePropulseDoublePunch );
			StartCoroutine ( propPunch );
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

		if ( propPunch != null )
		{
			StopCoroutine ( propPunch );
		}

		propP = false;
		propDP = false;
	}

	IEnumerator waitStopDash ( )
	{
		WaitForSeconds thisS = new WaitForSeconds ( DashTime );

		yield return thisS;

		Dash = false;

		StartCoroutine ( enableDash ( ) );
	}

	IEnumerator enableDash ( )
	{
		WaitForSeconds thisS = new WaitForSeconds ( CooldownDash );

		yield return thisS;

		canDash = true;
	}

	IEnumerator propulsePunch ( float thisTime )
	{
		WaitForSeconds thisSec = new WaitForSeconds ( thisTime );

		yield return thisSec;

		propP = false;
		propDP = false;
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
		GameObject getObj = thisColl.gameObject;

		if ( getObj.tag == Constants._Balls || getObj.tag == Constants._ElemDash )
		{
			if ( Dash )
			{
				if ( getObj.tag == Constants._ElemDash )
				{
					thisColl.gameObject.GetComponent<Rigidbody> ( ).AddForce ( getPunch.projection_double, ForceMode.VelocityChange );
				}
				else
				{
					StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( getObj, PropulseBalls, 1, 5, true ) );
				}

				return;
			}

			StartCoroutine ( GameOver ( ) );
		}
		else if ( getObj.tag == Constants._MissileBazoo )
		{
			getObj.GetComponent<MissileBazooka>().Explosion();
			StartCoroutine ( GameOver ( ) );
		}
		else if ( getObj.tag == Constants._EnnemisTag || getObj.tag == Constants._ObsTag || getObj.tag == Constants._MissileBazoo )
		{
			StartCoroutine ( GameOver ( ) );
		}
	}

	IEnumerator GameOver ( )
	{
		WaitForSeconds thisS = new WaitForSeconds ( 1 );
		playerDead = true;
		GlobalManager.Ui.OpenThisMenu ( MenuType.GameOver );

		yield return thisS;

		GlobalManager.GameCont.Restart ( );
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
