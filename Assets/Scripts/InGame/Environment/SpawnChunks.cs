using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnChunks : MonoBehaviour 
{
	#region Variable
	public List<ChunksScriptable> ChunksInfo;
	public Vector3 DefaultPos;

	[HideInInspector]
	public int currLevel = 0;

	List<GetSpawnable> AllSpawnable;
	List<GameObject> getSpawnChunks;

	GameObject WallOnLastChunk;
	Transform thisT;
	int currNbrCh = 0;
	bool randAllChunk = false;
	#endregion
	
	#region Mono
	#endregion
		
	#region Public
	public void InitChunck ( )
	{
		getSpawnChunks = new List<GameObject> ( );
		AllSpawnable = new List<GetSpawnable> ( );
		thisT = transform;

		List<ChunksScriptable> getChunks = ChunksInfo;
		List<GetSpawnable> getSpawnable = AllSpawnable;

		Transform[] getChildrenChunk;

		int a;
		int b;
		int c;

		for ( a = 0; a < getChunks.Count; a++ )
		{
			getSpawnable.Add ( new GetSpawnable ( ) );
			getSpawnable [ a ].getCoinSpawnable = new List<GameObject> ( );
			getSpawnable [ a ].getDebutFinCh = new List<GameObject> ( );
			getSpawnable [ a ].getEnnemySpawnable = new List<GameObject> ( );
			getSpawnable [ a ].getObstacleDestrucSpawnable = new List<GameObject> ( );
			getSpawnable [ a ].getObstacleSpawnable = new List<GameObject> ( );

			for ( b = 0; b < getChunks [ a ].TheseChunks.Count; b++ )
			{
				getChildrenChunk = getChunks [ a ].TheseChunks [ b ].GetComponentsInChildren<Transform> ( true );

				for ( c = 0; c < getChildrenChunk.Length; c++ )
				{
					switch ( getChildrenChunk[c].tag )
					{
					case Constants._SAbleCoin:
						getSpawnable [ a ].getCoinSpawnable.Add ( getChildrenChunk [ c ].gameObject );
						break;
					case Constants._SAbleDestObs:
						getSpawnable [ a ].getObstacleDestrucSpawnable.Add ( getChildrenChunk [ c ].gameObject );
						break;
					case Constants._SAbleObs:
						getSpawnable [ a ].getObstacleSpawnable.Add ( getChildrenChunk [ c ].gameObject );
						break;
					case Constants._SAbleEnnemy:
						getSpawnable [ a ].getEnnemySpawnable.Add ( getChildrenChunk [ c ].gameObject );
						break;
					case Constants._DebutFinChunk:
						getSpawnable [ a ].getDebutFinCh.Add ( getChildrenChunk [ c ].gameObject );
						break;
					}
				}
			}
		}
	}

	public void NewSpawn ( Transform sourceSpawn )
	{
		List<ChunksScriptable> getChunks = ChunksInfo;
		List<GameObject> getSpc = getSpawnChunks;

		float distChunk = Vector3.Distance ( AllSpawnable [ currLevel ].getDebutFinCh [ 0 ].transform.position, AllSpawnable [ currLevel ].getDebutFinCh [ 1 ].transform.position );

		spawnAfterThis ( sourceSpawn.position + sourceSpawn.forward * distChunk, sourceSpawn.rotation );

		if ( getSpc.Count > 2 )
		{
			Destroy ( getSpc [ 0 ] );
			getSpc.RemoveAt ( 0 );
		}

		if ( getChunks [ currLevel ].NbrChunkOneLvl >= currNbrCh )
		{
			newLevel ( );
		}
	}

	public void FirstSpawn ( )
	{
		randAllChunk = false;
		currNbrCh = 0;
		currLevel = 0;
		List<GameObject> getSpc = getSpawnChunks;
		bool doubleFirst = false;

		while ( getSpc.Count > 0 )
		{
			doubleFirst = true;
			Destroy ( getSpc [ 0 ] );
			getSpc.RemoveAt ( 0 );
		}

		if ( doubleFirst )
		{
			StartCoroutine ( waitSpawn ( ) );
		}
		else
		{
			List<ChunksScriptable> getChunks = ChunksInfo;

			spawnAfterThis ( DefaultPos, Quaternion.identity );

			if ( getChunks [ currLevel ].NbrChunkOneLvl <= currNbrCh )
			{
				newLevel ( );
			}
		}
	}
	#endregion
	
	#region Private
	IEnumerator waitSpawn ( )
	{
		yield return new WaitForEndOfFrame ( );
		List<ChunksScriptable> getChunks = ChunksInfo;

		spawnAfterThis ( DefaultPos, Quaternion.identity );

		if ( getChunks [ currLevel ].NbrChunkOneLvl <= currNbrCh )
		{
			newLevel ( );
		}
	}

	void newLevel ( )
	{
		List<ChunksScriptable> getChunks = ChunksInfo;
		List<GameObject> getSpc = getSpawnChunks;
		Transform getChunkT = getSpc [ getSpc.Count - 1 ].transform;
		GameObject thisObj;

		float distChunk = Vector3.Distance ( AllSpawnable [ currLevel ].getDebutFinCh [ 0 ].transform.position, AllSpawnable [ currLevel ].getDebutFinCh [ 1 ].transform.position );

		thisObj = ( GameObject ) Instantiate ( getChunks [ currLevel ].WallOnLastChunk, thisT );
		thisObj.transform.position = getChunkT.position + getChunkT.forward * distChunk;
		thisObj.transform.localPosition += thisObj.transform.up * thisObj.GetComponent<MeshRenderer> ( ).bounds.size.y / 2;

		currLevel++;

		if ( currLevel >= getChunks.Count || randAllChunk )
		{
			randAllChunk = true;

			currLevel = Random.Range ( 0, getChunks.Count );
		}

		currNbrCh = 0;
	}

	void spawnAfterThis ( Vector3 thisPos, Quaternion thisRot )
	{
		List<ChunksScriptable> getChunks = ChunksInfo;
		List<GetSpawnable> getSpawnable = AllSpawnable;;
		List<GameObject> getSpc = getSpawnChunks;
		GameObject thisSpawn;
		Transform getChunkT;

		if ( getChunks [ currLevel ].ChunkAleat )
		{
			thisSpawn = getChunks [ currLevel ].TheseChunks [ Random.Range ( 0, getChunks [ currLevel ].TheseChunks.Count ) ];
		}
		else
		{
			thisSpawn = getChunks [ currLevel ].TheseChunks [ currNbrCh ];
		}

		currNbrCh++;

		thisSpawn = ( GameObject ) Instantiate ( thisSpawn, thisT );

		getChunkT = thisSpawn.transform;

		getChunkT.rotation = thisRot;
		getChunkT.position = thisPos;

		spawnElements ( getSpawnable [ currLevel ].getCoinSpawnable, getChunks [ currLevel ].CoinSpawnable );
		spawnElements ( getSpawnable [ currLevel ].getEnnemySpawnable, getChunks [ currLevel ].EnnemySpawnable );
		spawnElements ( getSpawnable [ currLevel ].getObstacleSpawnable, getChunks [ currLevel ].ObstacleSpawnable );
		spawnElements ( getSpawnable [ currLevel ].getObstacleDestrucSpawnable, getChunks [ currLevel ].ObstacleDestrucSpawnable );

		getSpc.Add ( thisSpawn );
	}

	void spawnElements ( List<GameObject> spawnerElem, List<GameObject> elemSpawnable )
	{
		GameObject thisObj;
		int rand = ChunksInfo [ currLevel ].PourcSpawn;
		int a;

		for ( a = 0; a < spawnerElem.Count; a++ )
		{
			if ( Random.Range ( 0, 100 ) <= rand )
			{
				thisObj = ( GameObject ) Instantiate ( elemSpawnable [ Random.Range ( 0, elemSpawnable.Count ) ], spawnerElem [ a ].transform );
				thisObj.transform.localScale = new Vector3 ( 1, 1, 1 );
				thisObj.transform.localPosition = Vector3.zero;
			}
		}
	}
	#endregion
}

[System.Serializable]
public class GetSpawnable
{
	public List<GameObject> getEnnemySpawnable;
	public List<GameObject> getObstacleSpawnable;
	public List<GameObject> getObstacleDestrucSpawnable;
	public List<GameObject> getCoinSpawnable;
	public List<GameObject> getDebutFinCh;
}