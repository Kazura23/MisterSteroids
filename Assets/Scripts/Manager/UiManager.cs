using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using System.Diagnostics;
using System.Runtime.CompilerServices;

public class UiManager : ManagerParent
{
	#region Variables
	#endregion

	#region Mono
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	protected override void InitializeManager ( )
	{
		InitializeUI ( );
	}

	void InitializeUI ( )
	{
		
	}

	void InitializeUI<T>(ref T manager) where T : UiParent
	{
		//Debug.Log("Initializing managers");
		T[] managers = GetComponentsInChildren<T>();

		if(managers.Length == 0)
		{
			//Debug.LogError("No manager of type: " + typeof(T) + " found.");
			return;
		}

		//Set to first manager
		manager = managers[0];
		manager.Initialize();

		if(managers.Length > 1) //Too many managers
		{
			//Debug.LogError("Found " + managers.Length + " UI of type " + typeof(T));
			for(int i = 1; i < managers.Length; i++)
			{
				Destroy(managers[i].gameObject);
			}
		} 
	}
	#endregion
}
