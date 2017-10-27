using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemisLane : AbstractObject 
{
	#region Variables
	[Space]
	public float TimeToChange = 1;

	[Tooltip ("Nombre de lane que l'ennemis peut traverser")]
	public int NbrLane = 1;

	[Tooltip ("Si à 0 alors il peux se diriger vers la gauche ou droite : X = gauche - Y = droite")]
	public Vector2 CanGo = Vector2.zero;
	public bool FollowPlayer = false;

	int currLine = 0;
	bool moving;
	bool playerDetected = false;
	bool firtDe = false;

	GameObject savePlayer;
	#endregion

	#region Mono
	void Update ( )
	{
		if ( !isDead && playerDetected && ( FollowPlayer || !firtDe ) && !moving )
		{
			Transform objTrans = savePlayer.transform;
			Vector3 getPos = objTrans.position;
			Vector3 thisPos = getTrans.position;

			float getRight = Vector3.Distance ( getPos, thisPos + getTrans.right);
			float getLeft = Vector3.Distance ( getPos, thisPos - getTrans.right );

			if ( getRight - getLeft < 0.25f && getRight - getLeft > -0.25f )
			{
				return;
			}

			if ( getRight < getLeft )
			{
				if ( CanGo.y == 0 && currLine < NbrLane )
				{
					firtDe = true;
					currLine ++;
					StartCoroutine ( changeLane ( true ) );
				}
			}
			else if ( CanGo.x == 0 && currLine > -NbrLane )
			{
				firtDe = true;
				currLine --;
				StartCoroutine ( changeLane ( false ) );
			}
		}
	}
	#endregion

	#region Public Methods
	public override void PlayerDetected ( GameObject thisObj, bool isDetected )
	{
		base.PlayerDetected ( thisObj, isDetected );

		if ( isDetected && !isDead )
		{
			savePlayer = thisObj;
			Transform objTrans = thisObj.transform;
			Vector3 getPos = objTrans.position;
			Vector3 thisPos = getTrans.position;

			float getRight = Vector3.Distance ( getPos + objTrans.right, thisPos );
			float getLeft = Vector3.Distance ( getPos - objTrans.right, thisPos );

			playerDetected = true;

			if ( getRight - getLeft < 0.25f && getRight - getLeft > -0.25f )
			{
				return;
			}



			if ( getRight > getLeft )
			{
				if ( CanGo.y == 0 )
				{
					firtDe = true;
					currLine ++;
					StartCoroutine ( changeLane ( true ) );
				}
			}
			else if ( CanGo.x == 0 )
			{
				firtDe = true;
				currLine--;
				StartCoroutine ( changeLane ( false ) );
			}
		}
		else
		{
			playerDetected = false;
		}
	}
	#endregion

	#region Private Methode
	public override void Dead ( bool enemy = false ) 
	{
		base.Dead ( enemy );
        

		//mainCorps.GetComponent<BoxCollider> ( ).enabled = false;
	}

	IEnumerator changeLane ( bool rightLine ) 
	{
		moving = true;

		WaitForEndOfFrame thisFrame = new WaitForEndOfFrame ( );
		Transform objTrans = getTrans;
		Vector3 getStart;
		Vector3 thisDist;

		float getTime = TimeToChange;
		float currTime = 0;

		if ( rightLine )
		{
			thisDist = new Vector3 ( Constants.LineDist, 0, 0 );
		}
		else
		{
			thisDist = new Vector3 ( -Constants.LineDist, 0, 0 );
		}

		getStart = objTrans.localPosition;

		do 
		{
			yield return thisFrame;

			currTime += Time.deltaTime;
			objTrans.localPosition = getStart + thisDist * ( currTime / getTime);
		}while ( currTime < getTime );

		objTrans.localPosition = getStart + thisDist;
		moving = false;
	}
	#endregion
}
