using UnityEngine;

public class SpawnNewLvl : MonoBehaviour 
{
	#region Variable
	public Transform LevelParent;

	bool detect = false;
	#endregion
	
	#region Mono
	#endregion
		
	#region Public
	#endregion
	
	#region Private
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player"&& !detect) 
		{
			detect = true;
			GlobalManager.GameCont.SpawnerChunck.NewSpawn ( LevelParent );
		}
	}
	#endregion
}
