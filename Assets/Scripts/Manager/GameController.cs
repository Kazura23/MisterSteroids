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
    public bool GameStarted;
    #endregion

    #region Mono
    #endregion

    #region Public Methods
	public void StartGame ( )
	{
		Player = GameObject.FindGameObjectWithTag("Player");

		SpawnerChunck.FirstSpawn ( );
        Player.GetComponent<PlayerController>().MaxSpeed = 0;
        Camera.main.GetComponent<RainbowRotate>().time = 2;
        Camera.main.GetComponent<RainbowMove>().time = 1;
		GlobalManager.Ui.CloseThisMenu ( );
    }

    public void Restart ( ) 
	{
		SceneManager.LoadScene ( "ProtoAlex", LoadSceneMode.Single );
        GameStarted = false;
        StartGame ( );
    }

    void Update()
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
                Player.GetComponent<PlayerController>().MaxSpeed = 15;
                Camera.main.GetComponent<RainbowRotate>().time = .4f;
                Camera.main.GetComponent<RainbowMove>().time = .2f;
            }
        }
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
