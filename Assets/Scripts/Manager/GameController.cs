using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : ManagerParent
{
	#region Variables
	public Transform GarbageTransform;
	public MeshDesctruc MeshDest;
	public GameObject Player;
	public SpawnChunks SpawnerChunck;
    #endregion

    #region Mono
	void Start ( )
	{
		StartGame ( );
	}
    #endregion

    #region Public Methods
	public void StartGame ( )
	{
		SpawnerChunck.FirstSpawn ( );
	}

    public void Restart ( ) 
	{
		SceneManager.LoadScene ( "ProtoAlex", LoadSceneMode.Single );
		GlobalManager.Ui.DisplayOver ( false );
		StartGame ( );
    }
    #endregion

    #region Private Methods
    protected override void InitializeManager ( )
	{
		Player = GameObject.FindGameObjectWithTag("Player");
		SpawnerChunck = GetComponentInChildren<SpawnChunks> ( );
	}
	#endregion
}
