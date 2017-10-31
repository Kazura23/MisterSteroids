using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : ManagerParent
{
	#region Variables
	public GameObject LoadScenetry;

	#endregion

	#region Mono

	#endregion

	#region Public Methods
	public void LoadThisScene ( string thisScene )
	{
		GlobalManager.Ui.CloseThisMenu ( );
		SceneManager.LoadScene ( thisScene, LoadSceneMode.Single );
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
	{
		switch ( scene.name )
		{
		case "ProtoAlex":
			GlobalManager.GameCont.StartGame ( );
			break;
		case "HomeMenu":
			GlobalManager.Ui.OpenThisMenu ( MenuType.MenuHome );
			break;
		}
	}

	void OnNewLevel  ( string thisScene )
	{
		
	}
	#endregion

	#region Private Methods
	protected override void InitializeManager ( )
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	#endregion
}
