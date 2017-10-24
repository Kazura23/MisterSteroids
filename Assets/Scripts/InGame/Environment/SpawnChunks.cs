using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnChunks : MonoBehaviour 
{
	#region Variable
	public List<Chunks> AllChunks;
	public Vector3 DefaultPos;

	[HideInInspector]
	public int currLevel = 0;

	List<GameObject> getSpawnChunks;
	Transform thisT;
	int currNbrCh = 0;
	#endregion
	
	#region Mono
	void Awake ( )
	{
		getSpawnChunks = new List<GameObject> ( );
		thisT = transform;
	}
	#endregion
		
	#region Public
	public void NewSpawn ( Transform sourceSpawn )
	{
		List<Chunks> getChunks = AllChunks;
		List<GameObject> getSpc = getSpawnChunks;
		Transform getChunkT;
		GameObject thisObj;
		bool newLevel = false;

		currNbrCh++;

		if ( getChunks [ currLevel ].NbrChunkOneLvl == currNbrCh )
		{
			newLevel = true;
		}

		spawnAfterThis ( sourceSpawn.position + sourceSpawn.forward * Constants.ChunkLengh, sourceSpawn.rotation );

		if ( getSpc.Count > 2 )
		{
			Destroy ( getSpc [ 0 ] );
			getSpc.RemoveAt ( 0 );
		}

		if ( !newLevel )
		{
			getChunkT = getSpc [ getSpc.Count - 1 ].transform;
			thisObj = ( GameObject ) Instantiate ( getChunks [ currLevel ].WallLastThisLvl, thisT );
			thisObj.transform.position = getChunkT.position + getChunkT.forward * Constants.ChunkLengh;
			thisObj.transform.localPosition += thisObj.transform.up * thisObj.GetComponent<MeshRenderer> ( ).bounds.size.y / 2;

			currLevel++;
			currNbrCh = 0;
		}
	}

	public void FirstSpawn ( )
	{
		currNbrCh = 0;
		currLevel = 0;
		List<Chunks> getChunks = AllChunks;
		List<GameObject> getSpc = getSpawnChunks;

		while ( getSpc.Count > 0 )
		{
			Destroy ( getSpc [ 0 ] );
			getSpc.RemoveAt ( 0 );
		}

		spawnAfterThis ( DefaultPos, Quaternion.identity );
	}
	#endregion
	
	#region Private
	void spawnAfterThis ( Vector3 thisPos, Quaternion thisRot )
	{
		List<Chunks> getChunks = AllChunks;
		List<GameObject> getSpc = getSpawnChunks;
		GameObject thisSpawn;
		Transform getChunkT;

		thisSpawn = getChunks [ currLevel ].TheseChunks [ Random.Range ( 0, getChunks [ currLevel ].TheseChunks.Count - 1 ) ];
		thisSpawn = ( GameObject ) Instantiate ( thisSpawn, thisT );

		getChunkT = thisSpawn.transform;

		getChunkT.rotation = thisRot;
		getChunkT.position = thisPos;
		getSpc.Add ( thisSpawn );
	}

	#endregion
}
	
[System.Serializable]
public class Chunks
{
	[Tooltip ("Nombre de chunk pour faire un niveau")]
	public int NbrChunkOneLvl;

	public List<GameObject> TheseChunks;
	public GameObject WallLastThisLvl;
}
