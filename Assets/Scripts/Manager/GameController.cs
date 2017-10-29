using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : ManagerParent
{
	#region Variables
	public List<FxList> AllFx;

	public Transform GarbageTransform;
	public MeshDesctruc MeshDest;
	public GameObject Player;
	public SpawnChunks SpawnerChunck;
    public bool GameStarted;
    #endregion

    #region Mono
	void Update ( )
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			GlobalManager.Ui.OpenThisMenu(MenuType.Pause);
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			if (!GameStarted)
			{
				GameStarted = true;
				Player.GetComponent<PlayerController>().StopPlayer = false;
				Camera.main.GetComponent<RainbowRotate>().time = .4f;
				Camera.main.GetComponent<RainbowMove>().time = .2f;
			}
		}
	}
    #endregion

    #region Public Methods
	public void StartGame ( )
	{
		Player = GameObject.FindGameObjectWithTag("Player");

		SpawnerChunck.FirstSpawn ( );
		Player.GetComponent<PlayerController>().StopPlayer = true;
        Camera.main.GetComponent<RainbowRotate>().time = 2;
        Camera.main.GetComponent<RainbowMove>().time = 1;
		GlobalManager.Ui.CloseThisMenu ( );
    }

	public void FxInstanciate ( Vector3 thisPos, string fxName, Transform parentObj = null )
	{
		List<FxList> getAllFx = AllFx;
		GameObject getObj;

		for ( int a = 0;  a < getAllFx.Count; a++ )
		{
			if ( getAllFx [ a ].FxName == fxName )
			{
				getObj = getAllFx [ a ].FxObj;

				if ( parentObj != null )
				{
					getObj = ( GameObject ) Instantiate ( getObj, parentObj );

				}
				else
				{
					getObj = ( GameObject ) Instantiate ( getObj, parentObj );
				}

				getObj.transform.position = thisPos;

				break;
			}
		}

	}

    public void Restart ( ) 
	{
		SceneManager.LoadScene ( "ProtoAlex", LoadSceneMode.Single );
        GameStarted = false;
        StartGame ( );
    }   
    #endregion

    #region Private Methods
    protected override void InitializeManager ( )
	{
		SpawnerChunck = GetComponentInChildren<SpawnChunks> ( );
		SpawnerChunck.InitChunck ( );
	}
	#endregion
}


[System.Serializable]
public class FxList 
{
	public string FxName;
	public GameObject FxObj;
}