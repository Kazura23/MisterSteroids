﻿using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Chunk", menuName = "Chunk", order = 1)]
public class ChunksScriptable : ScriptableObject 
{
	[Tooltip ("Nombre de chunk pour faire un niveau")]
	public int NbrChunkOneLvl;

	public bool ChunkAleat = true;

	[Header ("Chunks")]
	public List<GameObject> TheseChunks;

	[Header ("Prefabs d'objet à instancier")]

	[Range( 0,100)]
	public int PourcSpawn;

	[Tooltip ("Tous les ennemis que l'on peut spawn dans ce chunk")]
	public List<GameObject> EnnemySpawnable;

	[Tooltip ("Obstacle ne pouvant pas être détruit")]
	public List<GameObject> ObstacleSpawnable;

	[Tooltip ("Obstacle pouvant être détruit")]
	public List<GameObject> ObstacleDestrucSpawnable;
	public List<GameObject> CoinSpawnable;
	public GameObject WallOnLastChunk;
}
