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

	List<List<GetSpawnable>> AllSpawnable;
	List<GameObject> getSpawnChunks;

	GameObject WallOnLastChunk;
	Transform thisT;
	int currNbrCh = 0;
	int currChunk = 0;
	bool randAllChunk = false;
	#endregion
	
	#region Mono
	#endregion
		
	#region Public
	public void InitChunck ( )
	{
		getSpawnChunks = new List<GameObject> ( );
		AllSpawnable = new List<List<GetSpawnable>> ( );
		thisT = transform;

		List<ChunksScriptable> getChunks = ChunksInfo;
		List<List<GetSpawnable>> getSpawnable = AllSpawnable;

		Transform[] getChildrenChunk;

		int a;
		int b;
		int c;

		for ( a = 0; a < getChunks.Count; a++ )
		{
			getSpawnable.Add ( new List<GetSpawnable> ( ) );


			for ( b = 0; b < getChunks [ a ].TheseChunks.Count; b++ )
			{
				getSpawnable [ a ].Add ( new GetSpawnable ( ) );
				getSpawnable [ a ] [ b ].getCoinSpawnable = new List<GameObject> ( );
				getSpawnable [ a ] [ b ].getEnnemySpawnable = new List<GameObject> ( );
				getSpawnable [ a ] [ b ].getObstacleDestrucSpawnable = new List<GameObject> ( );
				getSpawnable [ a ] [ b ].getObstacleSpawnable = new List<GameObject> ( );

				getChildrenChunk = getChunks [ a ].TheseChunks [ b ].GetComponentsInChildren<Transform> ( true );

				for ( c = 0; c < getChildrenChunk.Length; c++ )
				{
					switch ( getChildrenChunk[c].tag )
					{
					case Constants._SAbleCoin:
						getSpawnable [ a ] [ b ].getCoinSpawnable.Add ( getChildrenChunk [ c ].gameObject );
						break;
					case Constants._SAbleDestObs:
						getSpawnable [ a ] [ b ].getObstacleDestrucSpawnable.Add ( getChildrenChunk [ c ].gameObject );
						break;
					case Constants._SAbleObs:
						getSpawnable [ a ] [ b ].getObstacleSpawnable.Add ( getChildrenChunk [ c ].gameObject );
						break;
					case Constants._SAbleEnnemy:
						getSpawnable [ a ] [ b ].getEnnemySpawnable.Add ( getChildrenChunk [ c ].gameObject );
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

		spawnAfterThis ( sourceSpawn.position, sourceSpawn.rotation, sourceSpawn.GetComponent<SpawnNewLvl> ( ) );

		if ( getSpc.Count > 5 )
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

		if ( getChunks [ currLevel ].WallOnLastChunk != null )
		{
			thisObj = ( GameObject ) Instantiate ( getChunks [ currLevel ].WallOnLastChunk, thisT );
			thisObj.transform.position = getChunkT.position;
			thisObj.transform.localPosition += thisObj.transform.up * thisObj.GetComponent<MeshRenderer> ( ).bounds.size.y / 2;
		}

		currLevel++;

		if ( currLevel >= getChunks.Count || randAllChunk )
		{
			randAllChunk = true;

			currLevel = Random.Range ( 0, getChunks.Count );
		}

		currNbrCh = 0;
	}

	void spawnAfterThis ( Vector3 thisPos, Quaternion thisRot, SpawnNewLvl thisLvl = null )
	{
		List<ChunksScriptable> getChunks = ChunksInfo;
		List<List<GetSpawnable>> getSpawnable = AllSpawnable;
		List<GameObject> getSpc = getSpawnChunks;
		GameObject thisSpawn;
		Transform getChunkT;

		if ( getChunks [ currLevel ].ChunkAleat )
		{
			currChunk = Random.Range ( 0, getChunks [ currLevel ].TheseChunks.Count );

			thisSpawn = getChunks [ currLevel ].TheseChunks [ currChunk ];
		}
		else
		{
			currChunk = currNbrCh;
			thisSpawn = getChunks [ currLevel ].TheseChunks [ currNbrCh ];
		}

		if ( thisLvl != null )
		{
			SpawnNewLvl getNew = thisSpawn.GetComponentInChildren<SpawnNewLvl> ( );
			int getNbr;
			int calNbr;

			if ( Random.Range ( 0, 2 ) == 0 )
			{
				getNbr = -Random.Range ( 0, ( int ) thisLvl.NbrLaneFin.x + 1 );
				thisPos = new Vector3 ( thisPos.x + Constants.LineDist * getNbr, thisPos.y, thisPos.z );
			}
			else
			{
				getNbr = Random.Range ( 0, ( int ) thisLvl.NbrLaneFin.y + 1 );
				thisPos = new Vector3 ( thisPos.x + Constants.LineDist * getNbr, thisPos.y, thisPos.z );
			}

			calNbr = ( int ) thisLvl.NbrLaneFin.x - getNbr;

			int a;
			if ( getNew.NbrLaneDebut.x < calNbr )
			{
				for ( a = calNbr; a > getNew.NbrLaneDebut.x; a-- )
				{
					Instantiate ( getChunks [ currLevel ].WallEndChunk, thisLvl.transform );
				}
			}

			calNbr = ( int ) thisLvl.NbrLaneFin.x + getNbr;
			if ( getNew.NbrLaneDebut.y > calNbr )
			{
				for ( a = calNbr; a < getNew.NbrLaneDebut.y; a++ )
				{
					Instantiate ( getChunks [ currLevel ].WallEndChunk, thisLvl.transform );
				}
			}
		}

		currNbrCh++;

		if ( thisSpawn != null )
		{
			thisSpawn = ( GameObject ) Instantiate ( thisSpawn, thisT );

			getChunkT = thisSpawn.transform;
			getChunkT.rotation = thisRot;
			getChunkT.position = thisPos;
		}

		spawnElements ( getSpawnable [ currLevel ] [ currChunk ].getCoinSpawnable, getChunks [ currLevel ].CoinSpawnable );
		spawnElements ( getSpawnable [ currLevel ] [ currChunk ].getEnnemySpawnable, getChunks [ currLevel ].EnnemySpawnable );
		spawnElements ( getSpawnable [ currLevel ] [ currChunk ].getObstacleSpawnable, getChunks [ currLevel ].ObstacleSpawnable );
		spawnElements ( getSpawnable [ currLevel ] [ currChunk ].getObstacleDestrucSpawnable, getChunks [ currLevel ].ObstacleDestrucSpawnable );

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

public class GetSpawnable
{
	public List<GameObject> getEnnemySpawnable;
	public List<GameObject> getObstacleSpawnable;
	public List<GameObject> getObstacleDestrucSpawnable;
	public List<GameObject> getCoinSpawnable;
}

public class SpawnDetail 
{

}