using UnityEngine;

public class SpawnNewLvl : MonoBehaviour 
{
	#region Variable
	[Tooltip ("X = nombre de lane a gauche et Y à droite ( ne pas inclure la ligne ou est attaché le script )")]
	public Vector2 NbrLaneDebut, NbrLaneFin;
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
