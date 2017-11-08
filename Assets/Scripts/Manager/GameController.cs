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
	[HideInInspector]
	public GameObject Player;
	public SpawnChunks SpawnerChunck;
    public bool GameStarted;

	[HideInInspector]
	public Dictionary <string, ItemModif> AllModifItem;
	[HideInInspector]
	public List <ItemModif> AllTempsItem;

	bool checkStart = false;
    #endregion

    #region Mono
	void Update ( )
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			GlobalManager.Ui.OpenThisMenu(MenuType.Pause);
		}

		if (Input.GetAxis("CoupSimple") == 1 || Input.GetAxis("CoupDouble") == 1)
        {
			if ( GameStarted && !checkStart )
			{
                GlobalManager.Ui.Intro();

				checkStart = true;
				Player.GetComponent<PlayerController> ( ).StopPlayer = false;
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
		Player.GetComponent<PlayerController> ( ).ResetPlayer ( );
		Player.GetComponent<PlayerController> ( ).ThisAct = SpecialAction.Nothing;

		SetAllBonus ( );
		GameStarted = true;
		checkStart = false;

		SpawnerChunck.FirstSpawn ( );

        Camera.main.GetComponent<RainbowRotate>().time = 2;
        Camera.main.GetComponent<RainbowMove>().time = 1;
		GlobalManager.Ui.CloseThisMenu ( );
    }

	public GameObject FxInstanciate ( Vector3 thisPos, string fxName, Transform parentObj = null, float timeDest = 0.35f )
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

				Destroy(getObj, timeDest);

                getObj.transform.position = thisPos;

				return getObj;
				break;
			}
		}

		return null;
    }

    public void Restart ( ) 
	{
		SceneManager.LoadScene ( "ProtoAlex", LoadSceneMode.Single );
        GameStarted = false;
    }   
    #endregion

    #region Private Methods
    protected override void InitializeManager ( )
	{
		SpawnerChunck = GetComponentInChildren<SpawnChunks> ( );
		SpawnerChunck.InitChunck ( );
	}

	void SetAllBonus ( )
	{
		Dictionary <string, ItemModif> getMod = AllModifItem;
		PlayerController currPlayer = Player.GetComponent<PlayerController> ( );
		List <ItemModif> AllTI = AllTempsItem;
		ItemModif thisItem;
		List<string> getKey = new List<string> ( );

		if ( getMod != null )
		{
			foreach ( KeyValuePair <string, ItemModif> thisKV in getMod )
			{
				thisItem = thisKV.Value;

				setItemToPlayer ( thisItem, currPlayer );
			}
		}


		if ( AllTI != null )
		{
			while ( AllTI.Count > 0 )
			{
				setItemToPlayer ( AllTI [ 0 ], currPlayer );

				AllTI.RemoveAt ( 0 );
			}
		}
	}

	void setItemToPlayer ( ItemModif thisItem, PlayerController currPlayer )
	{
		if ( thisItem.ModifSpecial )
		{
			currPlayer.ThisAct = thisItem.SpecAction;

			if ( thisItem.SpecAction == SpecialAction.SlowMot )
			{
				currPlayer.SlowMotion = thisItem.SlowMotion;
				currPlayer.SpeedSlowMot = thisItem.SpeedSlowMot;
				currPlayer.SpeedDeacSM = thisItem.SpeedDeacSM;
				currPlayer.ReduceSlider = thisItem.ReduceSlider;
				currPlayer.RecovSlider = thisItem.RecovSlider;
			}
		}

		if ( thisItem.ModifVie )
		{
			currPlayer.Life += thisItem.NombreVie;
		}
	}
	#endregion
}


[System.Serializable]
public class FxList 
{
	public string FxName;
	public GameObject FxObj;
}