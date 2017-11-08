using System.Collections;
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

	[Header ("Caractéristique de changement de Lane")]
	public float MaxSpeedCL = 5;
	public float AccelerationCL = 10;
	public float ImpulsionCL = 10;
	public float DecelerationCL = 1;

	[Header ("Autres runner Caractéristiques ")]
	public float RotationSpeed = 1;
	[Tooltip ("Effet d'augmentation du FOV")]
	public float SpeedEffectTime;
	[Tooltip ("Force bonus en plus de la gravitée")]
	public float BonusGrav = 0;
	[Tooltip ("Pourcentage de ralentissement du personnage dans les airs")]
	public float PourcRal = 50;

	[Header ("IncreaseSpeed")]
	[Tooltip ("Distance a parcourir pour augmenter la vitesse Max")]
	public int DistIncMaxSpeed = 100;
	[Tooltip ("Augmentation du speed max")]
	public float SpeedIncrease = 0;
	public float CLSpeedIncrease = 0;
	public float MaxSpeedInc = 10;
	public float MaxCLInc = 10;
	public float AcceleraInc = 0;
	public float AcceleraCLInc = 0;

	//public float JumpForce = 200;
    [Header("Caractéristique Fight")]
    /*public float delayLeft = 1;
	public float delayRight = 1;*/
	public float DashTime = 1.5f;
	[Tooltip ("La valeur de DashSpeed est un multiplicateur sur la vitesse du joueur")]
	public float DashSpeed = 2.5f;
	[Tooltip ("Temps d'invicibilité apres avoir pris des dégats")]
	public float TimeInvincible = 2;

	//[Header ("Slow Motion Caractéristique")]
	[HideInInspector]
	public float SlowMotion, SpeedSlowMot, SpeedDeacSM, RecovSlider, ReduceSlider;

	[Header ("Caractérique punchs")]
    public float DelayPunch = 0.05f;
	public float TimeToDoublePunch = 0.25f;
	public float CooldownDoublePunch = 1;
	public float DelayHitbox = 0.05f;
	public float DelayPrepare = 0.05f;

	[Tooltip ("Le temps max sera delayPunch")]
	public float TimePropulsePunch = 0.1f, TimePropulseDoublePunch = 0.2f;
	[Tooltip ("La valeur est un multiplicateur sur la vitesse du joueur")]
	public float SpeedPunchRun = 1.2f, SpeedDoublePunchRun = 1.5f;

    [Header("Caractéristique Madness")]
    public float RatioMaxMadness = 4;
    public float DelayDownBar = 3;
    public float LessPointPunchInMadness = 15;
    public float SmoothSpeed = 100;

	[Header ("SphereMask")]
	public float Radius;
	public float SoftNess;

	[HideInInspector]
	public Slider BarMadness;
	[HideInInspector]
	public SpecialAction ThisAct;
	[HideInInspector]
	public int NbrLineRight = 1;
	[HideInInspector]
	public int NbrLineLeft = 1;
	[HideInInspector]
	public bool Dash = false;
	[HideInInspector]
	public bool Running = true;
	public int Life = 1;
	public bool StopPlayer = false;

	private Collider punchBox;
	private Punch punch;
    private bool canPunch, punchRight;//, punchLeft, preparRight, preparLeft, defense;
    //private Coroutine corou/*, preparPunch*/;

    public GameObject Plafond;

    Transform pTrans;
	Rigidbody pRig;
	Direction currentDir = Direction.North;
	Direction newDir = Direction.North;
	//Vector3 posDir;
	Vector3 dirLine = Vector3.zero;
	Vector3 lastPos;
	IEnumerator propPunch;
	Punch getPunch;
	Camera thisCam;
	Slider SliderSlow;
	Text textDist;
	Text textCoin;
	Animator playAnimator;

	float PropulseBalls = 100;
	float currSpeed = 0;
	float currSpLine = 0;
	//float calPos = 0;
	float newH = 0;
	float newDist;
	float saveDist;
	float befRot = 0;
	float SliderContent;
	float totalDis = 0;
    float rationUse = 1;
	float nextIncrease = 0;
	float maxSpeed = 0;
	float maxSpeedCL = 0;
	float accelerationCL = 0;
	float acceleration = 0;
	float impulsionCL = 0;
	float decelerationCL = 0;
    float valueSmooth = 0;
    float valueSmoothUse = 0;
	float timeToDP;

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
	bool invDamage = false;
    bool InMadness = false;
	bool animeSlo = false;
	bool canSpe = true;
	bool playerDead = false;
	bool dpunch = false;
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
		lastPos = pTrans.position;
		textDist = GlobalManager.Ui.ScorePoints;
		textCoin = GlobalManager.Ui.MoneyPoints;
		nextIncrease = DistIncMaxSpeed;
		maxSpeed = MaxSpeed;
		maxSpeedCL = MaxSpeedCL;
		accelerationCL = AccelerationCL;
		acceleration = Acceleration;
		impulsionCL = ImpulsionCL;
		decelerationCL = DecelerationCL;
		playAnimator = GetComponentInChildren<Animator> ( );
        /* punchLeft = true; preparRight = false; preparLeft = false; defense = false;
		preparPunch = null;*/


        Plafond.GetComponent<MeshRenderer>().enabled = true;
    }

	void Update ( )
	{
		Shader.SetGlobalFloat ( "_saturation", BarMadness.value / 15);

		float getTime = Time.deltaTime;

		rationUse = 1 + (RatioMaxMadness * (InMadness ? 1 : (BarMadness.value / BarMadness.maxValue)));
		punch.SetPunch ( !playerDead );

		distCal ( );
		playerAction ( getTime );

		Mathf.Clamp ( Radius, 0, 100 );
		Mathf.Clamp ( SoftNess, 0, 100 );

		Shader.SetGlobalVector ( "GlobaleMask_Position", new Vector4 ( pTrans.position.x, pTrans.position.y, pTrans.position.z, 0 ) );
		Shader.SetGlobalFloat ( "GlobaleMask_Radius", Radius );
		Shader.SetGlobalFloat ( "GlobaleMask_SoftNess", SoftNess );
		Shader.SetGlobalFloat ( "_SlowMot", Time.timeScale );


        SmoothBar();

        if (BarMadness.value - (getTime * DelayDownBar) > 0)
        {
            BarMadness.value -= getTime * DelayDownBar;
        }
        else
        {
            BarMadness.value = 0;
        }
        if (Input.GetKeyDown(KeyCode.O))
            PlayerPrefs.DeleteAll();

    }
	#endregion

	#region Public Functions
	public void ResetPlayer ( )
	{
		Life = 1;
		playerDead = false;
		StopPlayer = true;
		totalDis = 0;
		maxSpeed = MaxSpeed;
		maxSpeedCL = MaxSpeedCL;
		accelerationCL = AccelerationCL;
		acceleration = Acceleration;
		impulsionCL = ImpulsionCL;
		decelerationCL = DecelerationCL;
		ThisAct = SpecialAction.Nothing;
		timeToDP = TimeToDoublePunch;
		BarMadness = GlobalManager.Ui.Madness;
		BarMadness.value = 0;
		InMadness = false;
		GlobalManager.Ui.CloseMadness ( );
	}

	public void GameOver ( bool forceDead = false )
	{

        GlobalManager.Ui.GameOver();

        if ( invDamage  && !forceDead )
		{
			return;
		}

        ScreenShake.Singleton.ShakeGameOver();

        GameOverTok thisTok = new GameOverTok ( );
		thisTok.totalDist = totalDis;

        StopPlayer = true;

        Camera.main.GetComponent<RainbowMove>().enabled = false;
        Camera.main.GetComponent<RainbowRotate>().enabled = false;

        DOVirtual.DelayedCall(.2f, () => {

            Camera.main.transform.DORotate(new Vector3(-220, 0, 0), 1.8f, RotateMode.LocalAxisAdd);
            Camera.main.transform.DOLocalMoveZ(-50f, .4f);
        });



        Life--;

		if ( Life > 0 || playerDead )
		{
			invDamage = true;
			Invoke ( "waitInvDmg", TimeInvincible );
			GlobalManager.Ui.StartBonusLife();

			return;
		}


        DOVirtual.DelayedCall(1f, () => {

            GlobalManager.Ui.OpenThisMenu(MenuType.GameOver, thisTok);
            ScreenShake.Singleton.ShakeGameOver();
            playerDead = true;
        });
		//GlobalManager.Ui.OpenThisMenu ( MenuType.GameOver );

		//GlobalManager.GameCont.Restart ( );
	}

    public void AddSmoothCurve(float p_value)
    {
        valueSmooth = valueSmoothUse + p_value;
        valueSmoothUse = valueSmooth;
    }

    public bool IsInMadness()
    {
        return InMadness;
    }
	#endregion

	#region Private Functions
	void playerAction ( float getTime )
	{
        if ( playerDead || StopPlayer )
		{
			thisCam.fieldOfView = Constants.DefFov;
			return;
		}

		if( BarMadness.value == 0 && InMadness )
		{
            GetComponentInChildren<Animator>().SetBool("InMadness", false);


            GlobalManager.Ui.CloseMadness();
            InMadness = false;
		}

		if ( Running )
		{
			if ( currSpeed < maxSpeed )
			{
				currSpeed += acceleration * getTime;
			}
			else if ( currSpeed > maxSpeed )
			{
				currSpeed = maxSpeed;
			}
		}
		else
		{
			currSpeed -= Deceleration * getTime;

			if ( currSpeed < 0 )
			{
				currSpeed = 0;
			}

			currSpLine -= Deceleration * getTime;

			if ( currSpLine < 0 )
			{
				currSpLine = 0;
			}
		}

		if ( !playerDead )
		{
			if ( Input.GetAxis ( "CoupSimple" ) == 0 )
			{
				resetAxeS = true;
			}

			if ( Input.GetAxis ( "CoupDouble" ) == 0 )
			{
				resetAxeD = true;

				timeToDP = TimeToDoublePunch;
			}

			if ( Input.GetAxis ( "CoupDouble" ) != 0 && resetAxeD )
			{
				timeToDP -= Time.deltaTime;

				if ( timeToDP <= 0 )
				{
					resetAxeD = false;
					dpunch = true;
				}
			}

            playerFight ( );
		}

		if ( Input.GetAxis ( "Dash") != 0 && newH == 0 && canDash && !InMadness )
		{
			Dash = true;
			canDash = false;

			StartCoroutine ( waitStopDash ( ) );
		}

		checkInAir ( getTime );

		speAction ( getTime );

		changeLine ( getTime );

		playerMove ( getTime, currSpeed );
	}

	void distCal ( )
	{
		totalDis += Vector3.Distance ( lastPos, pTrans.position );
		lastPos = pTrans.position;
		textDist.text = "" + Mathf.RoundToInt ( totalDis );

		if ( totalDis > nextIncrease )
		{
			nextIncrease += DistIncMaxSpeed;

			if ( MaxSpeedInc > MaxSpeed - maxSpeed )
			{
				maxSpeed += SpeedIncrease;
				acceleration += AcceleraInc;
			}
			else
			{
				maxSpeed = SpeedIncrease;
			}

			if ( MaxCLInc > maxSpeedCL - MaxSpeedCL )
			{
				maxSpeedCL += CLSpeedIncrease;
				accelerationCL += AcceleraCLInc;
				impulsionCL += AcceleraCLInc;
				decelerationCL += AcceleraCLInc;
			}
			else
			{
				maxSpeedCL = MaxCLInc;
			}
		}
	}

	void speAction ( float getTime )
	{
		if ( ThisAct == SpecialAction.SlowMot )
		{
			if ( Input.GetAxis ( "SpecialAction" ) > 0 && canSpe && SliderContent > 0 )
			{
				Camera.main.GetComponent<CameraFilterPack_Vision_Aura>().enabled = true;

				if ( !animeSlo )
				{
					animeSlo = true;
					GlobalManager.Ui.StartSlowMo();
				}

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
					canSpe = false;
					SliderContent = 0;
				}

				Time.timeScale += getTime * SpeedDeacSM;
			}
			else if ( SliderContent < 10 )
			{
				animeSlo = false;
				Time.timeScale = 1;
				SliderContent += RecovSlider * getTime;
				Camera.main.GetComponent<CameraFilterPack_Vision_Aura>().enabled = false;

				if ( SliderContent > 2 )
				{
					canSpe = true;
				}

			}
			else
			{
				canSpe = true;
				SliderContent = 10;
			}

			SliderSlow.value = SliderContent;
		}
	}

	void waitInvDmg ( )
	{
		invDamage = false;
	}

	void checkInAir ( float getTime )
	{
		RaycastHit[] allHit;
		bool checkAir = true;

		allHit = Physics.RaycastAll ( pTrans.position, Vector3.down, 2 );
		foreach ( RaycastHit thisRay in allHit )
		{
			checkAir = false;

			if ( thisRay.collider.gameObject.layer == 9)
			{
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
			}
		}

		if ( checkAir )
		{
			pRig.useGravity = true;
			pRig.AddForce ( Vector3.down * BonusGrav * getTime, ForceMode.Force );
		}

		inAir = checkAir;
	}

	void playerMove ( float delTime, float speed )
	{
		Transform transPlayer = pTrans;
		Vector3 calTrans = Vector3.zero;
		delTime = Time.deltaTime;

		if ( inAir )
		{
			speed = ( speed / 100 ) * PourcRal;
		}
		/*else
		{
			canJump = true;
		}*/

		if ( Dash )
		{
			speed *= DashSpeed;

			GlobalManager.Ui.DashSpeedEffect ( true );
			Camera.main.GetComponent<CameraFilterPack_Blur_BlurHole> ( ).enabled = true;
		}
		else
		{
			GlobalManager.Ui.DashSpeedEffect ( false );
			Camera.main.GetComponent<CameraFilterPack_Blur_BlurHole>().enabled = false;

			if ( propP )
			{
				speed *= SpeedPunchRun;
			}
			else if ( propDP )
			{
				speed *= SpeedDoublePunchRun;
			}
		}

		float calCFov = Constants.DefFov * ( speed / maxSpeed );

		if ( !inAir )
		{
			Shader.SetGlobalFloat ( "_ReduceVis", speed / maxSpeed);

			if ( thisCam.fieldOfView < calCFov)
			{
				thisCam.fieldOfView += Time.deltaTime * SpeedEffectTime;
				if ( thisCam.fieldOfView > calCFov )
				{
					thisCam.fieldOfView = calCFov;
				}
			}
			else if ( thisCam.fieldOfView > calCFov)
			{
				thisCam.fieldOfView -= Time.deltaTime * SpeedEffectTime * 4;
				if ( thisCam.fieldOfView < calCFov )
				{
					thisCam.fieldOfView = calCFov;
				}
			}
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
			pRig.AddForce ( transPlayer.up * JumpForce, ForceMode.Impulse );
		}*/
	}

	void changeLine ( float delTime )
	{
		float newImp = Input.GetAxis ( "Horizontal" );
		float lineDistance = Constants.LineDist;

		if ( ( canChange || newH == 0 ) && !inAir )
		{
			if ( newImp == 1 && LastImp != 1 && currLine + 1 <= NbrLineRight && ( clDir == 1 || newH == 0 ) )
			{
				Dash = false;
				canChange = false;
				currLine++;
				LastImp = 1;
				clDir = 1;
				newH = newH + lineDistance;
				saveDist = newH;
			}
			else if ( newImp == -1 && LastImp != -1 && currLine - 1 >= -NbrLineLeft && ( clDir == -1 || newH == 0 ) )
			{
				Dash = false;
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
					currSpLine -= decelerationCL * delTime;

					canChange = true;

					if ( currSpLine < 0 )
					{
						currSpLine = 0.1f;
					}
				}
				else if ( currSpLine < maxSpeedCL )
				{
					accLine = ( currSpLine * impulsionCL ) / maxSpeedCL; 

					if ( accLine > 1 || accLine == 0 )
					{
						accLine = 1;
					}

					currSpLine += accelerationCL * accLine * delTime;
				}
				else if ( currSpLine > maxSpeedCL )
				{
					currSpLine = maxSpeedCL;
				}
			}

			float calTrans = clDir * currSpLine * delTime;

			if ( inAir )
			{
				calTrans = ( calTrans / 100 ) * PourcRal;
			}
			newH -= calTrans;

			if ( saveDist > 0 && newH - calTrans < 0 || saveDist < 0 && newH > 0 )
			{
				calTrans += newH;
				newH = 0;
				currSpLine = 0;
			}

			//Debug.Log ( currSpLine );

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
		if ( Input.anyKeyDown && Input.GetAxis ( "SpecialAction" ) == 0 && Input.GetAxis ( "Horizontal" ) == 0 )
		{
			if (InMadness)
			{
				if (BarMadness.value - LessPointPunchInMadness < 0)
				{
					BarMadness.value = 0;
				}
				else
				{
					BarMadness.value -= LessPointPunchInMadness;
                    ScreenShake.Singleton.ShakeMad();
                    Camera.main.GetComponent<CameraFilterPack_Distortion_Dream2>().Distortion = (BarMadness.value/10);
                }
			}
		}

		if(Input.GetAxis("CoupSimple") != 0 && canPunch && resetAxeS  )
        {
			Dash = false;
            resetAxeS = false;
            canPunch = false;
            propP = true;
			timeToDP = TimeToDoublePunch;

			if ( !InMadness )
			{
				punch.MadnessMana("Simple");
			}

            ScreenShake.Singleton.ShakeHitSimple();
       
            if (punchRight)
            {
                punch.RightPunch = true;

				playAnimator.SetTrigger("Right");

				GlobalManager.Ui.SimpleCoup();
            }
            else
            {
                punch.RightPunch = false;

				playAnimator.SetTrigger("Left");
            }
            punchRight = !punchRight;
			StartCoroutine( StartPunch ( 0 ) );
            propPunch = propulsePunch(TimePropulsePunch);
            StartCoroutine(propPunch);
		}
		else if( dpunch && canPunch )
        {
			Dash = false;
			dpunch = false;
			canPunch = false;
			timeToDP = TimeToDoublePunch;

            ScreenShake.Singleton.ShakeHitDouble();

            GlobalManager.Ui.DoubleCoup();

			playAnimator.SetTrigger("Double");

			if ( !InMadness )
			{
				punch.MadnessMana("Double");
			}

            propDP = true;
			StartCoroutine ( StartPunch ( 1 ) );

			propPunch = propulsePunch ( TimePropulseDoublePunch );
			StartCoroutine ( propPunch );
        }
	}

	private IEnumerator StartPunch(int type_technic)
	{
		yield return new WaitForSeconds(DelayPrepare / rationUse);
        punch.setTechnic(type_technic);
        punchBox.enabled = true;
       /* corou =*/ StartCoroutine("TimerHitbox");

        Shader.SetGlobalFloat("_saturation", BarMadness.value);

		StartCoroutine ( CooldownPunch ( type_technic ) );

        if (InMadness)
        {
            if (BarMadness.value - LessPointPunchInMadness < 0)
            {
                BarMadness.value = 0;
            }
            else
            {
                   
                BarMadness.value -= LessPointPunchInMadness;
            }
            AddSmoothCurve(-LessPointPunchInMadness);
        }
	}

	private IEnumerator CooldownPunch ( int type_technic )
    {
		if ( type_technic == 1 )
		{
			yield return new WaitForSeconds ( ( DelayPunch * 4 ) / rationUse );
		}
		else
		{
			yield return new WaitForSeconds(DelayPunch / rationUse);
		}

		canPunch = true;
    }

	private IEnumerator TimerHitbox()
	{
		yield return new WaitForSeconds(DelayHitbox);
		punchBox.enabled = false;
	}

	IEnumerator waitStopDash ( )
	{
		WaitForSeconds thisS = new WaitForSeconds ( DashTime );

		yield return thisS;

		Dash = false;

		yield return new WaitForSeconds ( DashTime / 2 );
		canDash = true;
	}

	IEnumerator propulsePunch ( float thisTime )
	{
		WaitForSeconds thisSec = new WaitForSeconds ( thisTime );

		yield return thisSec;

		propP = false;
		propDP = false;
	}

	void OnTriggerEnter ( Collider thisColl )
	{
		if ( thisColl.tag == Constants._NewDirec )
		{
			newPos = true;
			newDir = thisColl.GetComponent<NewDirect> ( ).NewDirection;
			befRot = Vector3.Distance ( thisColl.transform.position, pTrans.position );
		} 
	}

	void OnCollisionEnter ( Collision thisColl )
	{
		GameObject getObj = thisColl.gameObject;

		if ( Dash || InMadness )
		{
			if ( getObj.tag == Constants._EnnemisTag || getObj.tag == Constants._ElemDash )
			{
                GlobalManager.Ui.BloodHit();

				/*Vector3 getProj = getPunch.projection_basic;

				if ( Random.Range ( 0,2 ) == 0 )
				{
					getProj.x *= Random.Range ( -getProj.x, -getProj.x / 2 );
				}
				else
				{
					getProj.x *= Random.Range ( getProj.x / 2, getProj.x );
				}*/
				thisColl.collider.enabled = false;
				thisColl.gameObject.GetComponent<AbstractObject> ( ).ForceProp (  getPunch.projection_double );
				return;
			}
			else if ( getObj.tag == Constants._Balls )
			{
				StartCoroutine ( GlobalManager.GameCont.MeshDest.SplitMesh ( getObj, pTrans, PropulseBalls, 1, 5, true ) );
				return;
			}
		}

		if ( getObj.tag == Constants._MissileBazoo )
		{
			getObj.GetComponent<MissileBazooka> ( ).Explosion ( );
			GameOver ( );
		}
		else if ( getObj.tag == Constants._EnnemisTag || getObj.tag == Constants._MissileBazoo || getObj.tag == Constants._Balls )
		{
			GameOver ( );
		}
		else if ( getObj.tag == Constants._ObsTag )
		{
			Life = 0;
			GameOver ( true );
		}
	}

    private void SmoothBar()
    {
        float res = valueSmoothUse * (Time.deltaTime * SmoothSpeed);
        if(BarMadness.value + res < 0)
        {
            BarMadness.value = 0;
            valueSmooth = 0;
            valueSmoothUse = 0;
            if (InMadness)
            {
                InMadness = !InMadness;

				maxSpeed = MaxSpeed;
				maxSpeedCL = MaxSpeedCL;
				accelerationCL = AccelerationCL;
				acceleration = Acceleration;

                GlobalManager.Ui.CloseMadness();
            }
        }else if (BarMadness.value + res >= 100)
        {
            Debug.Log("first etape");
            BarMadness.value = 100;
            valueSmooth = 0;
            valueSmoothUse = 0;

            if (!InMadness)
            {
                Debug.Log("MADDDDDDDD");
                InMadness = !InMadness;

				maxSpeedCL = MaxSpeedCL * 2;
				maxSpeed = MaxSpeed * 3;
				accelerationCL = AccelerationCL * 2;
				acceleration = Acceleration * 4;

                GlobalManager.Ui.OpenMadness();
            }
        }
        else
        {
            BarMadness.value += res;
            valueSmoothUse -= res;
        }
    }
	#endregion
}
